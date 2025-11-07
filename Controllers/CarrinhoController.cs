using System.Globalization;
using CupcakeStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CupcakeStore.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly AppDbContext _context;
        private const string CarrinhoKey = "CARRINHO";

        public CarrinhoController(AppDbContext context)
        {
            _context = context;
        }

        private List<CarrinhoItem> ObterCarrinho()
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>(CarrinhoKey);
            return carrinho ?? new List<CarrinhoItem>();
        }

        private void SalvarCarrinho(List<CarrinhoItem> itens)
        {
            HttpContext.Session.SetObjectAsJson(CarrinhoKey, itens);
        }

        public IActionResult Index()
        {
            var itens = ObterCarrinho();
            foreach (var item in itens)
            {
                item.Cupcake = _context.Cupcakes.Find(item.CupcakeId);
            }

            ViewBag.Total = itens.Sum(i => i.Quantidade * i.Cupcake.Preco);
            return View(itens);
        }

        public IActionResult Adicionar(int id)
        {
            var cupcake = _context.Cupcakes.Find(id);
            if (cupcake == null) return NotFound();

            var carrinho = ObterCarrinho();
            var existente = carrinho.FirstOrDefault(c => c.CupcakeId == id);

            if (existente != null)
                existente.Quantidade++;
            else
                carrinho.Add(new CarrinhoItem { CupcakeId = id, Quantidade = 1, Cupcake = cupcake });

            SalvarCarrinho(carrinho);
            return RedirectToAction("Index");
        }

        public IActionResult Remover(int id)
        {
            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(c => c.CupcakeId == id);
            if (item != null) carrinho.Remove(item);

            SalvarCarrinho(carrinho);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Diminuir(int id)
        {
            var carrinho = ObterCarrinho();
            var item = carrinho.FirstOrDefault(c => c.CupcakeId == id);

            if (item != null)
            {
                item.Quantidade--;

                if (item.Quantidade <= 0)
                    carrinho.Remove(item);
            }

            SalvarCarrinho(carrinho);
            return RedirectToAction("Index");
        }

        public IActionResult Limpar()
        {
            SalvarCarrinho(new List<CarrinhoItem>());
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AplicarCupom(string codigo)
        {
            var cupom = _context.Cupons.FirstOrDefault(c => c.Codigo == codigo);
            if (cupom == null || !cupom.EstaValido())
            {
                TempData["ErroCupom"] = "Cupom inválido ou expirado.";
                return RedirectToAction("Index");
            }

            var carrinho = ObterCarrinho();
            decimal total = carrinho.Sum(i => i.Quantidade * _context.Cupcakes.Find(i.CupcakeId).Preco);

            decimal desconto = cupom.CalcularDesconto(total);
            cupom.RegistrarUso();
            _context.SaveChanges();

            // 🔧 grava na Session
            HttpContext.Session.SetString("CupomCodigo", cupom.Codigo);
            HttpContext.Session.SetString("CupomDesconto", desconto.ToString(CultureInfo.InvariantCulture));

            TempData["SucessoCupom"] = $"Cupom {cupom.Codigo} aplicado! Desconto de R$ {desconto:F2}";
            return RedirectToAction("Index");
        }
    }
}
