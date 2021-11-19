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
        public async Task<List<Propriedade>> Get()
        {
            return await contexto.Propriedades.OrderBy(e => e.Id_produtor).ToListAsync();
        }

        // GET api/<PropriedadeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Propriedade>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound("Identificador vazio!");
            }

            var propriedade = await contexto.Propriedades.FindAsync(id);

            if (propriedade == null)
            {
                return NotFound("Não existe a propriedade na base de dados!");
            }
            return propriedade;
        }

        [HttpGet("inscricao={inscricao}")]
        public async Task<ActionResult<Propriedade>> GetIncricao(string inscricao)
        {
            if (inscricao == null)
            {
                return NotFound("Inscrição Estadual vazia!");
            }

            Propriedade propriedade = new Propriedade();
            propriedade.Incricao_estadual =  inscricao;
            propriedade.FormataInscricao();

            if (!propriedade.ValidarInscricaoEstadual())
            {
                return NotFound("Incrição Estadual inválida!");
            }

            propriedade = await contexto.Propriedades.FirstAsync(e => e.Incricao_estadual == propriedade.Incricao_estadual);

            if (propriedade == null)
            {
                return NotFound("Não existe a propriedade na base de dados!");
            }
            return propriedade;
        }

        [HttpGet("validainscricao={inscricao}")]
        public async Task<ActionResult<Propriedade>> ValidaIncricao(string inscricao)
        {
            if (inscricao == null)
            {
                return NotFound("Inscrição Estadual vazia!");
            }

            Propriedade propriedade = new Propriedade();
            propriedade.Incricao_estadual = inscricao;
            propriedade.FormataInscricao();

            if (!propriedade.ValidarInscricaoEstadual())
            {
                return NotFound("Incrição Estadual inválida!");
            }

            propriedade = await contexto.Propriedades.FirstOrDefaultAsync(e => e.Incricao_estadual == propriedade.Incricao_estadual);

            if (propriedade != null)
            {
                return NotFound("Inscricao estadual já cadastrada!");
            }
            return NoContent();
        }

        [HttpGet("idprodutor={id}")]
        public async Task<ActionResult<dynamic>> GetPropriedadesProdutor(int? id)
        {
            if (id == null)
            {
                return NotFound("Identificador vazio!");
            }
            var propriedade = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();

            if (propriedade == null)
            {
                return NotFound("Não existe a propriedade na base de dados!");
            }
            return propriedade;
        }

        // POST api/<PropriedadeController>
        [HttpPost]
        public async Task<ActionResult<Propriedade>> Post([FromBody] Propriedade propriedade)
        {
            if (propriedade == null)
            {
                return NotFound();
            }

            propriedade.FormataInscricao();

            if (!propriedade.ValidarInscricaoEstadual())
            {
                return NotFound("Incrição Estadual inválida!");
            }

            await contexto.Propriedades.AddAsync(propriedade);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { propriedade });
        }

        // PUT api/<PropriedadeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Propriedade propriedade)
        {
            if (id != propriedade.Id)
            {
                return BadRequest("Id da Url está divergente do body!");
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
                    return NotFound("Não existe a propriedade na base de dados!");
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
