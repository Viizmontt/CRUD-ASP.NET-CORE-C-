using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace P001.Models.ViewModels
{
	public class ProductViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio")]
		[StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
		public required string Nombre { get; set; }

		[Required(ErrorMessage = "La categoría es obligatoria")]
		[Display(Name = "Categoría")]
		public int IdCategory { get; set; }

		[Required(ErrorMessage = "El precio es obligatorio")]
		[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe ser un valor decimal con hasta dos decimales")]
		[Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser un valor positivo mayor que 0 y menor que 10 millones")]
		public decimal Precio { get; set; }
	}
}