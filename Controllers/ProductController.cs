using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P001.Models;
using P001.Models.ViewModels;

namespace P001.Controllers{
	public class ProductController : Controller{
		private readonly CrudaspContext _context;

		public ProductController(CrudaspContext context){
			_context = context;
		}

		public async Task<IActionResult> Index(){
			try{
				var products = await _context.Products.Include(p => p.IdCategoryNavigation).ToListAsync();
				return View(products);
			}
			catch (Exception ex){
				return StatusCode(500, $"Error interno: {ex.Message}");
			}
		}


		private void LoadCategories(){
			ViewData["categories"] = new SelectList(_context.Categories, "Id", "Name");
		}

		public IActionResult Create(){
			LoadCategories();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductViewModel model) {
			if (!ModelState.IsValid) {
				LoadCategories();
				return View(model);
			}
			var ep = await _context.Products.FirstOrDefaultAsync(p=>p.Nombre.ToLower()==model.Nombre.ToLower());
			if (ep != null){
				ModelState.AddModelError("Nombre", "Producto ya existe");
				LoadCategories();
				return View(model);
			}
			var np = new Product(){
				Nombre = model.Nombre,
				IdCategory = model.IdCategory,
				Precio = model.Precio
			};

			_context.Products.Add(np);
			await _context.SaveChangesAsync();
			TempData["success"] = "Producto agregado";
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> edit(int? id){
			var product = await _context.Products.FindAsync(id);
			var model = new ProductViewModel{
				//Id = product.Id,
				Nombre = product.Nombre,
				IdCategory = product.IdCategory,
				Precio = (decimal)product.Precio
			};
			LoadCategories();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> edit(int id, ProductViewModel model){
			if (!ModelState.IsValid) {
				LoadCategories();
				return View(model);
			}
			var exp = await _context.Products.Where(p=>p.Nombre==model.Nombre && p.Id!=id).FirstOrDefaultAsync();
			if (exp != null) {
				ModelState.AddModelError("Nombre", "Producto ya existe");
				LoadCategories();
				return View(model);
			}
			var product = await _context.Products.FindAsync(id);
			product.Nombre = model.Nombre;
			product.IdCategory = model.IdCategory;
			product.Precio = model.Precio;

			_context.Update(product);
			await _context.SaveChangesAsync();
			TempData["success"] = "Producto actualizado";
			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> delete(int? id) {
			var product = await _context.Products.FindAsync(id);
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			TempData["success"] = "Producto eliminado";
			return RedirectToAction(nameof(Index));	
		}
	}
}