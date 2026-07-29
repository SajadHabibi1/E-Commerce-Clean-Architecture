using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.Commands
{
    public sealed class CreateCategoryHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand cmd, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(cmd);
            
            if (await _categoryRepository.ExistsByNameAsync(cmd.Name, ct: ct))
            {
                return Result<Guid>.Failure("Category name already exists");
            }

            try
            {
                var category = new Category(
                    cmd.Name,
                    cmd.Description
                );

                await _categoryRepository.AddAsync(category, ct);
                return Result<Guid>.Success(category.Id);
            }
            catch(DomainException ex)
            {
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}