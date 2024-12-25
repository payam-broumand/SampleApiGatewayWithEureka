using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.ClientRazorPage.ViewModels;
using SampleApiWithEureka.CourseService.Models;
using System.Net.Mime;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Courses
{
	public class AddModel : BasePageModel
    {
		public AddModel(
			ILogger<BasePageModel> logger, 
			IHttpClientFactory httpClientFactory)
			: base(logger, httpClientFactory, "course")
		{
		}

		[BindProperty]
		public CreateCourseViewModel CreateCourseModel { get; set; } = new CreateCourseViewModel();

		public async Task OnGetAsync()
        {
			CreateCourseModel = new CreateCourseViewModel();
			CreateCourseModel = (CreateCourseViewModel)(await BindCourseCategories(CreateCourseModel) ?? CreateCourseModel); 
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				BindMessages(ModelState, status: MessageStatus.danger);
				return Page();
			}

			StringContent content = new StringContent(
				JsonSerializer.Serialize(CreateCourseModel),
				System.Text.Encoding.UTF8,
				MediaTypeNames.Application.Json);
			HttpResponseMessage response = await _client.PostAsync("api/course", content);
			if (!response.IsSuccessStatusCode)
			{
				AddModelStateError(
					"خطا در ثبت دوره آموزشی جدید",
					ModelState,
					response.Content.ReadAsStringAsync().Result);
				CreateCourseModel = (CreateCourseViewModel)(await BindCourseCategories(CreateCourseModel) ?? CreateCourseModel);

				return Page();
			}

			Course? course = JsonSerializer.Deserialize<Course>(await response.Content.ReadAsStringAsync());
			if(course is null)
			{
				AddModelStateError(
									"خطا در ثبت دوره آموزشی جدید",
									ModelState,
									response.Content.ReadAsStringAsync().Result);
				CreateCourseModel = (CreateCourseViewModel)(await BindCourseCategories(CreateCourseModel) ?? CreateCourseModel);
				return Page();
			}

			AddMessage("دوره آموزشی با موفقیت ذخیره گردید", "New course has been added successfully");
			return RedirectToPage("Index");
		}
    }
}
