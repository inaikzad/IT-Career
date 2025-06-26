using Diary.Controllers;
using Diary.Models.Categories;
using Diary.Models.DiaryEntry;
using Diary.Services.Category;
using Diary.Services.DiaryEntry;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Diary.Tests
{
    public partial class DiaryEntryControllerTests
    {
        
        [Fact] // Check that the view is returned
        public void GetAll_CallsServiceAndReturnsView()
        {
            // Arrange
            var diaryService = Substitute.For<IDiaryEntryService>();
            var mockCategoryService = Substitute.For<ICategoryService>();
            var testController = new DiaryEntryController(diaryService, mockCategoryService);
            var expectedEntries = new List<GetAllDiaryEntriesViewModel>();
            diaryService.GetAll().Returns(expectedEntries);

            // Act
            var result = testController.GetAll();

            // Assert
            Assert.IsType<ViewResult>(result);
            diaryService.Received(1).GetAll();
        }
        
        [Fact] // Check when we pass a valid id, edit returns a view
        public void Edit_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var diaryService = Substitute.For<IDiaryEntryService>();
            var mockCategoryService = Substitute.For<ICategoryService>();
            var testController = new DiaryEntryController(diaryService, mockCategoryService);
            
            var diaryEntry = new GetDiaryEntryViewModel // Specific properties for more functionality
            { 
                Description = "Test",
                IsPrivate = false,
                CategoryId = 1
            };
            diaryService.GetById(1).Returns(diaryEntry);
            mockCategoryService.GetAll().Returns(new List<CategoryDto>());

            // Act
            var result = testController.Edit(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }


        [Fact] // Check when we pass a valid id, delete returns a view
        public void Delete_WithValidId_ReturnsViewResult()
        {
            // Arrange
            var diaryService = Substitute.For<IDiaryEntryService>();
            var mockCategoryService = Substitute.For<ICategoryService>();
            var testController = new DiaryEntryController(diaryService, mockCategoryService);
            diaryService.GetById(1).Returns(new GetDiaryEntryViewModel()); // Default values only for confirmation
            

            // Act
            var result = testController.Delete(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}