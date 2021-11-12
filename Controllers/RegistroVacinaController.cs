using ControleBovideo.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroVacinaController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public RegistroVacinaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<Registro_vacinaController>
        [HttpGet]
        [Authorize]
        public async Task<List<RegistroVacina>> Get()
        {
            return await contexto.RegistroVacinas.ToListAsync();
        }

        // GET api/<Registro_vacinaController>/5
        [HttpGet("propriedade={id}")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Get(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == id).ToListAsync();
            
            if (rebanhos == null)
            {
                return NotFound();
            }
            List<RegistroVacina> registroVacinas = new List<RegistroVacina>();
            
            foreach(var rebanho in rebanhos)
            {
                var registros = await contexto.RegistroVacinas.Where(e => e.Id_rebanho == rebanho.Id).ToListAsync();
                foreach(var rv in registros)
                {
                    registroVacinas.Add(rv);
                }
            }

            return registroVacinas;
        }

        // POST api/<Registro_vacinaController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RegistroVacina>> Post([FromBody] RegistroVacina registroVacina)
        {
            if (registroVacina == null)
            {
                return NotFound();
            }
            await contexto.RegistroVacinas.AddAsync(registroVacina);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { registroVacina });
        }

        // PUT api/<Registro_vacinaController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] RegistroVacina registroVacina)
        {
            if (id != registroVacina.Id)
            {
                return BadRequest();
            }

            contexto.Entry(registroVacina).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroVacinaExists(registroVacina.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { registroVacina });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var registroVacina = await contexto.RegistroVacinas.FindAsync(id);
            if (registroVacina == null)
            {
                return NotFound();
            }

            contexto.RegistroVacinas.Remove(registroVacina);
            await contexto.SaveChangesAsync();

            return NoContent();
        }

        private Boolean RegistroVacinaExists(int id) => contexto.RegistroVacinas.Any(e => e.Id == id);
    }
}
