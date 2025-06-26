using Diary.Data;
using Diary.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.Services;

public static class DbSeeder
{
    public static void SeedCategories(ApplicationDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = Enum.GetValues<CategoryType>()
                .Select(type => new CategoryEntity
                {
                    Type = type,
                    Description = type.ToString()
                })
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}