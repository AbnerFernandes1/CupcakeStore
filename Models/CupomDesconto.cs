using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
    public class CupomDesconto
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string Codigo { get; set; }

        [Range(0.01, 9999)]
        public decimal Valor { get; set; }

        // true = desconto fixo, false = percentual
        public bool EhValorFixo { get; set; } = false;

        public DateTime CriadoEm { get; set; } = DateTime.Now;

        [Required]
        public DateTime ExpiraEm { get; set; }

        // opcional: cupom apenas para certo produto
        public int? CupcakeId { get; set; }
        public Cupcake Cupcake { get; set; }

        // opcional: uso controlado
        public bool Ativo { get; set; } = true;
        public int UsoMaximo { get; set; } = 0;
        public int UsoAtual { get; set; } = 0;

        public bool EstaValido()
        {
            return Ativo && DateTime.Now <= ExpiraEm && (UsoMaximo == 0 || UsoAtual < UsoMaximo);
        }

        public decimal CalcularDesconto(decimal total)
        {
            if (!EstaValido()) return 0;

            return EhValorFixo ? Valor : Math.Round(total * (Valor / 100), 2);
        }

        public void RegistrarUso()
        {
            UsoAtual++;
        }
    }
}
