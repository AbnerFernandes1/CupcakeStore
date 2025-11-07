using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
        public class Cupcake
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required, Range(0.1, 1000)]
        public decimal Preco { get; set; }

        [Required]
        public CategoriaCupcake Categoria { get; set; }

        public string ImagemUrl { get; set; }
        public bool Destaque { get; set; }
        public int Estoque { get; set; } = 10;
    }
}