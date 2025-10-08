namespace Midas.Infrastructure.Persistence.Entities
{
    public class Cofrinho
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal Meta { get; set; }
        public decimal Atingido { get; set; }
        public bool Aplicado { get; set; }
    }
}
