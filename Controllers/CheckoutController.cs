using System.Globalization;
using System.Security.Claims;
using CupcakeStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

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

            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("CARRINHO");
            if (carrinho == null || !carrinho.Any())
            {
                TempData["ErroCheckout"] = "Seu carrinho está vazio.";
                return RedirectToAction("Index", "Carrinho");
            }

            foreach (var item in carrinho)
            {
                item.Cupcake = _context.Cupcakes.Find(item.CupcakeId);
            }

            ViewBag.Cliente = cliente;
            ViewBag.Endereco = ultimoEndereco;

            return View(carrinho);
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(
            string nome, string email, string telefone,
            string rua, string numero, string bairro, string cidade, string uf, string cep, string complemento,
            string formaPagamento)
        {
            // ⚙️ 1. Carrinho
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("CARRINHO");
            if (carrinho == null || !carrinho.Any())
            {
                TempData["ErroCheckout"] = "Seu carrinho está vazio.";
                return RedirectToAction("Index", "Carrinho");
            }

            // ⚙️ 2. Validação mínima
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(rua) ||
                string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(cidade) ||
                string.IsNullOrWhiteSpace(uf) || string.IsNullOrWhiteSpace(cep))
            {
                TempData["ErroCheckout"] = "Por favor, preencha todos os campos obrigatórios.";
                return RedirectToAction("Index", "Checkout");
            }

            // ⚙️ 3. Cálculo total
            decimal subtotal = carrinho.Sum(i => i.Quantidade * i.Cupcake.Preco);
            decimal desconto = 0M;
            var descontoStr = HttpContext.Session.GetString("CupomDesconto");
            if (!string.IsNullOrEmpty(descontoStr))
                decimal.TryParse(descontoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out desconto);

            decimal frete = 5M;
            decimal total = subtotal - desconto + frete;

            // ⚙️ 4. Cliente — garante que existe
            Cliente cliente = null;

            // tenta buscar por usuário logado
            if (User.Identity?.IsAuthenticated == true)
            {
                var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                cliente = await _context.Clientes.FindAsync(clienteId);
            }

            // tenta buscar por email
            if (cliente == null)
                cliente = _context.Clientes.FirstOrDefault(c => c.Email == email);

            // cria novo cliente se necessário
            if (cliente == null)
            {
                cliente = new Cliente
                {
                    Nome = nome.Trim(),
                    Email = email.Trim(),
                    Telefone = telefone.Trim()
                };
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
            }
            else
            {
                // atualiza dados
                cliente.Nome = nome.Trim();
                cliente.Telefone = telefone.Trim();
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }

            // ⚙️ 5. Endereço
            var endereco = new Endereco
            {
                ClienteId = cliente.Id,
                Rua = rua.Trim(),
                Numero = numero.Trim(),
                Bairro = bairro?.Trim() ?? "",
                Cidade = cidade.Trim(),
                UF = uf.Trim(),
                CEP = cep.Trim(),
                Complemento = complemento?.Trim() ?? ""
            };
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            // ⚙️ 6. Pedido
            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                EnderecoId = endereco.Id,
                FormaPagamento = formaPagamento,
                Total = total,
                Status = "Em preparação",
                Data = DateTime.Now
            };
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // ⚙️ 7. Itens do pedido
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
            await _context.SaveChangesAsync();

            // ⚙️ 8. Limpa sessão
            HttpContext.Session.Remove("CARRINHO");
            HttpContext.Session.Remove("CupomCodigo");
            HttpContext.Session.Remove("CupomDesconto");

            // ⚙️ 9. Faz login do cliente, se ainda não estiver logado
            if (User.Identity?.IsAuthenticated != true)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Name, cliente.Nome),
            new Claim(ClaimTypes.Email, cliente.Email)
        };

                var identity = new ClaimsIdentity(claims, "ClienteCookie");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("ClienteCookie", principal);
            }

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
