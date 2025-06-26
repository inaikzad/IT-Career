using Diary.Models.Entities;

namespace Diary.Models.DiaryEntry;

public class GetAllDiaryEntriesViewModel
{
    public object Id { get; set; }
    public string Description { get; set; }
    public string DiaristFirstName { get; set; }
    public string DiaristLastName { get; set; }
    public bool IsPrivate { get; set; }
    public CategoryType? CategoryType { get; set; }
}