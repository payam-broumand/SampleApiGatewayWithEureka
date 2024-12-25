using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleApiWithEureka.CategoryService.Models;
using System.Net.Mime;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Categories
{
	public class DeleteModel : BasePageModel
	{
		public DeleteModel(
			ILogger<BasePageModel> logger,
			IHttpClientFactory httpClientFactory)
			: base(logger, httpClientFactory, "category")
		{
		}

		[BindProperty]
		public Category? Category { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id is null or 0)
			{
				_logger.LogError("category id is null");
				return RedirectToPage("Index");
			}

			HttpResponseMessage response = await _client.GetAsync($"api/category/find/{id}");
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError(response.Content.ReadAsStringAsync().Result);
				return RedirectToPage("Index");
			}

			string result = await response.Content.ReadAsStringAsync();
			Category = JsonSerializer.Deserialize<Category>(result);
			if (Category is null)
			{
				_logger.LogError(response.Content.ReadAsStringAsync().Result);
				return RedirectToPage("Index");
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			HttpResponseMessage response = await _client.DeleteAsync($"api/category/{Category.Id}");
			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError(response.Content.ReadAsStringAsync().Result);
				return Page();
			}

			_logger.LogInformation("Category has been deleted successfully");
			return RedirectToPage("Index");
		}
	}
}
