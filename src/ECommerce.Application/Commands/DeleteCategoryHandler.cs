using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Commands
{
    public sealed class DeleteCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Result<Guid>> HandleAsync(DeleteCategoryCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            if (cmd.Id == Guid.Empty)
            {
                return Result<Guid>.Failure("Category id cannot be empty");
            }

            var category = await _categoryRepository.GetByIdAsync(cmd.Id, ct);

            if (category is null)
            {
                return Result<Guid>.NotFound("Category not found");
            }

            category.SoftDelete();
            await _categoryRepository.UpdateAsync(category, ct);

            return Result<Guid>.Success(category.Id);
        }
    }
}