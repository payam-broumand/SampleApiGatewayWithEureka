using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SampleApiWithEureka.CategoryService.Models
{
	public class Category
	{
		[JsonPropertyName("Id")]
		public int Id { get; set; }

		[JsonPropertyName("Title")]
		[Required(ErrorMessage = "عنوان دسته بندی را وارد کنید")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "عنوان باید بین 3 تا 100 کاراکتر باشد")]
		[Display(Name = "عنوان دوره")]
		public string Title { get; set; }
	}
}
