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

        public IActionResult Home(CategoriaCupcake? categoria)
        {
            var query = _context.Cupcakes.AsQueryable();

            if (categoria.HasValue)
                query = query.Where(c => c.Categoria == categoria);

            ViewBag.CategoriaSelecionada = categoria?.ToString();
            ViewBag.Categorias = Enum.GetValues(typeof(CategoriaCupcake)).Cast<CategoriaCupcake>();

            var cupcakes = query.OrderBy(c => c.Nome).ToList();
            return View(cupcakes);
        }

        public IActionResult Contato()
        {
            return View("Contato");
        }

        public IActionResult Index(CategoriaCupcake? categoria, string termo, string ordenarPor)
        {
            var query = _context.Cupcakes.AsQueryable();

            if (categoria.HasValue)
                query = query.Where(c => c.Categoria == categoria);

            if (!string.IsNullOrWhiteSpace(termo))
                query = query.Where(c => c.Nome.ToUpper().Contains(termo.ToUpper()) || c.Descricao.ToUpper().Contains(termo.ToUpper()));

            ordenarPor ??= "Nome";
            query = ordenarPor switch
            {
                "PrecoAsc"  => query.OrderBy(c => (double)c.Preco),
                "PrecoDesc" => query.OrderByDescending(c => (double)c.Preco),
                _           => query.OrderBy(c => c.Nome)
            };

            ViewBag.CategoriaSelecionada = categoria?.ToString();
            ViewBag.Categorias = Enum.GetValues(typeof(CategoriaCupcake)).Cast<CategoriaCupcake>();
            ViewBag.OrdenarPor = ordenarPor;
            ViewBag.TermoBusca = termo;

            var cupcakes = query.ToList();
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
