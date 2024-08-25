namespace ControleNotasFiscais.Models
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public decimal Valor { get; set; }
        public string TipoCompra { get; set; }
        public DateTime DataCompra { get; set; }
        public string Usuario { get; set; }
        public string? CaminhoImagem { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
