using ECommerce.Api.Contracts.Carts;
using ECommerce.Application.Commands;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Endpoints
{
    public static class CartsEndpoints
    {
        private static async Task<Results<Ok<CartDto>, ProblemHttpResult>> GetCart(
            Guid customerId,
            [FromServices] GetCartByCustomerIdHandler handler,
            CancellationToken ct
        )
        {
            var result = await handler.HandleAsync(new GetCartByCustomerIdQuery(customerId), ct);

            return TypedResults.Ok(result.Value);
        }

        private static async Task<Results<Ok<CartDto>, ProblemHttpResult>> AddItem(
            Guid customerId,
            [FromBody] AddItemToCartRequest request,
            [FromServices] AddItemToCartHandler addHandler,
            [FromServices] GetCartByCustomerIdHandler getHandler,
            CancellationToken ct
        )
        {
            var cmd = new AddItemToCartCommand(
                customerId,
                request.ProductId,
                request.Quantity
            );

            var addResult = await addHandler.HandleAsync(cmd, ct);

            if (addResult.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Product not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Product not found"
                });
            }

            if (addResult.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Add item to cart failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = addResult.Error
                });
            }

            var cartResult = await getHandler.HandleAsync(new GetCartByCustomerIdQuery(customerId), ct);
            
            return TypedResults.Ok(cartResult.Value);
        }

        private static async Task<Results<Ok<CartDto>, ProblemHttpResult>> RemoveItem(
            Guid customerId,
            Guid cartItemId,
            [FromServices] RemoveItemFromCartHandler removeHandler,
            [FromServices] GetCartByCustomerIdHandler getHandler,
            CancellationToken ct
        )
        {
            var cmd = new RemoveItemFromCartCommand(customerId, cartItemId);
            var removeResult = await removeHandler.HandleAsync(cmd, ct);

            if (removeResult.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Cart not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Cart not found"
                });
            }

            if (removeResult.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Remove item from cart failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = removeResult.Error
                });
            }

            var cartResult = await getHandler.HandleAsync(new GetCartByCustomerIdQuery(customerId), ct);

            return TypedResults.Ok(cartResult.Value);
        }

        private static async Task<Results<Ok<CartDto>, ProblemHttpResult>> UpdateItemQuantity(
            Guid customerId,
            Guid cartItemId,
            [FromBody] UpdateCartItemQuantityRequest request,
            [FromServices] UpdateCartItemQuantityHandler updateHandler,
            [FromServices] GetCartByCustomerIdHandler getHandler,
            CancellationToken ct
        )
        {
            var cmd = new UpdateCartItemQuantityCommand(customerId, cartItemId, request.Quantity);

            var updateResult = await updateHandler.HandleAsync(cmd, ct);

            if (updateResult.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Cart not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Cart not found"
                });
            }

            if (updateResult.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Update cart item quantity failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = updateResult.Error
                });
            }

            var cartResult = await getHandler.HandleAsync(new GetCartByCustomerIdQuery(customerId), ct);

            return TypedResults.Ok(cartResult.Value);
        }

        public static IEndpointRouteBuilder MapCartsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/customers/{customerId:guid}/cart")
            .WithTags("Carts");

            group.MapGet("/", GetCart)
            .Produces<CartDto>(StatusCodes.Status200OK);

            group.MapPost("/items", AddItem)
            .Produces<CartDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapDelete("/items/{cartItemId:guid}", RemoveItem)
            .Produces<CartDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPatch("/items/{cartItemId:guid}", UpdateItemQuantity)
            .Produces<CartDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            return app;
        }
    }
}