using System.ComponentModel.DataAnnotations;

namespace Diary.Models.Entities;

public class User
{
    [Key]
    public int Id  { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email  { get; set; }
    public string FirstName  { get; set; }
    public string LastName { get; set; }
    
    public ICollection<DiaryEntryEntity> DiaryEntries { get; set; }
}