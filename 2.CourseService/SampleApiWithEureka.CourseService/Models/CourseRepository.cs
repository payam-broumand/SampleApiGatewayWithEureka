namespace SampleApiWithEureka.CourseService.Models
{
	public class CourseRepository
	{
		private readonly List<Course> _courses;

		public IReadOnlyList<Course> Courses => _courses;

		public CourseRepository()
		{
			_courses = new List<Course>
			{
				new Course {Id = 1, Title = "برنامه نویسی مقدماتی سی شارپ", Price = 1_800_000, CategoryId = 1},
				new Course {Id = 2, Title = "آموزش جامع الگوهای طراحی در سی شارپ", Price = 1_900_000, CategoryId = 1},
				new Course {Id = 2, Title = "آموزش تزریق وابستگی در Asp.Net Core", Price = 250_000, CategoryId = 2},
				new Course {Id = 2, Title = "برنامه نویسی پیشرفته سی شارپ", Price = 2_000_000, CategoryId = 1},
			};
		}

		public Course Add(Course course)
		{
			course.Id = _courses.Count + 1;
			_courses.Add(course);

			return course;
		}

		public Course? GetById(int id) => _courses.Find(a => a.Id == id);

		public Course? Update(int id, Course model)
		{
			Course? course = GetById(id);
			if (course is null) return null;

			course.Title = model.Title;
			course.Price = model.Price;
			course.CategoryId = model.CategoryId;

			return course;
		}

		public void Delete(Course course) => _courses.Remove(course);

		public IReadOnlyList<Course> GetByCategoryId(int id)
			=> _courses.Where(a => a.CategoryId == id).ToList();
	}
}
