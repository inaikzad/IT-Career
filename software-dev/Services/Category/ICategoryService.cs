using Diary.Models.Categories;

namespace Diary.Services.Category;

public interface ICategoryService
{
    List<CategoryDto> GetAll();
}