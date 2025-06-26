using Diary.Models.Categories;
using Diary.Models.DiaryEntry;

public class CreateDiaryEntryViewModel
{
    public CreateEntryDto Entry { get; set; }
    public List<CategoryDto> Categories { get; set; }
}
