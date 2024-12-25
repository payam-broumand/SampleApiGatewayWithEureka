
using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.ClientRazorPage.ViewModels;
using SampleApiWithEureka.CourseService.Models;
using System.Net.Mime;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Courses
{
	public class  EditModel : BasePageModel
    {  
		public EditModel(
			ILogger<BasePageModel> logger, 
			IHttpClientFactory httpClientFactory) 
			: base(logger, httpClientFactory, "course")
		{ 
		}

		[BindProperty]
		public EditCourseViewModel? EditCourseViewModel { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
        {
			if(id is null or 0)
			{
				AddModelStateError("دوره ای یافت نشد", ModelState);
				return RedirectToPage("Index");
			}

			HttpResponseMessage response = await _client.GetAsync($"api/course/find/{id}");
			if (!response.IsSuccessStatusCode)
			{
				BindMessages(null, [response.Content.ReadAsStringAsync().Result], MessageStatus.danger);
				return RedirectToPage("Index");
			}

			string result = response.Content.ReadAsStringAsync().Result;
			Course? course = JsonSerializer.Deserialize<Course>(result);
			if(course is null)
			{
				BindMessages(null, [response.Content.ReadAsStringAsync().Result], MessageStatus.danger);
				return RedirectToPage("Index");
			}
			EditCourseViewModel = new EditCourseViewModel
			{
				Id = course.Id,
				Title = course.Title,
				Price = course.Price,
				CategoryId = course.CategoryId
			};
			 
			BaseCourseViewModel? baseCourseViewModel = await BindCourseCategories(EditCourseViewModel);
			EditCourseViewModel = baseCourseViewModel is not null
				? (EditCourseViewModel)baseCourseViewModel
				: EditCourseViewModel;
			if(EditCourseViewModel is null) return RedirectToPage("Index"); 

			return Page();

        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				BindMessages(ModelState, status: MessageStatus.danger);

				BaseCourseViewModel? baseCourseViewModel = await BindCourseCategories(EditCourseViewModel);
				EditCourseViewModel = baseCourseViewModel is not null
					? (EditCourseViewModel)baseCourseViewModel
					: EditCourseViewModel; 

				return Page();
			}

			StringContent content = new StringContent(
				JsonSerializer.Serialize(EditCourseViewModel),
				System.Text.Encoding.UTF8,
				MediaTypeNames.Application.Json);
			HttpResponseMessage response = await _client.PutAsync($"api/course/{EditCourseViewModel.Id}", content);

			if (!response.IsSuccessStatusCode)
			{
				BindMessages(null, [response.Content.ReadAsStringAsync().Result], MessageStatus.danger);

				BaseCourseViewModel? baseCourseViewModel = await BindCourseCategories(EditCourseViewModel);
				EditCourseViewModel = baseCourseViewModel is not null
					? (EditCourseViewModel)baseCourseViewModel
					: EditCourseViewModel;

				return Page();
			}

			Course? course = JsonSerializer.Deserialize<Course>(response.Content.ReadAsStringAsync().Result);
			if(course is null)
			{
				AddModelStateError("خطا در ویرایش دوره آموزشی", ModelState);
				return Page();
			}

			AddMessage("دوره آموزشی با موفقیت ویرایش گردید", "Course has been updated successfully");
			return RedirectToPage("Index");
		}
	}
}
