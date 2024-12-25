using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SampleApiWithEureka.CategoryService.Models;
using System.Diagnostics.Metrics;
using System.Net.Mime;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages.Categories
{
	public class AddModel : BasePageModel
    {
		public AddModel(
			ILogger<BasePageModel> logger, 
			IHttpClientFactory httpClientFactory)
			: base(logger, httpClientFactory, "category")
		{
		}

		[BindProperty]
		public Category? Category{ get; set; } = new Category();


		public void OnGet()
        { 
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				BindMessages(modelState: ModelState, status: MessageStatus.danger);
				return Page();
			}

			StringContent stringContent = new StringContent(
				JsonSerializer.Serialize(Category),
				System.Text.Encoding.UTF8,
				MediaTypeNames.Application.Json);
			HttpResponseMessage response = await _client.PostAsync("api/category", stringContent);
			if (!response.IsSuccessStatusCode)
			{
				string responseMessage = response.Content.ReadAsStringAsync().Result;
				_logger.LogError(responseMessage);
				BindMessages(null, [responseMessage], MessageStatus.danger);
				return Page();
			}

			string result = await response.Content.ReadAsStringAsync();
			Category = JsonSerializer.Deserialize<Category>(result);
			if(Category is null)
			{
				AddModelStateError("خطا در ثبت دسته بندی جدید", ModelState);
				return Page();
			}

			//TempData["message"] = "دسته بندی با موفقیت ذخیره گردید";
			AddMessage("دسته بندی با موفقیت ذخیره گردید");

			return RedirectToPage("Index");
		}
    }
}
