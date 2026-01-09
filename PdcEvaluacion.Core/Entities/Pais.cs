using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdcEvaluacion.Core.Entities
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public List<Departamento> Departamentos { get; set; } = new();
    }
}
