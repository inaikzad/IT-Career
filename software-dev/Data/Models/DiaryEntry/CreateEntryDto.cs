namespace Diary.Models.DiaryEntry;

public class CreateEntryDto
{
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
    
    public int? CategoryId { get; set; }
}