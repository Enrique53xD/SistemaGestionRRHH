using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdcEvaluacion.Core.Entities
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; } = new();
    }
}
