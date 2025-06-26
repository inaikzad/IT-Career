namespace Diary.Models.DiaryEntry;

public class UpdateDiaryDto
{
    public int? CategoryId;
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
}