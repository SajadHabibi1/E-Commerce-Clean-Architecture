using ECommerce.Api.Contracts.Categories;
using ECommerce.Application.Commands;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Endpoints
{
    public static class CategoriesEndpoints
    {
        private static async Task<Results<Ok<IReadOnlyList<CategoryDto>>, ProblemHttpResult>> GetAll(
            [FromServices] GetAllCategoriesHandler handler,
            CancellationToken ct
        )
        {
            var result = await handler.HandleAsync(new GetAllCategoriesQuery(), ct);

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Get categories failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Ok(result.Value);
        }

        private static async Task<Results<Ok<CategoryDto>, ProblemHttpResult>> GetById(
            Guid Id,
            [FromServices]GetCategoryByIdHandler handler,
            CancellationToken ct
        )
        {
            var result = await handler.HandleAsync(new GetCategoryByIdQuery(Id), ct);

            if (result.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Category not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Category not found"
                });
            }

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Get category failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Ok(result.Value);
        }

        private static async Task<Results<Created<CreateCategoryResponse>, ProblemHttpResult>> Create(
            [FromBody] CreateCategoryRequest request,
            [FromServices] CreateCategoryHandler handler,
            CancellationToken ct
        )
        {
            var cmd = new CreateCategoryCommand(
                request.Name,
                request.Description
            );

            var result = await handler.HandleAsync(cmd, ct);

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Create category failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.Created(
                $"/categories/{result.Value}",
                new CreateCategoryResponse(result.Value)
            );
        }

        private static async Task<Results<Ok<CreateCategoryResponse>, ProblemHttpResult>> Update(
            Guid id,
            [FromBody] UpdateCategoryRequest request,
            [FromServices] UpdateCategoryHandler handler,
            CancellationToken ct
        )
        {
            var cmd = new UpdateCategoryCommand(
                id,
                request.Name,
                request.Description
            );

            var result = await handler.HandleAsync(cmd, ct);

            if (result.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Category not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Category not found"
                });
            }

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Edit category failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }
            
            return TypedResults.Ok(new CreateCategoryResponse(id));
        }

        private static async Task<Results<NoContent, ProblemHttpResult>> Delete(
            Guid id,
            [FromServices] DeleteCategoryHandler handler,
            CancellationToken ct
        )
        {
            var cmd = new DeleteCategoryCommand(id);

            var result = await handler.HandleAsync(cmd, ct);

            if (result.ErrorType == ErrorType.NotFound)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Category not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Category not found"
                });
            }

            if (result.IsFailure)
            {
                return TypedResults.Problem(new ProblemDetails
                {
                    Title = "Delete category failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error
                });
            }

            return TypedResults.NoContent();
        }

        public static IEndpointRouteBuilder MapCategoriesEndpoints( this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/categories")
            .WithTags("Categories");

            group.MapGet("/", GetAll)
            .Produces<IReadOnlyList<CategoryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:guid}", GetById)
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPost("/", Create)
            .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", Update)
            .Produces<CreateCategoryResponse>(StatusCodes.Status200OK)
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