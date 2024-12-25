
using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.CourseService.Models;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Courses
{
	public class IndexModel : BasePageModel
    {
		public IndexModel(
			ILogger<BasePageModel> logger, 
			IHttpClientFactory httpClientFactory) 
			: base(logger, httpClientFactory, "course")
		{
		}

		public IReadOnlyList<Course> Courses { get; set; }

		public async Task<IActionResult> OnGetAsync()
        {
			HttpResponseMessage response = await _client.GetAsync("api/course");
			if (!response.IsSuccessStatusCode)
			{
				AddModelStateError(
					response.Content.ReadAsStringAsync().Result,
					ModelState);
				return RedirectToPage("/Index");
			}

			string result = await response.Content.ReadAsStringAsync();
			Courses = JsonSerializer.Deserialize<IReadOnlyList<Course>>(result) ?? [];
			return Page();
        }
    }
}
