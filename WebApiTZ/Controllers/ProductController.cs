using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiTZ.Models;

namespace WebApiTZ.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ILogger<ProductController> _logger;
		public FilterModel Filter { get; set; }
		public IEnumerable<ProductModel> Products { get; private set; }

		private readonly ProductService _service;
		public ProductController(ProductService service, ILogger<ProductController> logger)
		{
			_service = service;
			_logger = logger;
			_logger.LogDebug(1, "NLog injected into HomeController");
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

			_logger.LogInformation(JsonSerializer.Serialize(Products));

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

			_logger.LogInformation(JsonSerializer.Serialize(Inputs));

			return Ok();

		}
	}
}
