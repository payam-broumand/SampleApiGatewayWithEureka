using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SampleApiWithEureka.ClientRazorPage.ViewModels
{
	public class CreateCourseViewModel : BaseCourseViewModel
	{
		[JsonPropertyName("Id")]
		[Display(Name = "شناسه")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "شناسه دوره وارد نشده است")]
		public override int Id { get; set; }

		[JsonPropertyName("Title")]
		[Display(Name = "عنوان")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "عنوان دوره را وارد کنید")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "عنوان باید بین 3 تا 100 کاراکتر باشد")]
		public override string Title { get; set; }

		[JsonPropertyName("Price")]
		[Display(Name = "قیمت")]
		[Required(ErrorMessage = "قیمت دوره را وارد کنید")]
		[RegularExpression("^(0)|(?!(?:\\d{1,2}|1000)$)[0-9]\\d+$", ErrorMessage = "قیمت دوره باید 0 یا بزرگتر از 1000 باشد")]
		public override double Price { get; set; }


		[JsonPropertyName("CategoryId")]
		[Display(Name = "دسته آموزشی")]
		[Required(ErrorMessage = "دسته دوره را وارد کنید")]
		[RegularExpression("^([1-9])*$", ErrorMessage = "دسته دوره را انتخاب کنید")]
		public override int CategoryId { get; set; }
	}
}
