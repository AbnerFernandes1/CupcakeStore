using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Nota { get; set; }

        [StringLength(500)]
        public string Comentario { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        // Relacionamentos
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}