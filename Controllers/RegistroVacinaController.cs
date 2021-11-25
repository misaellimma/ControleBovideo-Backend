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
        public async Task<List<RegistroVacina>> Get()
        {
            return await contexto.RegistroVacinas.ToListAsync();
        }

        // GET api/<Registro_vacinaController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var registroVacina = await contexto.RegistroVacinas.FindAsync(id);
            if (registroVacina == null)
            {
                return NotFound();
            }
            var rebanho = await contexto.Rebanhos.FindAsync(registroVacina.Id_rebanho);
            var propriedade = await contexto.Propriedades.FindAsync(rebanho.Id_propriedade);
            var especie = await contexto.EspecieBovideos.FindAsync(rebanho.Id_especie);
            var vacina = await contexto.Vacinas.FindAsync(registroVacina.Id_vacina);
            var obj = new
            {
                registroVacina.Id,
                rebanho = especie.Descricao,
                propriedade.Nome_propriedade,
                registroVacina.Id_rebanho,
                registroVacina.Id_vacina,
                vacina = vacina.Nome,
                registroVacina.Qtde_vacinado,
                data = registroVacina.Data.ToString("dd/MM/yyyy")
            };


            return obj;
        }

        // GET api/<Registro_vacinaController>/5
        [HttpGet("propriedade={id}")]
        public async Task<ActionResult<dynamic>> Get(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var rebanhos = await contexto.Rebanhos.OrderBy(e => e.Id).Where(e => e.Id_propriedade == id).ToListAsync();
            
            if (rebanhos == null)
            {
                return NotFound();
            }
            List<object> registroVacinas = new List<object>();
            
            foreach(var rebanho in rebanhos)
            {
                var registros = await contexto.RegistroVacinas.Where(e => e.Id_rebanho == rebanho.Id).ToListAsync();
                var especie = await contexto.EspecieBovideos.FindAsync(rebanho.Id_especie);
                foreach (var rv in registros)
                {
                    var vacina = await contexto.Vacinas.FindAsync(rv.Id_vacina);
                    var obj = new
                    {
                        rv.Id,
                        rebanho = especie.Descricao,
                        rv.Id_rebanho,
                        rv.Id_vacina,
                        vacina = vacina.Nome,
                        rv.Qtde_vacinado,
                        data = rv.Data.ToString("dd/MM/yyyy")
                    };
                    registroVacinas.Add(obj);
                }
            }

            return registroVacinas;
        }

        // POST api/<Registro_vacinaController>
        [HttpPost]
        public async Task<ActionResult<RegistroVacina>> Post([FromBody] RegistroVacina registroVacina)
        {
            if (registroVacina == null)
            {
                return NotFound();
            }

            //busca o ultimo registro de vacina do rebanho
            var rv = await contexto.RegistroVacinas.OrderBy(e => e.Id).LastOrDefaultAsync(e => e.Id_rebanho == registroVacina.Id_rebanho);
            
            Rebanho rebanho = new Rebanho();
            //busca o rebanho
            rebanho = await contexto.Rebanhos.FindAsync(registroVacina.Id_rebanho);

            if(registroVacina.Id_vacina == 1)
            {
                rebanho.CreditarSaldoVacinadoAftosa(registroVacina.Qtde_vacinado);
            }
            else if(registroVacina.Id_vacina == 2)
            {
                rebanho.CreditarSaldoVacinadoBrucelose(registroVacina.Qtde_vacinado);
            }

            if (!rebanho.ValidarQtdeVacinado())
            {
                return NotFound("Registro não foi criado, o número de vacinados não pode ser maior que o total do rebanho!");
            }
            else if(rv == null)
            {
                await contexto.RegistroVacinas.AddAsync(registroVacina);
                contexto.Rebanhos.Update(rebanho);
            }
            else if (registroVacina.ValidarDataVacina(rv.Data) && rebanho.ValidarQtdeVacinado())
            {
                await contexto.RegistroVacinas.AddAsync(registroVacina);
                contexto.Rebanhos.Update(rebanho);
            }
            else if (!registroVacina.ValidarDataVacina(rv.Data) && rebanho.ValidarQtdeVacinado())
            {
                await contexto.RegistroVacinas.AddAsync(registroVacina);
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
                        .Where(e => e.Rebanho_origem == rebanho.Id).LastOrDefaultAsync();
            
            if (registroVacina.Id_vacina == 1)
            {
                rebanho.DebitarSaldoVacinadoAftosa(registroVacina.Qtde_vacinado);
            }
            else if (registroVacina.Id_vacina == 2)
            {
                rebanho.DebitarSaldoVacinadoBrucelose(registroVacina.Qtde_vacinado);
            }

            if (venda == null)
            {
                contexto.RegistroVacinas.Remove(registroVacina);
                await contexto.SaveChangesAsync();
                return NoContent();
            }
            else if(registroVacina.CalculoData(venda.Data))
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
