using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary.Models.Entities;

public class DiaryEntryEntity
{
    [Key]
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User user { get; set; }
    
    [ForeignKey("Category")]
    public int? CategoryId { get; set; }
    
    public CategoryEntity? Category { get; set; }
}