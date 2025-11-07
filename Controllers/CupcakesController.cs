using CupcakeStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CupcakeStore.Controllers
{
    public class CupcakesController : Controller
    {
        private readonly AppDbContext _context;

        public CupcakesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(CategoriaCupcake? categoria)
        {
            var query = _context.Cupcakes.AsQueryable();

            if (categoria.HasValue)
                query = query.Where(c => c.Categoria == categoria);

            ViewBag.CategoriaSelecionada = categoria?.ToString();
            ViewBag.Categorias = Enum.GetValues(typeof(CategoriaCupcake)).Cast<CategoriaCupcake>();

            var cupcakes = query.OrderBy(c => c.Nome).ToList();
            return View(cupcakes);
        }

        public IActionResult Details(int id)
        {
            var cupcake = _context.Cupcakes.FirstOrDefault(c => c.Id == id);
            if (cupcake == null) return NotFound();
            return View(cupcake);
        }
    }
}
