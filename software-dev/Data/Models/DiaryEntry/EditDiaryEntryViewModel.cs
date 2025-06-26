using Diary.Models.Categories;

public class EditDiaryEntryViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
    public int? CategoryId { get; set; }
    public List<CategoryDto> Categories { get; set; }
}