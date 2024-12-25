
using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.CategoryService.Models;
using SampleApiWithEureka.ClientRazorPage.ViewModels;
using SampleApiWithEureka.CourseService.Models;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Categories
{
	public class CourseListModel : BasePageModel
    {
		private readonly HttpClient _courseClient;

		public CourseListModel(
			ILogger<BasePageModel> logger, 
			IHttpClientFactory httpClientFactory)
			: base(logger, httpClientFactory, "category")
		{
			_courseClient = _httpClientFactory.CreateClient("course");

			CategoryModel = new CategoryWithCourseViewModel();
		}

		public CategoryWithCourseViewModel CategoryModel { get; private set; }

		public async Task<IActionResult> OnGet(int? id)
        {
			if(id is null or 0)
			{
				AddModelStateError("دسته آموزشی یافت نشد", ModelState, "Category Id is null");
				return RedirectToPage("Index");
			}

			HttpResponseMessage response = await _client.GetAsync($"api/category/find/{id}");
			if (!response.IsSuccessStatusCode)
			{
				AddModelStateError(
					"دسته آموزشی یافت نشد",
					ModelState,
					response.Content.ReadAsStringAsync().Result);
				return RedirectToPage("Index");
			}

			CategoryModel.Parent = JsonSerializer.Deserialize<Category>(response.Content.ReadAsStringAsync().Result);
			if(CategoryModel is null || CategoryModel.Parent is null)
			{
				AddModelStateError(
				"دسته آموزشی یافت نشد",
				ModelState,
				response.Content.ReadAsStringAsync().Result);
				return RedirectToPage("Index");
			}

			response = await _courseClient.GetAsync($"api/course/getbycategory/{CategoryModel?.Parent?.Id ?? 0}");
			if (!response.IsSuccessStatusCode)
			{
				AddModelStateError(
				"دوره های آموزشی دردسته آموزشی یافت نشد",
				ModelState,
				response.Content.ReadAsStringAsync().Result);
				return RedirectToPage("Index");
			}

			string result = await response.Content.ReadAsStringAsync();
			IReadOnlyList<Course>? courses = JsonSerializer.Deserialize<IReadOnlyList<Course>>(result);

			CategoryModel.ChildList = courses;

			_logger.LogInformation($"Category {CategoryModel.Parent.Title} with courses list loaded suceesfully");

			return Page();
        }
    }
}
