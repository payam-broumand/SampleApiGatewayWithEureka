using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.CategoryService.Models;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Categories
{
	public class IndexModel : BasePageModel
    {
		public IndexModel(
            ILogger<BasePageModel> logger, 
            IHttpClientFactory httpClientFactory) 
            : base(logger, httpClientFactory, "category")
		{
		}

		public IReadOnlyList<Category> Categories { get; set; }

		public async Task<IActionResult> OnGet()
        {
			HttpResponseMessage response = await _client.GetAsync("api/category");
            if (!response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();

				_logger.LogError(message);
                return RedirectToPage("/Index");
            }

            string result = await response.Content.ReadAsStringAsync();
			Categories = JsonSerializer.Deserialize<IReadOnlyList<Category>>(result) 
                ?? new List<Category>();

            return Page();
        }
    }
}
