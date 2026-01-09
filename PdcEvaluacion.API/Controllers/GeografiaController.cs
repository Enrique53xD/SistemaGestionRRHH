using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdcEvaluacion.Core.Entities;
using PdcEvaluacion.Infrastructure.Data;

namespace PdcEvaluacion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeografiaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GeografiaController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("Paises")]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaises()
        {
            return await _context.Paises.ToListAsync();
        }

        [HttpPost("Paises")]
        public async Task<ActionResult<Pais>> PostPais(Pais pais)
        {
            if (await _context.Paises.AnyAsync(p => p.Nombre == pais.Nombre))
                return BadRequest("¡Ese país ya existe!");

            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPaises", new { id = pais.Id }, pais);
        }

        [HttpPut("Paises/{id}")]
        public async Task<IActionResult> PutPais(int id, Pais pais)
        {
            if (id != pais.Id) return BadRequest();

            if (await _context.Paises.AnyAsync(p => p.Nombre == pais.Nombre && p.Id != id))
                return BadRequest("¡Ya existe otro país con ese nombre!");

            _context.Entry(pais).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Paises/{id}")]
        public async Task<IActionResult> DeletePais(int id)
        {
            bool hayEmpresas = await _context.Empresas
                .Include(e => e.Municipio).ThenInclude(m => m.Departamento)
                .AnyAsync(e => e.Municipio.Departamento.PaisId == id);

            if (hayEmpresas) return BadRequest("⚠️ No se puede eliminar: Hay empresas en este país.");

            var pais = await _context.Paises.FindAsync(id);
            if (pais == null) return NotFound();

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("Departamentos/{paisId}")]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamentos(int paisId)
        {
            return await _context.Departamentos.Where(d => d.PaisId == paisId).ToListAsync();
        }

        [HttpPost("Departamentos")]
        public async Task<ActionResult<Departamento>> PostDepartamento(Departamento depto)
        {
            if (await _context.Departamentos.AnyAsync(d => d.PaisId == depto.PaisId && d.Nombre == depto.Nombre))
                return BadRequest("Ese departamento ya existe en este país.");

            _context.Departamentos.Add(depto);
            await _context.SaveChangesAsync();
            return Ok(depto);
        }

        [HttpPut("Departamentos/{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamento depto)
        {
            if (id != depto.Id) return BadRequest();

            if (await _context.Departamentos.AnyAsync(d => d.PaisId == depto.PaisId && d.Nombre == depto.Nombre && d.Id != id))
                return BadRequest("¡Ya existe otro departamento con ese nombre aquí!");

            _context.Entry(depto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Departamentos/{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            bool hayEmpresas = await _context.Empresas.Include(e => e.Municipio).AnyAsync(e => e.Municipio.DepartamentoId == id);
            if (hayEmpresas) return BadRequest("⚠️ No se puede eliminar: Hay empresas en este departamento.");

            var depto = await _context.Departamentos.FindAsync(id);
            if (depto == null) return NotFound();

            _context.Departamentos.Remove(depto);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("Municipios/{departamentoId}")]
        public async Task<ActionResult<IEnumerable<Municipio>>> GetMunicipios(int departamentoId)
        {
            return await _context.Municipios.Where(m => m.DepartamentoId == departamentoId).ToListAsync();
        }

        [HttpPost("Municipios")]
        public async Task<ActionResult<Municipio>> PostMunicipio(Municipio mun)
        {
            if (await _context.Municipios.AnyAsync(m => m.DepartamentoId == mun.DepartamentoId && m.Nombre == mun.Nombre))
                return BadRequest("Ese municipio ya existe en este departamento.");

            _context.Municipios.Add(mun);
            await _context.SaveChangesAsync();
            return Ok(mun);
        }

        [HttpPut("Municipios/{id}")]
        public async Task<IActionResult> PutMunicipio(int id, Municipio mun)
        {
            if (id != mun.Id) return BadRequest();

            if (await _context.Municipios.AnyAsync(m => m.DepartamentoId == mun.DepartamentoId && m.Nombre == mun.Nombre && m.Id != id))
                return BadRequest("¡Ya existe otro municipio con ese nombre aquí!");

            _context.Entry(mun).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Municipios/{id}")]
        public async Task<IActionResult> DeleteMunicipio(int id)
        {
            bool hayEmpresas = await _context.Empresas.AnyAsync(e => e.MunicipioId == id);
            if (hayEmpresas) return BadRequest("⚠️ No se puede eliminar: Hay empresas en este municipio.");

            var mun = await _context.Municipios.FindAsync(id);
            if (mun == null) return NotFound();

            _context.Municipios.Remove(mun);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}