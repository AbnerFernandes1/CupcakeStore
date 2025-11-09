using CupcakeStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=CupcakeStore.db"));
builder.Services.AddAuthentication("ClienteCookie")
    .AddCookie("ClienteCookie", options =>
    {
        options.LoginPath = "/Login"; // redirecionamento para login se necessário
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AcessoNegado";
    });


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cupcakes}/{action=Home}/{id?}");

app.Run();
