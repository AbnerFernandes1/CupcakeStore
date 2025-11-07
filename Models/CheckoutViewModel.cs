public class CheckoutViewModel
{
    // Dados do cliente
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }

    // Endereço
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string UF { get; set; }
    public string CEP { get; set; }
    public string Complemento { get; set; }

    public string FormaPagamento { get; set; }
}
