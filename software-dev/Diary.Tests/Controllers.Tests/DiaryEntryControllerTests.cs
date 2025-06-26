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
        private readonly IDiaryEntryService diaryEntryService;
        private readonly ICategoryService categoryService;
        private readonly DiaryEntryController controller;

        public DiaryEntryControllerTests() // Constructor
        {
            this.diaryEntryService = Substitute.For<IDiaryEntryService>();
            this.categoryService = Substitute.For<ICategoryService>();
            this.controller = new DiaryEntryController(diaryEntryService, categoryService);
        }

        [Fact] // Check that the view is returned
        public void Create_ReturnsViewResult() 
        {
            // Arrange - nothing needed, setup is in constructor

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
        
        [Fact] // Check the interaction between the controller and the service by the GetAll method
        public void GetAll_CallsDiaryEntryService() 
        {
            // Act
            controller.GetAll();

            // Assert
            diaryEntryService.Received(1).GetAll();
        }

        [Fact] // Check that the view is returned with the correct model
        public void Create_WithInvalidModelState_ReturnsViewResultWithViewModel()
        {
            // Arrange
            controller.ModelState.AddModelError("Error", "Sample error");
            var entry = new CreateEntryDto();
            var categories = new List<CategoryDto>();
            categoryService.GetAll().Returns(categories);

            // Act
            var result = controller.Create(entry) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var viewModel = Assert.IsType<CreateDiaryEntryViewModel>(result.Model);
            Assert.Same(entry, viewModel.Entry);
            Assert.Same(categories, viewModel.Categories);
        }


        
    }
}