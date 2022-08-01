using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTZ.Data;
using WebApiTZ.Models;

namespace WebApiTZ
{
	public class ProductService
	{
		readonly AppDbContext _context;

		public ProductService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<int> CreateProducts(List<CreateProductCommand> cmds)
		{
			cmds.Sort();

			_context.Products.RemoveRange(_context.Products);

			foreach (var cmd in cmds)
			{
				var product = cmd.ToProduct();
				_context.Add(product);
			}

			return await _context.SaveChangesAsync(); ;
		}

		public async Task<List<ProductModel>> GetProducts(FilterModel filter)
		{
			var products = await _context.Products
										.Where(x => (filter.ProductId != 0) ? x.ProductId == filter.ProductId : true)
										.Where(x => (filter.Code != 0) ? x.Code == filter.Code : true)
										.Where(x => (filter.Value != null) ? x.Value.Contains(filter.Value) : true)
										.Select(x => new ProductModel
										{
											ProductId = x.ProductId,
											Code = x.Code,
											Value = x.Value,
										})
										.ToListAsync();

			return products;
		}
	}
}
