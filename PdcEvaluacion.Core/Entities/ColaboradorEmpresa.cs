using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdcEvaluacion.Core.Entities
{
    public class ColaboradorEmpresa
    {
        public int ColaboradorId { get; set; }
        public Colaborador? Colaborador { get; set; }

        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}
