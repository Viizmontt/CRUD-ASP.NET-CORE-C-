using System;
using System.Collections.Generic;

namespace P001.Models;

public partial class Product
{
	public int Id { get; set; }

	public string Nombre { get; set; } = null!;

	public decimal? Precio { get; set; }

	public int IdCategory { get; set; }

	public virtual Category IdCategoryNavigation { get; set; } = null!;
}
