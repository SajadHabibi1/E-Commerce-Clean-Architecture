using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Queries
{
    public sealed class GetAllCategoriesHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Result<IReadOnlyList<CategoryDto>>> HandleAsync(GetAllCategoriesQuery query, CancellationToken ct = default)
        {
            var categories = await _categoryRepository.GetAllAsync(ct);

            var dtos = categories.Select(category => category.ToDto()).ToList();

            return Result<IReadOnlyList<CategoryDto>>.Success(dtos);
        }
    }
}