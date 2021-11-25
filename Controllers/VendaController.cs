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
        public async Task<ActionResult<dynamic>> GetId(int? id)
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

            var rebanhoOrigem = contexto.Rebanhos.Where(e => e.Id == venda.Rebanho_origem).FirstOrDefault();
            var propriedade = contexto.Propriedades.Where(e => e.Id == rebanhoOrigem.Id_propriedade).FirstOrDefault();
            var rebanhoDestino = contexto.Rebanhos.Where(e => e.Id == venda.Rebanho_destino).FirstOrDefault();
            var prop = contexto.Propriedades.Where(e => e.Id == rebanhoDestino.Id_propriedade).FirstOrDefault();
            var especie = contexto.EspecieBovideos.Where(e => e.Id == rebanhoOrigem.Id_especie).FirstOrDefault();
            var finalidade = contexto.FinalidadeVendas.Where(e => e.Id == venda.Id_finalidade_venda).FirstOrDefault();
            var obj = new
            {
                id = venda.Id,
                id_propriedade_origem = propriedade.Id,
                propriedade_origem = propriedade.Nome_propriedade,
                id_propriedade_destino = prop.Id,
                propriedade_destino = prop.Nome_propriedade,
                venda.Rebanho_origem,
                venda.Rebanho_destino,
                especie = especie.Descricao,
                venda.Id_finalidade_venda,
                finalidade_venda = finalidade.Descricao,
                venda.Qtde_vendida,
                data = venda.Data.ToString("dd/MM/yyyy"),
            };

            return obj;
        }

        // GET api/<VendaController>/5
        [HttpGet("venda={id}")]
        public async Task<ActionResult<dynamic>> GetVenda(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<object> todasVendas = new List<object>();

            //var teste = contexto.Vendas.OrderBy(e => e.Id).Where(e => e.Rebanho_destino == )
            
            var propriedades = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();
            foreach(var propriedade in propriedades)
            {
                var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == propriedade.Id).ToListAsync();
                foreach(var rebanho in rebanhos)
                {
                    var vendas = await contexto.Vendas.Where(e => e.Rebanho_origem == rebanho.Id).ToListAsync();
                    foreach(var venda in vendas)
                    {
                        var finalidade = contexto.FinalidadeVendas.Where(e => e.Id == venda.Id_finalidade_venda).FirstOrDefault();
                        var prop = contexto.Propriedades.Where(e => e.Id == rebanho.Id_propriedade).FirstOrDefault();
                        var especie = contexto.EspecieBovideos.Where(e => e.Id == rebanho.Id_especie).FirstOrDefault();
                        var obj = new
                        {
                            id = venda.Id,
                            id_propriedade_origem = propriedade.Id,
                            propriedade_origem = propriedade.Nome_propriedade,
                            id_propriedade_destino = prop.Id,
                            propriedade_destino = prop.Nome_propriedade,
                            venda.Rebanho_origem,
                            venda.Rebanho_destino,
                            especie = especie.Descricao,
                            venda.Id_finalidade_venda,
                            finalidade_venda = finalidade.Descricao,
                            venda.Qtde_vendida,
                            data = venda.Data.ToString("dd/MM/yyyy"),
                        };
                        todasVendas.Add(obj);
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
        public async Task<ActionResult<object>> GetCompra(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<object> todasVendas = new List<object>();

            var propriedades = await contexto.Propriedades.Where(e => e.Id_produtor == id).ToListAsync();
            foreach (var propriedade in propriedades)
            {
                var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == propriedade.Id).ToListAsync();
                foreach (var rebanho in rebanhos)
                {
                    var vendas = await contexto.Vendas.Where(e => e.Rebanho_destino == rebanho.Id).ToListAsync();
                    foreach (var venda in vendas)
                    {
                        var finalidade = contexto.FinalidadeVendas.Where(e => e.Id == venda.Id_finalidade_venda).FirstOrDefault();
                        var prop = contexto.Propriedades.Where(e => e.Id == rebanho.Id_propriedade).FirstOrDefault();
                        var especie = contexto.EspecieBovideos.Where(e => e.Id == rebanho.Id_especie).FirstOrDefault();
                        var obj = new
                        {
                            id = venda.Id,
                            id_propriedade_origem = prop.Id,
                            propriedade_origem = prop.Nome_propriedade,
                            id_propriedade_destino = propriedade.Id,
                            propriedade_destino = propriedade.Nome_propriedade,
                            venda.Rebanho_origem,
                            venda.Rebanho_destino,
                            especie = especie.Descricao,
                            venda.Id_finalidade_venda,
                            finalidade_venda = finalidade.Descricao,
                            venda.Qtde_vendida,
                            data = venda.Data.ToString("dd/MM/yyyy"),
                        };
                        todasVendas.Add(obj);
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
