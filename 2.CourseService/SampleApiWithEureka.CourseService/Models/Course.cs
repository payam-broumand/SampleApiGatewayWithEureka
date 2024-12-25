using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SampleApiWithEureka.CourseService.Models
{
	public class Course
	{
		[JsonPropertyName("Id")]
		public int Id { get; set; }

		[JsonPropertyName("Title")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "عنوان دوره را وارد کنید")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "عنوان باید بین 3 تا 100 کاراکتر باشد")]
		public string Title { get; set; }

		[JsonPropertyName("Price")]
		[Required(ErrorMessage = "قیمت دوره را وارد کنید")]
		[RegularExpression("^(0)|(?!(?:\\d{1,2}|1000)$)[0-9]\\d+$", ErrorMessage = "قیمت دوره باید 0 یا بزرگتر از 1000 باشد")]
		public double Price { get; set; }


		[JsonPropertyName("CategoryId")]
		[Required(ErrorMessage = "دسته دوره را وارد کنید")]
		[RegularExpression("^[1-9]*$", ErrorMessage = "دسته دوره را انتخاب کنید")]
		public int CategoryId { get; set; }
	}
}
