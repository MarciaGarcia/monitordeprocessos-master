using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confere.Processos.Modelo
{
    //registra o interesse de uma pessoa em um processo
    public class Interesse
    {
        public int ProcessoId { get; set; }
        public int InteressadoId { get; set; }
        public Processo Processo { get; set; }
        public Interessado Interessado { get; set; }
    }
}
