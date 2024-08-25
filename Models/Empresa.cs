using System.ComponentModel.DataAnnotations;

namespace ControleNotasFiscais.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CNPJ")]
        public string? Cnpj { get; set; }

        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Display(Name = "Logo")]
        public string? Logo { get; set; }
    }
}
