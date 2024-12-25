using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleApiWithEureka.CategoryService.Models;
using System.Net.Mime;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Categories
{
    public class EditModel : BasePageModel
    {
		public EditModel(
            ILogger<BasePageModel> logger, 
            IHttpClientFactory httpClientFactory) 
            : base(logger, httpClientFactory, "category")
		{
		}

		[BindProperty]
		public Category? Category { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id is null or 0)
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
            if(Category is null)
            {
                _logger.LogError("category not found");
				return RedirectToPage("Index");

			}

            return Page();
		}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = 
                    ModelState.Values
                    .SelectMany(a => a.Errors)
                    .Select(e => e.ErrorMessage)
                    .Aggregate((e, c) => c + ",");
                _logger.LogError(errorMessages);
                return Page();
            }

			StringContent stringContent = new StringContent(
                JsonSerializer.Serialize(Category),
                System.Text.Encoding.UTF8,
                MediaTypeNames.Application.Json);

			HttpResponseMessage response = await _client.PutAsync($"api/category/{Category.Id}", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.Content.ReadAsStringAsync().Result);
                return Page();
            }

            Category = JsonSerializer.Deserialize<Category>(response.Content.ReadAsStringAsync().Result);
            if(Category is null)
            {
                _logger.LogError("error occured in update category");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
