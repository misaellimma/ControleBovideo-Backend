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
    public class EnderecoController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public EnderecoController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<EnderecoController>
        [HttpGet]
        [Authorize]
        public async Task<List<Endereco>> Get()
        {
            return await contexto.Enderecos.ToListAsync();
        }

        // GET api/<EnderecoController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Endereco>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound("Identificador vazio!");
            }

            var endereco = await contexto.Enderecos.FindAsync(id);

            if (endereco == null)
            {
                return NotFound();
            }
            return endereco;
        }

        // POST api/<EnderecoController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Endereco>> Post([FromBody] Endereco endereco)
        {
            if (endereco == null)
            {
                return NotFound();
            }
            await contexto.Enderecos.AddAsync(endereco);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { endereco });
        }

        // PUT api/<EnderecoController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return BadRequest();
            }

            contexto.Entry(endereco).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecoExists(endereco.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { endereco });
        }

        private Boolean EnderecoExists(int id) => contexto.Enderecos.Any(e => e.Id == id);
    }
}
