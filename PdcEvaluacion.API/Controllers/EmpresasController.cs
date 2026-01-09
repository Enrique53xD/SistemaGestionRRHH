using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdcEvaluacion.Core.Entities;
using PdcEvaluacion.Infrastructure.Data;

namespace PdcEvaluacion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            return await _context.Empresas
                .Include(e => e.Municipio)
                .ThenInclude(m => m.Departamento)
                .ThenInclude(d => d.Pais)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa(Empresa empresa)
        {
            if (await _context.Empresas.AnyAsync(e => e.Nit == empresa.Nit))
            {
                return BadRequest("¡El NIT ya está registrado en otra empresa!");
            }

            empresa.Municipio = null;

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEmpresas", new { id = empresa.Id }, empresa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, Empresa empresa)
        {
            if (id != empresa.Id) return BadRequest();

            if (await _context.Empresas.AnyAsync(e => e.Nit == empresa.Nit && e.Id != id))
            {
                return BadRequest("¡El NIT ya pertenece a otra empresa!");
            }

            empresa.Municipio = null;

            _context.Entry(empresa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null) return NotFound();

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}