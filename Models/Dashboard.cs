namespace ControleNotasFiscais.Models
{
    public class DashboardViewModel
    {
        public int TotalNotasFiscais { get; set; }
        public decimal TotalValorNotasFiscais { get; set; }
        public IEnumerable<NotaFiscalPorTipoCompra> NotasPorTipoCompra { get; set; }
        public IEnumerable<Empresa> Empresas { get; set; }
    }

    public class NotaFiscalPorTipoCompra
    {
        public string TipoCompra { get; set; }
        public int Count { get; set; }
    }
}
