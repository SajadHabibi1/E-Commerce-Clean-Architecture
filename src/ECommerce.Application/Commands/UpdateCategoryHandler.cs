using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Commands
{
    public sealed class UpdateCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public UpdateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Result<Guid>> HandleAsync(UpdateCategoryCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            if (cmd.Id == Guid.Empty)
            {
                return Result<Guid>.Failure("Category id cannot be empty");
            }

            try
            {
                var category = await _categoryRepository.GetByIdAsync(cmd.Id, ct);

                if (category is null)
                {
                    return Result<Guid>.NotFound("Category not found");
                }

                if (await _categoryRepository.ExistsByNameAsync(cmd.Name, cmd.Id, ct))
                {
                    return Result<Guid>.Failure("Category name already exists");
                }

                category.Edit(
                    cmd.Name,
                    cmd.Description
                );

                await _categoryRepository.UpdateAsync(category, ct);
                return Result<Guid>.Success(category.Id);
            }
            catch(DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}