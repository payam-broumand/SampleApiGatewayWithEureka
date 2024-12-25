using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.ClientRazorPage.ViewModels;
using SampleApiWithEureka.CourseService.Models;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Courses
{
	public class DeleteModel : BasePageModel
    {  
		public DeleteModel(
			ILogger<BasePageModel> logger,
			IHttpClientFactory httpClientFactory)
			: base(logger, httpClientFactory, "course")
		{ 
		}

		[BindProperty]
		public DeleteCourseViewModel? DeleteCourseViewModel{ get; set; }

		public async Task<IActionResult> OnGet(int? id)
        {
            if (id is null or 0) {
                AddModelStateError("دوره ای یافت نشد", ModelState, "course id is null");
                return RedirectToPage("Index");
            }

			HttpResponseMessage response = await _client.GetAsync($"api/course/find/{id}");
            if (!response.IsSuccessStatusCode)
            {
                AddModelStateError(
                    "دوره آموزشی یافت نشد",
                    ModelState,
                    response.Content.ReadAsStringAsync().Result);
                return RedirectToPage("Index");
            }

            Course? course = JsonSerializer.Deserialize<Course>(response.Content.ReadAsStringAsync().Result);
            if(course is null)
            {
				AddModelStateError(
				   "دوره آموزشی یافت نشد",
				   ModelState,
				   response.Content.ReadAsStringAsync().Result);
				return RedirectToPage("Index");
			}

            DeleteCourseViewModel = new DeleteCourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Price = course.Price,
                CategoryId = course.CategoryId
            };
			BaseCourseViewModel? baseCourseViewModel = await BindCourseCategories(DeleteCourseViewModel);
            DeleteCourseViewModel = baseCourseViewModel is not null
                ? (DeleteCourseViewModel)baseCourseViewModel
                : DeleteCourseViewModel;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			HttpResponseMessage response = await _client.DeleteAsync($"api/course/{DeleteCourseViewModel.Id}");
            if (!response.IsSuccessStatusCode)
            {
                AddModelStateError(
                    "خطا در حذف دوره آموزشی",
                    ModelState,
                    response.Content.ReadAsStringAsync().Result);

				BaseCourseViewModel? baseCourseViewModel = await BindCourseCategories(DeleteCourseViewModel);
                DeleteCourseViewModel = baseCourseViewModel is not null
                    ? (DeleteCourseViewModel)baseCourseViewModel
                    : DeleteCourseViewModel;

                return Page();
            }

            AddMessage("دوره با موفقیت حذف گردید", "course deleted successfully");
            return RedirectToPage("Index");
        }
	}
}
