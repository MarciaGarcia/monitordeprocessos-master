using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Confere.Processos.Modelo
{
    public enum OrigemProcesso
    {
        Senado, Camara
    } 

    public class Processo
    {
        public Processo()
        {
            Interesses = new List<Interesse>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }

        [Required]
        [Range(minimum: 1, maximum: 99999)]
        [Display(Name = "Número")]
        public int Numero { get; set; }

        [Required]
        [Range(minimum: 1900, maximum: 2100)]
        [Display(Name = "Ano")]
        public int Ano { get; set; }

        public string Emenda { get; set; }

        public DateTime? DataUltimaAtualizacao { get; set; }

        public int Codigo { get; set; }

        public OrigemProcesso Origem { get; set; }

        public IList<Interesse> Interesses { get; set; }

        public override string ToString()
        {
            return $"{Sigla}/{Numero}/{Ano}";
        }
    }
}
