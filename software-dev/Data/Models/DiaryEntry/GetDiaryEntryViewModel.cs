namespace Diary.Models.DiaryEntry;

public class GetDiaryEntryViewModel
{
    public int? CategoryId;
    public int Id { get; set; }
    public string Description { get; set; }
    public string DiaristFirstName { get; set; }
    public string DiaristLastName { get; set; }
    public bool IsPrivate { get; set; }
}