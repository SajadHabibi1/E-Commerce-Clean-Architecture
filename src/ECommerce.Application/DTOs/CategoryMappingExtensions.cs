using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public static class CategoryMappingExtensions
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto(
                category.Id,
                category.Name,
                category.Description,
                category.IsActive
            );
        }
    }
}