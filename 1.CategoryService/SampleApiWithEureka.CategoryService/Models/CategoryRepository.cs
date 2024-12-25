namespace SampleApiWithEureka.CategoryService.Models
{
	public class CategoryRepository
	{
		private readonly List<Category> _categories;

		public IReadOnlyList<Category> Categories => _categories;

		public CategoryRepository()
		{
			_categories = new List<Category>
			{
				new() {Id = 1, Title = "برنامه نویسی دالت نت"},
				new() {Id = 2, Title = "Asp.Net Core"}
			};
		}

		public Category Add(Category category)
		{
			category.Id = _categories.Count + 1;
			_categories.Add(category);

			return category;
		}

		public Category? GetById(int id) => _categories.Find(a => a.Id == id);

		public Category? Update(int id, Category model)
		{
			Category? category = GetById(id);
			if (category is null) return null;

			category.Title = model.Title;
			return category;
		}

		public void Delete(Category category) => _categories.Remove(category);
	}
}
