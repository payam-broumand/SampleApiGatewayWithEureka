using Microsoft.AspNetCore.Mvc;
using SampleApiWithEureka.CategoryService.Models;
using System.Text.Json;

namespace SampleApiWithEureka.CategoryService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly CategoryRepository _categoryRepository;

		public CategoryController(CategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<Category>))]
		public IActionResult Get()
		{
			IReadOnlyList<Category> categories = _categoryRepository.Categories;
			string result = JsonSerializer.Serialize(categories);

			return Ok(result);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
		public IActionResult Post(Category? model)
		{
			if (model is null) return BadRequest("category model is null");
			Category category = _categoryRepository.Add(model);
			return Ok(category);
		}

		[HttpGet("find/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))] 
		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
		public IActionResult GetById(int? id)
		{
			Category? category = _categoryRepository.GetById(id ?? 0);
			return category is null ? NotFound("category not found") : Ok(category);
		}

		[HttpPut("{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public IActionResult Put(int? id, Category model)
		{
			Category? category = _categoryRepository.Update(id ?? 0, model);
			return category is not null ? Ok(category) : BadRequest("error category update");
		}

		[HttpDelete("{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
		public IActionResult Delete(int? id)
		{
			Category? category = _categoryRepository.GetById(id ?? 0);
			if (category is null) return NotFound("category not found");

			_categoryRepository.Delete(category);
			return Ok();
		}
	}
}
