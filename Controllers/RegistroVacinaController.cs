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
    public class RegistroVacinaController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public RegistroVacinaController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<Registro_vacinaController>
        [HttpGet]
        [Authorize]
        public async Task<List<RegistroVacina>> Get()
        {
            return await contexto.RegistroVacinas.ToListAsync();
        }

        // GET api/<Registro_vacinaController>/5
        [HttpGet("propriedade={id}")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Get(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var rebanhos = await contexto.Rebanhos.Where(e => e.Id_propriedade == id).ToListAsync();
            
            if (rebanhos == null)
            {
                return NotFound();
            }
            List<RegistroVacina> registroVacinas = new List<RegistroVacina>();
            
            foreach(var rebanho in rebanhos)
            {
                var registros = await contexto.RegistroVacinas.Where(e => e.Id_rebanho == rebanho.Id).ToListAsync();
                foreach(var rv in registros)
                {
                    registroVacinas.Add(rv);
                }
            }

            return registroVacinas;
        }

        // POST api/<Registro_vacinaController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RegistroVacina>> Post([FromBody] RegistroVacina registroVacina)
        {
            if (registroVacina == null)
            {
                return NotFound();
            }

            registroVacina.Data = DateTime.Now;//armazena o horario do registro

            //busca o ultimo registro de vacina do rebanho
            var rv = await contexto.RegistroVacinas.OrderBy(e => e.Id).LastOrDefaultAsync(e => e.Id_rebanho == registroVacina.Id_rebanho);
            
            Rebanho rebanho = new Rebanho();
            //busca o rebanho
            rebanho = await contexto.Rebanhos.FindAsync(registroVacina.Id_rebanho);

            if (rv.ValidarDataVacina(rv.Data))
            {
                await contexto.RegistroVacinas.AddAsync(registroVacina);
                rebanho.CreditarSaldoVacinado(registroVacina.Qtde_vacinado);
                contexto.Rebanhos.Update(rebanho);

            }
            else if (!rv.ValidarDataVacina(rv.Data) && rebanho.ValidarQtdeVacinado())
            {
                await contexto.RegistroVacinas.AddAsync(registroVacina);
                rebanho.CreditarSaldoVacinado(registroVacina.Qtde_vacinado);
                contexto.Rebanhos.Update(rebanho);
            }
            else
            {
                return NotFound("Registro não foi criado!");
            }      

            await contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { registroVacina });
        }

        // PUT api/<Registro_vacinaController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] RegistroVacina registroVacina)
        {
            if (id != registroVacina.Id)
            {
                return BadRequest();
            }

            contexto.Entry(registroVacina).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroVacinaExists(registroVacina.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { registroVacina });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Identificador vazio!");
            }
            var registroVacina = await contexto.RegistroVacinas.FindAsync(id);
            
            if (registroVacina == null)
            {
                return NotFound("Não possui registro na base de dados!");
            }
            var rebanho = await contexto.Rebanhos.FindAsync(registroVacina.Id_rebanho);
            var venda = await contexto.Vendas.OrderBy(e => e.Id)
                        .Where(e => e.Propriedade_origem == rebanho.Id).LastAsync();
            
            if (registroVacina.CalculoData(venda.Data))
            {
                contexto.RegistroVacinas.Remove(registroVacina);
                await contexto.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound("Não pode ser deletado, pois já foi vendido!");
            }

        }

        private Boolean RegistroVacinaExists(int id) => contexto.RegistroVacinas.Any(e => e.Id == id);
    }
}
