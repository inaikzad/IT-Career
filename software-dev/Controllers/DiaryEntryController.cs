using Diary.Models.DiaryEntry;
using Diary.Services.Category;
using Diary.Services.DiaryEntry;
using Microsoft.AspNetCore.Mvc;

namespace Diary.Controllers;

public class DiaryEntryController : Controller
{
    private readonly IDiaryEntryService diaryEntryService;
    private readonly ICategoryService categoryService;
    
    public DiaryEntryController(IDiaryEntryService diaryEntryService, ICategoryService categoryService)
    {
        this.diaryEntryService = diaryEntryService;
        this.categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<GetAllDiaryEntriesViewModel> allDiaryEntries = diaryEntryService.GetAll();
        return View(allDiaryEntries);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CreateDiaryEntryViewModel
        {
            Entry = new CreateEntryDto(),
            Categories = categoryService.GetAll()
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(CreateEntryDto entry)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new CreateDiaryEntryViewModel
            {
                Entry = entry,
                Categories = categoryService.GetAll()
            };
            return View(viewModel);
        }

        diaryEntryService.Create(entry);
        return RedirectToAction("GetAll");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        try
        {
            var diaryEntry = diaryEntryService.GetById(id);
            if (diaryEntry == null)
            {
                return NotFound();
            }

            var viewModel = new EditDiaryEntryViewModel
            {
                Id = id,
                Description = diaryEntry.Description,
                IsPrivate = diaryEntry.IsPrivate,
                CategoryId = diaryEntry.CategoryId,
                Categories = categoryService.GetAll()
            };
            return View(viewModel);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Edit(int id, EditDiaryEntryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = categoryService.GetAll();
            return View(model);
        }

        try
        {
            var updateDto = new UpdateDiaryDto
            {
                Description = model.Description,
                IsPrivate = model.IsPrivate,
                CategoryId = model.CategoryId
            };
            
            diaryEntryService.Update(id, updateDto);
            return RedirectToAction("GetAll");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var diaryEntry = diaryEntryService.GetById(id);
        if (diaryEntry == null)
        {
            return NotFound();
        }
        return View(diaryEntry);
    }

    [HttpPost]
    public IActionResult DeleteDiaryEntry(int id)
    {
        try
        {
            diaryEntryService.Delete(id);
            return RedirectToAction("GetAll");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}