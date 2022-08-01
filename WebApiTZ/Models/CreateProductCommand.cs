using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiTZ.Data;

namespace WebApiTZ.Models
{
	public class CreateProductCommand : IComparable<CreateProductCommand>
	{
		[Required]
		public int Code { get; set; }
		[Required]
		public string Value { get; set; }

		public Product ToProduct()
		{
			return new Product
			{
				Code = Code,
				Value = Value,

			};
		}

		public int CompareTo(CreateProductCommand compareCreateProductCommand)
		{

			if (compareCreateProductCommand == null)
				return 1;

			else
				return this.Code.CompareTo(compareCreateProductCommand.Code);
		}
	}
}
