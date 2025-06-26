using Diary.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<DiaryEntryEntity> DiaryEntries { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("Server=localhost;Database=diary_db;Uid=root;Pwd=chang521;");
    }
    
    public DbSet<CategoryEntity> Categories { get; set; }
    
}