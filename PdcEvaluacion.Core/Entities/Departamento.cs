using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdcEvaluacion.Core.Entities
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public int PaisId { get; set; }
        public Pais? Pais { get; set; }

        public List<Municipio> Municipios { get; set; } = new();
    }
}
