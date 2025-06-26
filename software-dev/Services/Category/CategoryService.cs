using Diary.Data;
using Diary.Models.Categories;

namespace Diary.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<CategoryDto> GetAll()
    {
        return _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Type = c.Type,
                Description = c.Description
            })
            .ToList();
    }
}