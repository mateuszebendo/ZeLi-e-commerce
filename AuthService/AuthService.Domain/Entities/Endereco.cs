namespace AuthService.Domain.Entities
{
    public class Endereco
    {
        public int EnderecoId { get; private set; }
        public string Logradouro {  get; private set; }
        public int Numero { get; private set; }
        public string Complemento { get; private set; }

        public Endereco(int enderecoId, string logradouro, int numero, string complemento)
        {
            EnderecoId = enderecoId;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
        }

        public Endereco()
        {
        }
    }
}