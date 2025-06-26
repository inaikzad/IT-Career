using System.ComponentModel.DataAnnotations;

namespace Diary.Models.Entities;

public class CategoryEntity
{
    [Key]
    public int Id { get; set; }
    
    public CategoryType Type { get; set; }
    
    public string? Description { get; set; } // can be null
    
    // Navigation for diary entries in the category
    public ICollection<DiaryEntryEntity> DiaryEntries { get; set; }
}