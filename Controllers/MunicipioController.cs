using ControleBovideo.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControleBovideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public MunicipioController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<MunicipioController>
        [HttpGet]
        [Authorize]
        public async Task<List<Municipio>> Get()
        {
            return await contexto.Municipios.ToListAsync();
        }

        // GET api/<MunicipioController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Municipio>> Get(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var municipio = await contexto.Municipios.FindAsync(id);

            if (municipio == null)
            {
                return NotFound();
            }
            return municipio;
        }

        // POST api/<MunicipioController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Municipio>> Post([FromBody] Municipio municipio)
        {
            if (municipio == null)
            {
                return NotFound();
            }
            await contexto.Municipios.AddAsync(municipio);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { municipio });
        }

        // PUT api/<MunicipioController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Municipio municipio)
        {
            if (id != municipio.Id)
            {
                return BadRequest();
            }

            contexto.Entry(municipio).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MunicipioExists(municipio.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { municipio });
        }

        private Boolean MunicipioExists(int id) => contexto.Municipios.Any(e => e.Id == id);
    }
}
