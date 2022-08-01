using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiTZ.Models;

namespace WebApiTZ.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		public FilterModel Filter { get; set; }
		public IEnumerable<ProductModel> Products { get; private set; }

		private readonly ProductService _service;
		public ProductController(ProductService service)
		{
			_service = service;
		}

		[HttpGet("readFilter")]
		public async Task<IActionResult> Get([FromQuery] FilterModel filter)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Products = await _service.GetProducts(filter);
			if (Products is null)
			{
				return NotFound();
			}
			return new JsonResult(Products);
		}


		[HttpPost("create")]
		public async Task<IActionResult> Post([FromBody] List<CreateProductCommand> Inputs)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _service.CreateProducts(Inputs);

			return Ok();

		}
	}
}
