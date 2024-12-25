using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleApiWithEureka.CategoryService.Models;
using SampleApiWithEureka.ClientRazorPage.ViewModels;
using System.Text.Json;

namespace SampleApiWithEureka.ClientRazorPage.Pages
{
	public class BasePageModel : PageModel
	{
		protected readonly ILogger<BasePageModel> _logger;
		protected readonly IHttpClientFactory _httpClientFactory;
		protected HttpClient _client;
		private readonly string _clientName;
		private readonly HttpClient _categoryClient;

		private List<string> _messages;

		public MessageModel ErrorModel { get; private set; } = new MessageModel();

		public BasePageModel(
			ILogger<BasePageModel> logger,
			IHttpClientFactory httpClientFactory,
			string clientName)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
			_clientName = clientName;
			_client = _httpClientFactory.CreateClient(_clientName);

			_categoryClient = _httpClientFactory.CreateClient("category");

			_messages = new List<string>();

			//if (TempData.Keys.Any(k => k == "message") && 
			//	!string.IsNullOrEmpty(TempData["message"]?.ToString() ?? ""))
			//{
			//	_messages.Add(TempData["message"].ToString());
			//}
		}

		protected void BindMessages(
			ModelStateDictionary? modelState = null,
			List<string>? listMessages = null,
			MessageStatus status = MessageStatus.info,
			string logMessage = "Error in model validation")
		{
			if (modelState is not null && !ModelState.IsValid)
			{
				_messages = ModelState.Values
					.SelectMany(a => a.Errors)
					.Select(e => e.ErrorMessage).ToList();

			}
			else if(listMessages is not null && listMessages.Count > 0)
			{
				_messages = listMessages;
			}

			_logger.LogError(logMessage);
			ErrorModel = _messages.AddToMessageModel(status);
		}

		protected void AddModelStateError(
			string errorMessage,
			ModelStateDictionary modelState,
			string logMessage = "Error in model validation")
		{
			if (string.IsNullOrEmpty(errorMessage)) return;
			 
			modelState.AddModelError(string.Empty, errorMessage);
			BindMessages(modelState, status: MessageStatus.danger, logMessage: logMessage);
		}

		protected void AddMessage(string message, string logMessage = "Operation is done")
		{
			_logger.LogInformation(logMessage);
			BindMessages(null, [message], MessageStatus.success);
		}

		protected async Task<BaseCourseViewModel?> BindCourseCategories(BaseCourseViewModel editModel)
		{
			HttpResponseMessage response = await _categoryClient.GetAsync("api/category");
			if (!response.IsSuccessStatusCode)
			{
				BindMessages(null, [response.Content.ReadAsStringAsync().Result], MessageStatus.danger);
				return null;
			}

			string result = await response.Content.ReadAsStringAsync();
			IReadOnlyList<Category> categories = JsonSerializer.Deserialize<IReadOnlyList<Category>>(result) ?? [];
			editModel.Categories = categories.Select(c => new SelectListItem(c.Title, c.Id.ToString())).ToList();

			return editModel;
		}
	}
}
