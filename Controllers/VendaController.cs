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
        public async Task<List<Venda>> Get()
        {
            return await contexto.Vendas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetId(int? id)
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

            return venda;
        }

        // GET api/<VendaController>/5
        [HttpGet("venda={id}")]
        public async Task<ActionResult<dynamic>> GetVenda(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Venda> todasVendas = new List<Venda>();
            var obj = new
            {
                id = 0,
                vendedor_origem = "",
                propriedade_origem = "",
                vendedor_destino = "",
                propriedade_destino = "",
                finalidade_venda = "",
                especie = "",
                qtde_vendida = 0,
                data = ""
            };

            List<Object> objs = new List<Object>();
            

            //var teste = contexto.Vendas.OrderBy(e => e.Id).Select()
            
            var propriedades = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();
            foreach(var propriedade in propriedades)
            {
                var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == propriedade.Id).ToListAsync();
                foreach(var rebanho in rebanhos)
                {
                    var vendas = await contexto.Vendas.Where(e => e.Rebanho_origem == rebanho.Id).ToListAsync();
                    foreach(var venda in vendas)
                    {
                        var ob = new
                        {
                            id = venda.Id,
                            vendedor_origem = "",
                            propriedade_origem = "",
                            vendedor_destino = "",
                            propriedade_destino = "",
                            finalidade_venda = "",
                            especie = "",
                            qtde_vendida = 0,
                            data = ""
                        };
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
                    var vendas = await contexto.Vendas.Where(e => e.Rebanho_destino == rebanho.Id).ToListAsync();
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
        public async Task<ActionResult<Venda>> Post([FromBody] Venda venda)
        {
            if (venda == null)
            {
                return NotFound();
            }
            venda.Data = DateTime.Now;

            if(venda.Qtde_vendida < 1)
            {
                return NotFound("A quantidade vendida não deve ser menor que 1!");
            }

            Rebanho rebanhoOrigem = new Rebanho();
            rebanhoOrigem = await contexto.Rebanhos.FindAsync(venda.Rebanho_origem);

            if (!rebanhoOrigem.DebitarSaldoRebanhoVenda(venda.Qtde_vendida))
            {
                return NotFound("Saldo do rebanho vacinado de origem insuficiente!");
            }

            Rebanho rebanhoDestino = new Rebanho();
            rebanhoDestino = await contexto.Rebanhos.FindAsync(venda.Rebanho_destino);
            rebanhoDestino.CreditarSaldoRebanhoVenda(venda.Qtde_vendida);
            
            if(rebanhoOrigem.Id_especie != rebanhoDestino.Id_especie)
            {
                return NotFound("O rebanho deve ser da mesma espécie!");
            }
            var registroVacina = await contexto.RegistroVacinas.OrderBy(e => e.Id).LastOrDefaultAsync(e => e.Id_rebanho == rebanhoOrigem.Id);
            
            if(registroVacina == null)
            {
                return NotFound("Sem registro de vacinas do rebanho de origem! ");

            }
            if (venda.Data.Year != registroVacina.Data.Year)
            {
                return NotFound("Animais não foram vacinados no ano de " + venda.Data.Year);
            }

            contexto.Rebanhos.Update(rebanhoOrigem);
            contexto.Rebanhos.Update(rebanhoDestino);

            await contexto.Vendas.AddAsync(venda);
            await contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { venda });
        }

        // PUT api/<VendaController>/5
        [HttpPut("{id}")]
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
            return CreatedAtAction(nameof(Get), new { venda });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var venda = await contexto.Vendas.FindAsync(id);

            Rebanho rebanhoOrigem = new Rebanho();
            rebanhoOrigem = await contexto.Rebanhos.FindAsync(venda.Rebanho_origem);

            rebanhoOrigem.CreditarSaldoRebanhoVenda(venda.Qtde_vendida);

            Rebanho rebanhoDestino = new Rebanho();
            rebanhoDestino = await contexto.Rebanhos.FindAsync(venda.Rebanho_destino);
            rebanhoDestino.DebitarSaldoRebanhoVenda(venda.Qtde_vendida);

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
