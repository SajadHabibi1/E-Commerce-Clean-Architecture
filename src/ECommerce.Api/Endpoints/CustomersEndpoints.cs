using ECommerce.Api.Contracts.Customers;
using ECommerce.Application.Commands;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Endpoints
{
    public static class CustomersEndpoints
    {
        private static async Task<Results<Ok<IReadOnlyList<CustomerDto>>, ProblemHttpResult>> GetAll(
            [FromServices] GetAllCustomersHandler handler,
            CancellationToken ct
        )
        {
            var result = await handler.HandleAsync(new GetAllCustomersQuery(), ct);

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Get customers failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Ok(result.Value);
        }

        private static async Task<Results<Ok<CustomerDto>, ProblemHttpResult>> GetById(
            Guid id,
            [FromServices] GetCustomerByIdHandler handler,
            CancellationToken ct
        )
        {
            var result = await handler.HandleAsync(new GetCustomerByIdQuery(id), ct);

            if (result.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Customer not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Customer not found"
                });
            }

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Get customer failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Ok(result.Value);
        }

        private static async Task<Results<Created<CreateCustomerResponse>, ProblemHttpResult>> Create(
            [FromBody] CreateCustomerRequest request,
            [FromServices] CreateCustomerHandler handler,
            CancellationToken ct
        )
        {
            var cmd = new CreateCustomerCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.Street,
                request.City,
                request.PostalCode,
                request.Country
            );

            var result = await handler.HandleAsync(cmd, ct);

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Create customer failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Created(
                $"/customers/{result.Value}",
                new CreateCustomerResponse(result.Value)
            );
        }

        private static async Task<Results<Ok<CreateCustomerResponse>, ProblemHttpResult>> Update(
            Guid id,
            [FromBody] UpdateCustomerRequest request,
            [FromServices] UpdateCustomerHandler handler,
            CancellationToken ct
        )
        {
            var cmd = new UpdateCustomerCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.Street,
                request.City,
                request.PostalCode,
                request.Country
            );

            var result = await handler.HandleAsync(cmd, ct);

            if (result.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Customer not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Customer not found"
                });
            }

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Edit customer failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Ok(new CreateCustomerResponse(id));
        }

        private static async Task<Results<NoContent, ProblemHttpResult>> Delete(
            Guid id,
            [FromServices] DeleteCustomerHandler handler,
            CancellationToken ct
        )
        {
            var cmd = new DeleteCustomerCommand(id);

            var result = await handler.HandleAsync(cmd, ct);

            if (result.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Customer not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Customer not found"
                });
            }

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Delete customer failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.NoContent();
        }

        public static IEndpointRouteBuilder MapCustomersEndpoints( this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/customers")
            .WithTags("Customers");

            group.MapGet("/", GetAll)
            .Produces<IReadOnlyList<CustomerDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:guid}", GetById)
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPost("/", Create)
            .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", Update)
            .Produces<CreateCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapDelete("/{id:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            return app;
        }
    }
}