using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdcEvaluacion.Core.Entities;
using PdcEvaluacion.Infrastructure.Data;

namespace PdcEvaluacion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColaboradoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colaborador>>> GetColaboradores()
        {
            var colaboradores = await _context.Colaboradores
                .Include(c => c.ColaboradoresEmpresas)
                .ThenInclude(ce => ce.Empresa)
                .ToListAsync();

            foreach (var colab in colaboradores)
            {
                if (colab.ColaboradoresEmpresas != null)
                {
                    foreach (var ce in colab.ColaboradoresEmpresas)
                    {
                        if (ce.Empresa != null)
                        {
                            ce.Empresa.ColaboradoresEmpresas = null;
                        }
                    }
                }
            }

            return colaboradores;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Colaborador>> GetColaborador(int id)
        {
            var colaborador = await _context.Colaboradores
                .Include(c => c.ColaboradoresEmpresas)
                .ThenInclude(ce => ce.Empresa)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (colaborador == null)
            {
                return NotFound();
            }

            if (colaborador.ColaboradoresEmpresas != null)
            {
                foreach (var ce in colaborador.ColaboradoresEmpresas)
                {
                    if (ce.Empresa != null)
                    {
                        ce.Empresa.ColaboradoresEmpresas = null;
                    }
                }
            }

            return colaborador;
        }

        [HttpPost]
        public async Task<ActionResult<Colaborador>> PostColaborador(ColaboradorDTO dto)
        {
            // --- VALIDACIÓN: EMAIL ÚNICO ---
            if (await _context.Colaboradores.AnyAsync(c => c.Email == dto.Email))
            {
                return BadRequest("¡Ese correo electrónico ya está registrado!");
            }

            var colaborador = new Colaborador
            {
                NombreCompleto = dto.NombreCompleto,
                Edad = dto.Edad,
                Telefono = dto.Telefono,
                Email = dto.Email
            };

            if (dto.EmpresaIds != null && dto.EmpresaIds.Any())
            {
                foreach (var idEmpresa in dto.EmpresaIds)
                {
                    colaborador.ColaboradoresEmpresas.Add(new ColaboradorEmpresa { EmpresaId = idEmpresa });
                }
            }

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            foreach (var ce in colaborador.ColaboradoresEmpresas)
            {
                if (ce.Empresa != null) ce.Empresa.ColaboradoresEmpresas = null;
            }

            return CreatedAtAction("GetColaborador", new { id = colaborador.Id }, colaborador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaborador(int id, ColaboradorDTO dto)
        {
            if (await _context.Colaboradores.AnyAsync(c => c.Email == dto.Email && c.Id != id))
            {
                return BadRequest("¡Ese correo ya pertenece a otro colaborador!");
            }

            var colaboradorExistente = await _context.Colaboradores
                .Include(c => c.ColaboradoresEmpresas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (colaboradorExistente == null)
            {
                return NotFound();
            }

            colaboradorExistente.NombreCompleto = dto.NombreCompleto;
            colaboradorExistente.Edad = dto.Edad;
            colaboradorExistente.Telefono = dto.Telefono;
            colaboradorExistente.Email = dto.Email;

            colaboradorExistente.ColaboradoresEmpresas.Clear();

            if (dto.EmpresaIds != null)
            {
                foreach (var idEmpresa in dto.EmpresaIds)
                {
                    colaboradorExistente.ColaboradoresEmpresas.Add(new ColaboradorEmpresa
                    {
                        ColaboradorId = id,
                        EmpresaId = idEmpresa
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColaboradorExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColaboradorExists(int id)
        {
            return _context.Colaboradores.Any(e => e.Id == id);
        }
    }

    public class ColaboradorDTO
    {
        public string NombreCompleto { get; set; }
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public List<int> EmpresaIds { get; set; } = new();
    }
}