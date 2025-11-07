using CupcakeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeStore.Controllers
{
    [Authorize(AuthenticationSchemes = "ClienteCookie")]
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Obtém o ID do cliente logado a partir das Claims
            var clienteIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (clienteIdClaim == null)
                return RedirectToAction("Index", "Login");

            int clienteId = int.Parse(clienteIdClaim.Value);

            // Filtra os pedidos do cliente logado
            var pedidos = _context.Pedidos
                .Where(p => p.ClienteId == clienteId)
                .Include(p => p.Endereco)
                .OrderByDescending(p => p.Data)
                .ToList();

            return View(pedidos);
        }

        public IActionResult Detalhes(int id)
        {
            var clienteId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var pedido = _context.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Cupcake)
                .Include(p => p.Cliente)
                .Include(p => p.Endereco)
                .FirstOrDefault(p => p.Id == id && p.ClienteId == clienteId);

            if (pedido == null) return NotFound();
            return View(pedido);
        }

        public IActionResult Avaliar(int id)
        {
            var clienteId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var pedido = _context.Pedidos
                .Include(p => p.Avaliacao)
                .FirstOrDefault(p => p.Id == id && p.ClienteId == clienteId);
                
            if (pedido == null) return NotFound();
            if (pedido.Avaliacao != null)
            {
                TempData["Aviso"] = "Pedido já foi avaliado.";
                return RedirectToAction("Detalhes", new { id });
            }

            ViewBag.PedidoId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Avaliar(int pedidoId, int nota, string comentario)
        {
            var pedido = _context.Pedidos.Find(pedidoId);
            if (pedido == null) return NotFound();

            var avaliacao = new Avaliacao
            {
                PedidoId = pedidoId,
                Nota = nota,
                Comentario = comentario
            };

            _context.Avaliacoes.Add(avaliacao);
            _context.SaveChanges();

            TempData["Sucesso"] = "Avaliação registrada com sucesso!";
            return RedirectToAction("Detalhes", new { id = pedidoId });
        }
    }
}
