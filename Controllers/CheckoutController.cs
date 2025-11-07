using System.Globalization;
using System.Security.Claims;
using CupcakeStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Cliente cliente = null;
            Endereco ultimoEndereco = null;

            if (User.Identity.IsAuthenticated)
            {
                var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

                ultimoEndereco = _context.Enderecos
                    .Where(e => e.ClienteId == clienteId)
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefault();
            }

            ViewBag.Cliente = cliente;
            ViewBag.Endereco = ultimoEndereco;

            return View(); // retorna Index.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(string nome, string email, string telefone,
                                                   string rua, string numero, string bairro, string cidade, string uf, string cep, string complemento,
                                                   string formaPagamento)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("CARRINHO");
            if (carrinho == null || !carrinho.Any())
            {
                TempData["ErroCheckout"] = "Carrinho vazio.";
                return RedirectToAction("Index");
            }

            // Calcula subtotal e desconto
            decimal subtotal = carrinho.Sum(i => i.Quantidade * i.Cupcake.Preco);
            decimal desconto = 0;
            var descontoStr = HttpContext.Session.GetString("CupomDesconto");
            if (!string.IsNullOrEmpty(descontoStr))
            {
                decimal.TryParse(descontoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out desconto);
            }
            decimal total = subtotal - desconto;

            Cliente cliente;
            if (User.Identity.IsAuthenticated)
            {
                var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

                if (cliente != null)
                {
                    cliente.Nome = nome;
                    cliente.Email = email;
                    cliente.Telefone = telefone;
                    _context.Update(cliente);
                }
            }
            else
            {
                // Verifica se o cliente já existe pelo e-mail
                cliente = _context.Clientes.FirstOrDefault(c => c.Email == email);

                if (cliente == null)
                {
                    cliente = new Cliente { Nome = nome, Email = email, Telefone = telefone };
                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();
                }

                // Autentica o cliente (se não estava logado)
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Name, cliente.Nome),
            new Claim(ClaimTypes.Email, cliente.Email)
        };

                var identidade = new ClaimsIdentity(claims, "ClienteCookie");
                var principal = new ClaimsPrincipal(identidade);
                await HttpContext.SignInAsync("ClienteCookie", principal);
            }

            // Cria endereço
            var endereco = new Endereco
            {
                ClienteId = cliente.Id,
                Rua = rua,
                Numero = numero,
                Bairro = bairro,
                Cidade = cidade,
                UF = uf,
                CEP = cep,
                Complemento = complemento
            };
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();

            // Cria pedido
            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                EnderecoId = endereco.Id,
                FormaPagamento = formaPagamento,
                Total = total,
                Status = "Em preparação"
            };
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            // Adiciona itens do pedido
            foreach (var item in carrinho)
            {
                _context.PedidoItens.Add(new PedidoItem
                {
                    PedidoId = pedido.Id,
                    CupcakeId = item.CupcakeId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.Cupcake.Preco
                });
            }

            _context.SaveChanges();

            // Limpa carrinho e cupom
            HttpContext.Session.Remove("CARRINHO");
            HttpContext.Session.Remove("CupomCodigo");
            HttpContext.Session.Remove("CupomDesconto");

            TempData["SucessoPedido"] = $"Pedido #{pedido.Id} realizado com sucesso!";
            return RedirectToAction("Confirmacao", new { id = pedido.Id });
        }

        public IActionResult Confirmacao(int id)
        {
            var pedido = _context.Pedidos
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Data,
                    p.Total,
                    p.FormaPagamento,
                    Cliente = p.Cliente.Nome
                })
                .FirstOrDefault();

            if (pedido == null) return NotFound();

            ViewBag.Pedido = pedido;
            return View();
        }
    }
}
