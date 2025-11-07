using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

        [Required]
        public string FormaPagamento { get; set; }

        public string Status { get; set; } = "Em preparação";

        // Relacionamentos
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        public List<PedidoItem> Itens { get; set; } = new();
        public Avaliacao Avaliacao { get; set; }
    }
}