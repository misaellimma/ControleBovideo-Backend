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
    public class ProdutorController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public ProdutorController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<ProdutorController>
        [HttpGet]
        [Authorize]
        public async Task<List<Produtor>> Get()
        {
            return await contexto.Produtores.ToListAsync();
        }

        // GET api/<ProdutorController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Produtor>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound("Identificador vazio!");
            }

            var produtor = await contexto.Produtores.FindAsync(id);

            if (produtor == null)
            {
                return NotFound("Não existe o produtor na base de dados!");
            }
            return produtor;
        }

        // GET api/<ProdutorController>/5
        [HttpGet("cpf={cpf}")]
        [Authorize]
        public async Task<ActionResult<Produtor>> GetCpf(string cpf)
        {
            if (cpf == null)
            {
                return NotFound("CPF vazio!");
            }
            Produtor produtor = new Produtor();
            
            if (!produtor.ValidaCpf(cpf))
            {
                return NotFound("CPF invalido!");
            }
            produtor = await contexto.Produtores.FirstOrDefaultAsync(e => e.Cpf == cpf);
            
            if (produtor == null)
            {
                return NotFound("Não existe o CPF na base de dados!");
            }
            return produtor;
        }

        // POST api/<ProdutorController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Produtor>> Post([FromBody] Produtor produtor)
        {
            if (produtor == null)
            {
                return NotFound();
            }
            produtor.Id = 0;

            if (!produtor.ValidaCpf(produtor.Cpf))
            {
                return NotFound("CPF invalido!");
            }

            await contexto.Produtores.AddAsync(produtor);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = produtor.Id });
        }

        // PUT api/<ProdutorController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Produtor produtor)
        {
            if (id != produtor.Id)
            {
                return BadRequest("Id da Url está divergente do body!");
            }

            contexto.Entry(produtor).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutorExists(produtor.Id))
                {
                    return NotFound("Produtor não existe!");
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { produtor });
        }

        private Boolean ProdutorExists(int id) => contexto.Produtores.Any(e => e.Id == id);
    }
}
