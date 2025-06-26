using Diary.Data;
using Diary.Models.DiaryEntry;
using Diary.Models.Entities;
using Diary.Services;
using Diary.Services.DiaryEntry;
using Microsoft.EntityFrameworkCore;

namespace Diary.Services;

public class DiaryEntryService : IDiaryEntryService
{
    private readonly ApplicationDbContext _context;
    private readonly LoggedUserService _loggedUserService;

    public DiaryEntryService(ApplicationDbContext context, LoggedUserService loggedUserService)
    {
        _context = context;
        _loggedUserService = loggedUserService;
    }

    public List<GetAllDiaryEntriesViewModel> GetAll()
    {
        var query = _context.DiaryEntries.AsQueryable();

        // If user is not logged in, show only public entries
        if (_loggedUserService.User == null)
        {
            query = query.Where(x => !x.IsPrivate);
        }
        else
        {
            // If user is logged in, show all public entries and their own private entries
            query = query.Where(x => !x.IsPrivate || x.UserId == _loggedUserService.User.Id);
        }

        return query
            .Include(x => x.user)
            .Include(d => d.Category)
            .Select(x => new GetAllDiaryEntriesViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                DiaristFirstName = x.user.FirstName,
                DiaristLastName = x.user.LastName,
                IsPrivate = x.IsPrivate,
                CategoryType = x.Category.Type
            })
            .ToList();
    }

    public GetDiaryEntryViewModel GetById(int id)
    {
        DiaryEntryEntity diaryEntry = GetDiaryEntryById(id);

        if (diaryEntry == null)
        {
            throw new InvalidOperationException("Diary entry not found.");
        }

        // Check if the entry is private and the user is not the owner
        if (diaryEntry.IsPrivate && 
            (_loggedUserService.User == null || _loggedUserService.User.Id != diaryEntry.UserId))
        {
            throw new InvalidOperationException("You are not authorized to view this private entry.");
        }

        var diaryEntryViewModel = new GetDiaryEntryViewModel
        {
            Id = diaryEntry.Id,
            Description = diaryEntry.Description,
            DiaristFirstName = diaryEntry.user.FirstName,
            DiaristLastName = diaryEntry.user.LastName,
            IsPrivate = diaryEntry.IsPrivate
        };

        return diaryEntryViewModel;
    }

    public void Create(CreateEntryDto createEntryDto)
    {
        if (_loggedUserService.User == null)
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        DiaryEntryEntity diaryEntry = new DiaryEntryEntity
        {
            Description = createEntryDto.Description,
            UserId = _loggedUserService.User.Id,
            IsPrivate = createEntryDto.IsPrivate,
            CategoryId = createEntryDto.CategoryId
        };

        _context.DiaryEntries.Add(diaryEntry);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateDiaryDto updateDiaryDto)
    {
        DiaryEntryEntity diaryEntry = GetDiaryEntryById(id);

        if (diaryEntry == null)
        {
            throw new InvalidOperationException("Diary entry not found.");
        }

        if (_loggedUserService.User == null || _loggedUserService.User.Id != diaryEntry.UserId)
        {
            throw new InvalidOperationException("You are not authorized to edit this entry.");
        }

        diaryEntry.Description = updateDiaryDto.Description;
        diaryEntry.IsPrivate = updateDiaryDto.IsPrivate;

        _context.DiaryEntries.Update(diaryEntry);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        DiaryEntryEntity diaryEntry = GetDiaryEntryById(id);

        if (diaryEntry == null)
        {
            throw new InvalidOperationException("Diary entry not found.");
        }

        if (_loggedUserService.User == null || _loggedUserService.User.Id != diaryEntry.UserId)
        {
            throw new InvalidOperationException("You are not authorized to delete this entry.");
        }

        _context.DiaryEntries.Remove(diaryEntry);
        _context.SaveChanges();
    }

    private DiaryEntryEntity GetDiaryEntryById(int id)
    {
        return _context.DiaryEntries
            .Include(d => d.user)
            .FirstOrDefault(d => d.Id == id);
    }
}