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
    public class FinalidadeVendaController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public FinalidadeVendaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<FinalidadeVendaController>
        [HttpGet]
        [Authorize]
        public async Task<List<FinalidadeVenda>> Get()
        {
            return await contexto.FinalidadeVendas.ToListAsync();
        }

        // GET api/<FinalidadeVendaController>/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FinalidadeVenda>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var finalidadeVenda = await contexto.FinalidadeVendas.FindAsync(id);

            if (finalidadeVenda == null)
            {
                return NotFound();
            }
            return finalidadeVenda;
        }

        // POST api/<FinalidadeVendaController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FinalidadeVenda>> Post([FromBody] FinalidadeVenda finalidadeVenda)
        {
            if (finalidadeVenda == null)
            {
                return NotFound();
            }
            await contexto.FinalidadeVendas.AddAsync(finalidadeVenda);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = finalidadeVenda.Id });
        }

        // PUT api/<FinalidadeVendaController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] FinalidadeVenda finalidadeVenda)
        {
            if (id != finalidadeVenda.Id)
            {
                return BadRequest();
            }

            contexto.Entry(finalidadeVenda).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinalidadeVendaExists(finalidadeVenda.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { id = finalidadeVenda.Id, finalidadeVenda });
        }

        private Boolean FinalidadeVendaExists(int id) => contexto.FinalidadeVendas.Any(e => e.Id == id);
    }
}
