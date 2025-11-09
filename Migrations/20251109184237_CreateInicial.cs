using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CupcakeStore.Migrations
{
    /// <inheritdoc />
    public partial class CreateInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cupcakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Preco = table.Column<decimal>(type: "TEXT", nullable: false),
                    Categoria = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagemUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Destaque = table.Column<bool>(type: "INTEGER", nullable: false),
                    Estoque = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupcakes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rua = table.Column<string>(type: "TEXT", nullable: false),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    Complemento = table.Column<string>(type: "TEXT", nullable: false),
                    Bairro = table.Column<string>(type: "TEXT", nullable: false),
                    Cidade = table.Column<string>(type: "TEXT", nullable: false),
                    UF = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    CEP = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enderecos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                    EhValorFixo = table.Column<bool>(type: "INTEGER", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiraEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CupcakeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsoMaximo = table.Column<int>(type: "INTEGER", nullable: false),
                    UsoAtual = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cupons_Cupcakes_CupcakeId",
                        column: x => x.CupcakeId,
                        principalTable: "Cupcakes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    FormaPagamento = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnderecoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nota = table.Column<int>(type: "INTEGER", nullable: false),
                    Comentario = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    CupcakeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Cupcakes_CupcakeId",
                        column: x => x.CupcakeId,
                        principalTable: "Cupcakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cupcakes",
                columns: new[] { "Id", "Categoria", "Descricao", "Destaque", "Estoque", "ImagemUrl", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, 1, "Cobertura de brigadeiro e massa de chocolate", true, 10, "/img/chocolate.jpg", "Cupcake de Chocolate", 8.50m },
                    { 2, 2, "Recheado com geleia de morango e cobertura de chantilly", true, 10, "/img/morango.jpg", "Cupcake de Morango", 9.00m },
                    { 3, 3, "Sem ingredientes de origem animal", false, 10, "/img/limao.jpg", "Cupcake Vegano de Limão", 10.00m },
                    { 4, 4, "Clássico red velvet com cobertura de cream cheese", true, 10, "/img/redvelvet.jpg", "Cupcake de Red Velvet", 11.50m },
                    { 5, 1, "Massa de cenoura e cobertura de chocolate cremoso", true, 10, "/img/cenoura.jpg", "Cupcake de Cenoura com Chocolate", 8.00m },
                    { 6, 2, "Recheado com doce de leite e cobertura de coco ralado", false, 10, "/img/coco.jpg", "Cupcake de Coco com Doce de Leite", 9.50m },
                    { 7, 4, "Cobertura de frutas vermelhas e massa de baunilha", false, 10, "/img/frutasvermelhas.jpg", "Cupcake de Baunilha com Frutas Vermelhas", 9.20m },
                    { 8, 1, "Cobertura de pasta de amendoim cremosa", false, 10, "/img/amendoim.jpg", "Cupcake de Amendoim", 8.80m },
                    { 9, 4, "Recheado e coberto com Nutella", true, 10, "/img/nutella.jpg", "Cupcake de Nutella", 11.00m },
                    { 10, 1, "Cobertura de mousse de maracujá e calda azedinha", false, 10, "/img/maracuja.jpg", "Cupcake de Maracujá", 9.50m },
                    { 11, 4, "Cobertura cremosa com pedaços de biscoito Oreo", true, 10, "/img/oreo.jpg", "Cupcake de Oreo", 10.50m },
                    { 12, 4, "Recheado com Nutella e cobertura de Leite Ninho", true, 10, "/img/leiteninho.jpg", "Cupcake de Leite Ninho com Nutella", 11.90m },
                    { 13, 1, "Massa de café com cobertura de chantilly de cappuccino", false, 10, "/img/cafe.jpg", "Cupcake de Café", 9.80m },
                    { 14, 4, "Massa suave e cobertura cremosa de pistache", false, 10, "/img/pistache.jpg", "Cupcake de Pistache", 12.00m },
                    { 15, 3, "Sabor cítrico com toque de gengibre natural", false, 10, "/img/laranja.jpg", "Cupcake de Laranja com Gengibre", 9.00m },
                    { 16, 4, "Recheado com doce de leite e cobertura crocante de nozes", false, 10, "/img/nozes.jpg", "Cupcake de Doce de Leite com Nozes", 10.50m },
                    { 17, 1, "Sabor caseiro com toque de canela", false, 10, "/img/banana.jpg", "Cupcake de Banana com Canela", 8.50m },
                    { 18, 1, "Cobertura cremosa de chocolate branco e raspas finas", true, 10, "/img/chocolatebranco.jpg", "Cupcake de Chocolate Branco", 10.00m },
                    { 19, 1, "Sabor refrescante com pedaços de chocolate meio amargo", false, 10, "/img/menta.jpg", "Cupcake de Menta com Chocolate", 9.70m },
                    { 20, 4, "Equilíbrio perfeito entre doce e cítrico", false, 10, "/img/framboesa.jpg", "Cupcake de Framboesa com Limão Siciliano", 11.20m },
                    { 21, 1, "70% cacau com cobertura de ganache", false, 10, "/img/chocolateamargo.jpg", "Cupcake de Chocolate Amargo", 10.00m },
                    { 22, 1, "Tradicional massa de baunilha com cobertura leve", true, 10, "/img/baunilha.jpg", "Cupcake de Baunilha Clássico", 7.90m },
                    { 23, 1, "Cobertura de paçoca e farofa crocante", false, 10, "/img/pacoca.jpg", "Cupcake de Paçoca", 9.30m },
                    { 24, 2, "Cobertura cítrica e toque suave de coco", false, 10, "/img/limaococo.jpg", "Cupcake de Limão Siciliano com Coco", 9.80m },
                    { 25, 4, "Recheado com brigadeiro e cobertura granulada", true, 10, "/img/brigadeiro.jpg", "Cupcake de Brigadeiro Gourmet", 10.50m },
                    { 26, 4, "Inspirado em cheesecake com calda de morango", false, 10, "/img/cheesecake.jpg", "Cupcake de Cheesecake com Morango", 12.00m },
                    { 27, 3, "Sem leite e ovos, com manga e maracujá", false, 10, "/img/tropicais.jpg", "Cupcake Vegano de Frutas Tropicais", 10.50m },
                    { 28, 7, "Combinação ousada e levemente picante", false, 10, "/img/chocolatepimenta.jpg", "Cupcake de Chocolate com Pimenta", 9.90m },
                    { 29, 4, "Recheado com caramelo salgado e cobertura amanteigada", true, 10, "/img/caramelo.jpg", "Cupcake de Caramelo Salgado", 11.30m },
                    { 30, 3, "Sabor tropical e refrescante", false, 10, "/img/abacaxi.jpg", "Cupcake de Abacaxi com Hortelã", 9.60m }
                });

            migrationBuilder.InsertData(
                table: "Cupons",
                columns: new[] { "Id", "Ativo", "Codigo", "CriadoEm", "CupcakeId", "EhValorFixo", "ExpiraEm", "UsoAtual", "UsoMaximo", "Valor" },
                values: new object[,]
                {
                    { 1, true, "PROMO10", new DateTime(2025, 11, 9, 15, 42, 37, 144, DateTimeKind.Local).AddTicks(4101), null, false, new DateTime(2025, 12, 9, 15, 42, 37, 144, DateTimeKind.Local).AddTicks(4115), 0, 100, 10m },
                    { 2, true, "CUPCAKE5", new DateTime(2025, 11, 9, 15, 42, 37, 144, DateTimeKind.Local).AddTicks(4122), null, true, new DateTime(2026, 1, 9, 15, 42, 37, 144, DateTimeKind.Local).AddTicks(4123), 0, 0, 5m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_PedidoId",
                table: "Avaliacoes",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_CupcakeId",
                table: "Cupons",
                column: "CupcakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_ClienteId",
                table: "Enderecos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_CupcakeId",
                table: "PedidoItens",
                column: "CupcakeId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidoId",
                table: "PedidoItens",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EnderecoId",
                table: "Pedidos",
                column: "EnderecoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "Cupons");

            migrationBuilder.DropTable(
                name: "PedidoItens");

            migrationBuilder.DropTable(
                name: "Cupcakes");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
