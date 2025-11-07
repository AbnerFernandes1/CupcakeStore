namespace CupcakeStore.Models
{
    public class CarrinhoItem
    {
        public int CupcakeId { get; set; }
        public Cupcake Cupcake { get; set; }
        public int Quantidade { get; set; }
    }
}