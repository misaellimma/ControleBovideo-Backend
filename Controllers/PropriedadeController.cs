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
    public class PropriedadeController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public PropriedadeController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<PropriedadeController>
        [HttpGet]
        [Authorize]
        public async Task<List<Propriedade>> Get()
        {
            return await contexto.Propriedades.ToListAsync();
        }

        // GET api/<PropriedadeController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Propriedade>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propriedade = await contexto.Propriedades.FindAsync(id);

            if (propriedade == null)
            {
                return NotFound();
            }
            return propriedade;
        }

        [HttpGet("inscricao={inscricao}")]
        [Authorize]
        public async Task<ActionResult<Propriedade>> GetIncricao(string inscricao)
        {
            if (inscricao == null)
            {
                return NotFound();
            }

            var propriedade = await contexto.Propriedades.FirstOrDefaultAsync(e => e.Incricao_estadual == inscricao);

            if (propriedade == null)
            {
                return NotFound();
            }
            return propriedade;
        }

        [HttpGet("idprodutor={id}")]
        [Authorize]
        public async Task<List<Propriedade>> GetPropriedadesProdutor(int id)
        {
            var propriedade = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();

            return propriedade;
        }

        // POST api/<PropriedadeController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Propriedade>> Post([FromBody] Propriedade propriedade)
        {
            if (propriedade == null)
            {
                return NotFound();
            }
            await contexto.Propriedades.AddAsync(propriedade);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = propriedade.Id });
        }

        // PUT api/<PropriedadeController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Propriedade propriedade)
        {
            if (id != propriedade.Id)
            {
                return BadRequest();
            }

            contexto.Entry(propriedade).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropriedadeExists(propriedade.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { propriedade });
        }

        private Boolean PropriedadeExists(int id) => contexto.Propriedades.Any(e => e.Id == id);
    }
}
