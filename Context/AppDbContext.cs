using Microsoft.EntityFrameworkCore;

namespace CupcakeStore.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cupcake> Cupcakes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<CupomDesconto> Cupons { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relações
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Enderecos)
                .WithOne(e => e.Cliente)
                .HasForeignKey(e => e.ClienteId);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Avaliacao)
                .WithOne(a => a.Pedido)
                .HasForeignKey<Avaliacao>(a => a.PedidoId);

            modelBuilder.Entity<Cupcake>().HasData(
                new Cupcake { Id = 1, Nome = "Cupcake de Chocolate", Descricao = "Cobertura de brigadeiro e massa de chocolate", Preco = 8.50m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/chocolate.jpg", Destaque = true },
                new Cupcake { Id = 2, Nome = "Cupcake de Morango", Descricao = "Recheado com geleia de morango e cobertura de chantilly", Preco = 9.00m, Categoria = CategoriaCupcake.Recheado, ImagemUrl = "/img/morango.jpg", Destaque = true },
                new Cupcake { Id = 3, Nome = "Cupcake Vegano de Limão", Descricao = "Sem ingredientes de origem animal", Preco = 10.00m, Categoria = CategoriaCupcake.Vegano, ImagemUrl = "/img/limao.jpg", Destaque = false },
                new Cupcake { Id = 4, Nome = "Cupcake de Red Velvet", Descricao = "Clássico red velvet com cobertura de cream cheese", Preco = 11.50m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/redvelvet.jpg", Destaque = true },
                new Cupcake { Id = 5, Nome = "Cupcake de Cenoura com Chocolate", Descricao = "Massa de cenoura e cobertura de chocolate cremoso", Preco = 8.00m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/cenoura.jpg", Destaque = true },
                new Cupcake { Id = 6, Nome = "Cupcake de Coco com Doce de Leite", Descricao = "Recheado com doce de leite e cobertura de coco ralado", Preco = 9.50m, Categoria = CategoriaCupcake.Recheado, ImagemUrl = "/img/coco.jpg", Destaque = false },
                new Cupcake { Id = 7, Nome = "Cupcake de Baunilha com Frutas Vermelhas", Descricao = "Cobertura de frutas vermelhas e massa de baunilha", Preco = 9.20m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/frutasvermelhas.jpg", Destaque = false },
                new Cupcake { Id = 8, Nome = "Cupcake de Amendoim", Descricao = "Cobertura de pasta de amendoim cremosa", Preco = 8.80m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/amendoim.jpg", Destaque = false },
                new Cupcake { Id = 9, Nome = "Cupcake de Nutella", Descricao = "Recheado e coberto com Nutella", Preco = 11.00m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/nutella.jpg", Destaque = true },
                new Cupcake { Id = 10, Nome = "Cupcake de Maracujá", Descricao = "Cobertura de mousse de maracujá e calda azedinha", Preco = 9.50m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/maracuja.jpg", Destaque = false },
                new Cupcake { Id = 11, Nome = "Cupcake de Oreo", Descricao = "Cobertura cremosa com pedaços de biscoito Oreo", Preco = 10.50m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/oreo.jpg", Destaque = true },
                new Cupcake { Id = 12, Nome = "Cupcake de Leite Ninho com Nutella", Descricao = "Recheado com Nutella e cobertura de Leite Ninho", Preco = 11.90m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/leiteninho.jpg", Destaque = true },
                new Cupcake { Id = 13, Nome = "Cupcake de Café", Descricao = "Massa de café com cobertura de chantilly de cappuccino", Preco = 9.80m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/cafe.jpg", Destaque = false },
                new Cupcake { Id = 14, Nome = "Cupcake de Pistache", Descricao = "Massa suave e cobertura cremosa de pistache", Preco = 12.00m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/pistache.jpg", Destaque = false },
                new Cupcake { Id = 15, Nome = "Cupcake de Laranja com Gengibre", Descricao = "Sabor cítrico com toque de gengibre natural", Preco = 9.00m, Categoria = CategoriaCupcake.Vegano, ImagemUrl = "/img/laranja.jpg", Destaque = false },
                new Cupcake { Id = 16, Nome = "Cupcake de Doce de Leite com Nozes", Descricao = "Recheado com doce de leite e cobertura crocante de nozes", Preco = 10.50m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/nozes.jpg", Destaque = false },
                new Cupcake { Id = 17, Nome = "Cupcake de Banana com Canela", Descricao = "Sabor caseiro com toque de canela", Preco = 8.50m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/banana.jpg", Destaque = false },
                new Cupcake { Id = 18, Nome = "Cupcake de Chocolate Branco", Descricao = "Cobertura cremosa de chocolate branco e raspas finas", Preco = 10.00m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/chocolatebranco.jpg", Destaque = true },
                new Cupcake { Id = 19, Nome = "Cupcake de Menta com Chocolate", Descricao = "Sabor refrescante com pedaços de chocolate meio amargo", Preco = 9.70m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/menta.jpg", Destaque = false },
                new Cupcake { Id = 20, Nome = "Cupcake de Framboesa com Limão Siciliano", Descricao = "Equilíbrio perfeito entre doce e cítrico", Preco = 11.20m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/framboesa.jpg", Destaque = false },
                new Cupcake { Id = 21, Nome = "Cupcake de Chocolate Amargo", Descricao = "70% cacau com cobertura de ganache", Preco = 10.00m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/chocolateamargo.jpg", Destaque = false },
                new Cupcake { Id = 22, Nome = "Cupcake de Baunilha Clássico", Descricao = "Tradicional massa de baunilha com cobertura leve", Preco = 7.90m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/baunilha.jpg", Destaque = true },
                new Cupcake { Id = 23, Nome = "Cupcake de Paçoca", Descricao = "Cobertura de paçoca e farofa crocante", Preco = 9.30m, Categoria = CategoriaCupcake.Tradicional, ImagemUrl = "/img/pacoca.jpg", Destaque = false },
                new Cupcake { Id = 24, Nome = "Cupcake de Limão Siciliano com Coco", Descricao = "Cobertura cítrica e toque suave de coco", Preco = 9.80m, Categoria = CategoriaCupcake.Recheado, ImagemUrl = "/img/limaococo.jpg", Destaque = false },
                new Cupcake { Id = 25, Nome = "Cupcake de Brigadeiro Gourmet", Descricao = "Recheado com brigadeiro e cobertura granulada", Preco = 10.50m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/brigadeiro.jpg", Destaque = true },
                new Cupcake { Id = 26, Nome = "Cupcake de Cheesecake com Morango", Descricao = "Inspirado em cheesecake com calda de morango", Preco = 12.00m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/cheesecake.jpg", Destaque = false },
                new Cupcake { Id = 27, Nome = "Cupcake Vegano de Frutas Tropicais", Descricao = "Sem leite e ovos, com manga e maracujá", Preco = 10.50m, Categoria = CategoriaCupcake.Vegano, ImagemUrl = "/img/tropicais.jpg", Destaque = false },
                new Cupcake { Id = 28, Nome = "Cupcake de Chocolate com Pimenta", Descricao = "Combinação ousada e levemente picante", Preco = 9.90m, Categoria = CategoriaCupcake.Especial, ImagemUrl = "/img/chocolatepimenta.jpg", Destaque = false },
                new Cupcake { Id = 29, Nome = "Cupcake de Caramelo Salgado", Descricao = "Recheado com caramelo salgado e cobertura amanteigada", Preco = 11.30m, Categoria = CategoriaCupcake.Gourmet, ImagemUrl = "/img/caramelo.jpg", Destaque = true },
                new Cupcake { Id = 30, Nome = "Cupcake de Abacaxi com Hortelã", Descricao = "Sabor tropical e refrescante", Preco = 9.60m, Categoria = CategoriaCupcake.Vegano, ImagemUrl = "/img/abacaxi.jpg", Destaque = false }
            );

            modelBuilder.Entity<CupomDesconto>()
                .HasOne(c => c.Cupcake)
                .WithMany()
                .HasForeignKey(c => c.CupcakeId);

            modelBuilder.Entity<CupomDesconto>().HasData(
                new CupomDesconto
                {
                    Id = 1,
                    Codigo = "PROMO10",
                    Valor = 10,
                    EhValorFixo = false,
                    ExpiraEm = DateTime.Now.AddMonths(1),
                    Ativo = true,
                    UsoMaximo = 100
                },
                new CupomDesconto
                {
                    Id = 2,
                    Codigo = "CUPCAKE5",
                    Valor = 5,
                    EhValorFixo = true,
                    ExpiraEm = DateTime.Now.AddMonths(2),
                    Ativo = true
                }
            );

        }
    }
}
