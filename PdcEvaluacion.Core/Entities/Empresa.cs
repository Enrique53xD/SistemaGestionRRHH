using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdcEvaluacion.Core.Entities
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nit { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string NombreComercial { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int MunicipioId { get; set; }
        public Municipio? Municipio { get; set; }

        public List<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; } = new();
    }
}
