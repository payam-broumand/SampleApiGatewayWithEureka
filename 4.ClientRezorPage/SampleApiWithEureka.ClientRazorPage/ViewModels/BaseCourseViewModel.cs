using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SampleApiWithEureka.ClientRazorPage.ViewModels
{
	public abstract class BaseCourseViewModel
	{
		[Display(Name = "شناسه")]
		public virtual int Id { get; set; }

		[Display(Name = "عنوان")]
		public virtual string Title { get; set; }

		[Display(Name = "قیمت")]
		public virtual double Price { get; set; }


		[Display(Name = "دسته آموزشی")]
		public virtual int CategoryId { get; set; }

		public List<SelectListItem>? Categories { get; set; }
	}

	public abstract class ParentWithChildListViewModel<T, C>
		where T : class
		where C : class
	{
		public T? Parent { get; set; }
		public IReadOnlyList<C>? ChildList { get; set; }
	}
}
