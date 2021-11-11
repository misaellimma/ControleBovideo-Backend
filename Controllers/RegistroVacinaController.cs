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
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RegistroVacina>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await contexto.RegistroVacinas.FindAsync(id);

            if (municipio == null)
            {
                return NotFound();
            }
            return municipio;
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
            return CreatedAtAction(nameof(Get), new { id = registroVacina.Id });
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
            return CreatedAtAction(nameof(Get), new { id = registroVacina.Id, registroVacina });
        }

        private Boolean RegistroVacinaExists(int id) => contexto.RegistroVacinas.Any(e => e.Id == id);
    }
}
