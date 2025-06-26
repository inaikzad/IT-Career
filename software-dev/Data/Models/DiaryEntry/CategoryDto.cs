using Diary.Models.Entities;

namespace Diary.Models.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public CategoryType Type { get; set; }
    public string? Description { get; set; }
}