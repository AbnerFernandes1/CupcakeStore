using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        [Required]
        public string Rua { get; set; }

        [Required]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        [Required]
        public string Bairro { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required, StringLength(2)]
        public string UF { get; set; }

        [Required, StringLength(10)]
        public string CEP { get; set; }

        // Relacionamento
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}