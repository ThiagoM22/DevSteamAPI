namespace DevSteamAPI.Model
{
    public class Jogo
    {
        public Guid JogoId { get; set; }
        public string Nome { get; set; }
        public int Classificacao { get; set; }
        //? serve para permitir valores nulos
        public string? Descricao { get; set; }
        public string? Imagem { get; set; }

        public Guid CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
