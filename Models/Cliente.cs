using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Telefone { get; set; }

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}