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
    public class VendaController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public VendaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<VendaController>
        [HttpGet]
        [Authorize]
        public async Task<List<Venda>> Get()
        {
            return await contexto.Vendas.ToListAsync();
        }

        // GET api/<VendaController>/5
        [HttpGet("venda={id}")]
        [Authorize]
        public async Task<ActionResult<dynamic>> GetVenda(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Venda> todasVendas = new List<Venda>();

            var propriedades = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();
            foreach(var propriedade in propriedades)
            {
                var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == propriedade.Id).ToListAsync();
                foreach(var rebanho in rebanhos)
                {
                    var vendas = await contexto.Vendas.Where(e => e.Propriedade_origem == rebanho.Id_propriedade).ToListAsync();
                    foreach(var venda in vendas)
                    {
                        todasVendas.Add(venda);
                    }
                }
            }
            if (todasVendas == null)
            {
                return NotFound();
            }
            return todasVendas;
        }

        // GET api/<VendaController>/5
        [HttpGet("compra={id}")]
        [Authorize]
        public async Task<ActionResult<dynamic>> GetCompra(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Venda> todasVendas = new List<Venda>();

            var propriedades = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();
            foreach (var propriedade in propriedades)
            {
                var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == propriedade.Id).ToListAsync();
                foreach (var rebanho in rebanhos)
                {
                    var vendas = await contexto.Vendas.Where(e => e.Propriedade_destino == rebanho.Id_propriedade).ToListAsync();
                    foreach (var venda in vendas)
                    {
                        todasVendas.Add(venda);
                    }
                }
            }
            if (todasVendas == null)
            {
                return NotFound();
            }
            return todasVendas;
        }

        // POST api/<VendaController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Venda>> Post([FromBody] Venda venda)
        {
            if (venda == null)
            {
                return NotFound();
            }
            await contexto.Vendas.AddAsync(venda);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = venda.Id });
        }

        // PUT api/<VendaController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Venda venda)
        {
            if (id != venda.Id)
            {
                return BadRequest();
            }

            contexto.Entry(venda).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(venda.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { id = venda.Id, venda });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var venda = await contexto.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            contexto.Vendas.Remove(venda);
            await contexto.SaveChangesAsync();

            return NoContent();
        }

        private Boolean VendaExists(int id) => contexto.Vendas.Any(e => e.Id == id);
    }
}
