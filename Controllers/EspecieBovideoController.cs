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
    public class EspecieBovideoController : ControllerBase
    {
        private Contexto contexto { get; set; }

        public EspecieBovideoController(Contexto contexto)
        {
            this.contexto = contexto;
        }

        // GET: api/<EspecieBovideoController>
        [HttpGet]
        public async Task<List<EspecieBovideo>> Get()
        {
            return await contexto.EspecieBovideos.ToListAsync();
        }

        // GET api/<EspecieBovideoController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EspecieBovideo>> Get(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especieBovideo = await contexto.EspecieBovideos.FindAsync(id);

            if (especieBovideo == null)
            {
                return NotFound();
            }
            return especieBovideo;
        }

        // POST api/<EspecieBovideoController>
        [HttpPost]
        public async Task<ActionResult<EspecieBovideo>> Post([FromBody] EspecieBovideo especieBovideo)
        {
            if (especieBovideo == null)
            {
                return NotFound();
            }
            await contexto.EspecieBovideos.AddAsync(especieBovideo);
            await contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { especieBovideo });
        }

        // PUT api/<EspecieBovideoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EspecieBovideo especieBovideo)
        {
            if (id != especieBovideo.Id)
            {
                return BadRequest();
            }

            contexto.Entry(especieBovideo).State = EntityState.Modified;
            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecieBovideoExists(especieBovideo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new { especieBovideo });
        }

        private Boolean EspecieBovideoExists(int id) => contexto.EspecieBovideos.Any(e => e.Id == id);
    }
}
