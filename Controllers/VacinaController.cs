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
    public class VacinaController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public VacinaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<VacinaController>
        [HttpGet]
        [Authorize]
        public async Task<List<Vacina>> Get()
        {
            return await contexto.Vacinas.ToListAsync();
        }

        // GET api/<VacinaController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Vacina>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacina = await contexto.Vacinas.FindAsync(id);

            if (vacina == null)
            {
                return NotFound();
            }
            return vacina;
        }

        // POST api/<VacinaController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Vacina>> Post([FromBody] Vacina vacina)
        {
            if (vacina == null)
            {
                return NotFound();
            }
            await contexto.Vacinas.AddAsync(vacina);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { vacina });
        }

        // PUT api/<VacinaController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Vacina vacina)
        {
            if (id != vacina.Id)
            {
                return BadRequest();
            }

            contexto.Entry(vacina).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VacinaExists(vacina.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { vacina });
        }

        private Boolean VacinaExists(int id) => contexto.Vacinas.Any(e => e.Id == id);
    }
}
