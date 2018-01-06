using System;
using System.Collections.Generic;
using System.Linq;

namespace Confere.Processos.Modelo
{
    public class Interessado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public IList<Interesse> Interesses { get; set; }

        public Interessado()
        {
            Interesses = new List<Interesse>();
        }

        public void RegistraInteresse(params Processo[] processos)
        {
            //IEnumerable<Processo> lista = processos;
            RegistraInteresse(processos.ToList());
        }

        public void RegistraInteresse(IEnumerable<Processo> processos)
        {
            foreach (var processo in processos)
            {
                var interesse = new Interesse { Processo = processo, Interessado = this };
                this.Interesses.Add(interesse);
                processo.Interesses.Add(interesse);
            }
        }
    }
}
