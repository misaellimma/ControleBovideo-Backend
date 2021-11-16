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
    public class RebanhoController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public RebanhoController(Contexto contexto)
        {
            this.contexto = contexto;
        }
        [HttpGet]
        [Authorize]
        public async Task<List<Rebanho>> Get()
        {
            return await contexto.Rebanhos.ToListAsync();
        }

        public Rebanho Get(int? id)
        {
            var rebanho = contexto.Rebanhos.Find(id);   
            return rebanho;
        }

        [HttpGet("produtor={id}")]
        [Authorize]
        public async Task<List<Rebanho>> GetAnimalProdutor(int? id)
        {
            var propriedades = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();
            List<Rebanho> rebanho = new List<Rebanho>();
            
            foreach (var propriedade in propriedades)
            {
                var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == propriedade.Id).ToListAsync();
                foreach(var r in rebanhos)
                {
                    rebanho.Add(r);
                }
            }
            return rebanho;
        }

        [HttpGet("propriedade={id}")]
        [Authorize]
        public async Task<List<Rebanho>> GetAnimalPropriedade(int? id) 
        { 
            var rebanho = await contexto.Rebanhos.Where(e => e.Id_propriedade == id).ToListAsync();
            
            return rebanho;
        }

        // POST api/<RebanhoController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Rebanho>> Post([FromBody] Rebanho rebanho)
        {
            if (rebanho == null)
            {
                return NotFound();
            }
            await contexto.Rebanhos.AddAsync(rebanho);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { rebanho });
        }

        // PUT api/<RebanhoController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Rebanho rebanho)
        {
            if (id != rebanho.Id)
            {
                return BadRequest();
            }

            contexto.Entry(rebanho).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RebanhoExists(rebanho.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { rebanho });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rebanho = await contexto.Rebanhos.FindAsync(id);
            if (rebanho == null)
            {
                return NotFound();
            }

            contexto.Rebanhos.Remove(rebanho);
            await contexto.SaveChangesAsync();

            return NoContent();
        }

        private Boolean RebanhoExists(int id) => contexto.Rebanhos.Any(e => e.Id == id);
    }
}
