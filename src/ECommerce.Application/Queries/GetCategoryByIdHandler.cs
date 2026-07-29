using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Queries
{
    public sealed class GetCategoryByIdHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Result<CategoryDto>> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            if (query.Id == Guid.Empty)
            {
                return Result<CategoryDto>.Failure("Category id cannot be empty");
            }

            var category = await _categoryRepository.GetByIdAsync(query.Id, ct);

            if (category is null)
            {
                return Result<CategoryDto>.NotFound("Category not found");
            }

            var dto = category.ToDto();

            return Result<CategoryDto>.Success(dto);
        }
    }
}