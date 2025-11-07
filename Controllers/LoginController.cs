using CupcakeStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class LoginController : Controller
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginClienteViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginClienteViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var cliente = _context.Clientes
            .FirstOrDefault(c => c.Email == model.Email && c.Telefone == model.Telefone);

        if (cliente == null)
        {
            ModelState.AddModelError("", "Dados inválidos ou cliente não encontrado.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Name, cliente.Nome),
            new Claim(ClaimTypes.Email, cliente.Email)
        };

        var identity = new ClaimsIdentity(claims, "ClienteCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("ClienteCookie", principal);

        return RedirectToAction("Index", "Cupcakes");
    }
    
    [Authorize(AuthenticationSchemes = "ClienteCookie")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("ClienteCookie");
        return RedirectToAction("Index", "Cupcakes");
    }
}
