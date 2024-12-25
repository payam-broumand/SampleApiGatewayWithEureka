using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.CourseService.Models;
using System.Text.Json;

namespace SampleApiWithEureka.CourseService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CourseController : ControllerBase
	{
		private readonly CourseRepository _courseRepository;

		public CourseController(CourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<Course>))]
		public IActionResult Get()
		{
			IReadOnlyList<Course> courses = _courseRepository.Courses;
			string result = JsonSerializer.Serialize(courses);

			return Ok(result);
		}

		[HttpGet("find/{id?}")] 
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		public IActionResult GetById(int? id)
		{
			Course? course = _courseRepository.GetById(id ?? 0);
			return course is null ? NotFound("محصولی یافت نشد") : Ok(course);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		public IActionResult Post(Course? model)
		{
			if (model is null) return BadRequest("course model is null");
			Course? course = _courseRepository.Add(model);
			return course is not null ? Ok(course) : BadRequest("an error occured in creating new course");
		}

		[HttpPut("{id?}")]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		public IActionResult Put(int? id, Course? model)
		{
			if (model is null) return BadRequest("course model is null");
			Course? course = _courseRepository.Update(id ?? 0, model);
			return course is not null ? Ok(course) : NotFound("course not found");
		}

		[HttpDelete("{id?}")]
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))] 
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		public IActionResult Delete(int? id)
		{
			Course? course = _courseRepository.GetById(id ?? 0);
			if (course is null) return NotFound("course not found");
			_courseRepository.Delete(course);
			return Ok();
		}

		[HttpGet("getbycategory/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<Course>))] 
		public IActionResult GetCoursesByCategoryId(int? id)
		{
			IReadOnlyList<Course> courses = _courseRepository.GetByCategoryId(id ?? 0);
			return Ok(courses);
		}
	}
}
