using ControleBovideo.model;
using ControleBovideo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ControleBovideo.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public Contexto Contexto { get; set; }
        public HomeController(Contexto contexto)
        {
            this.Contexto = contexto;
        }

        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] Usuario model)
        {
            // Recupera o usuário
            //var user = UserRepository.Get(model.Username, model.Password);
            var user = Contexto.Usuarios.First(e => e.Username == model.Username && e.Password == model.Password);
            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Oculta a senha
            user.Password = "";

            // Retorna os dados
            return new
            {
                id = user.Id,
                username = user.Username,
                roles = user.Role,
                accessToken = token,
                tokenType = "Bearer"
            };
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostLivro(Usuario usuario)
        {
            Boolean validar = true;
            if (validar)
            {
                usuario.Id = 0;
                usuario.Role = "authenticated";
                Contexto.Usuarios.Add(usuario);
                await Contexto.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { usuario.Id });
            }
            else
            {
                return BadRequest();
            }
        }

        public List<Usuario> Get()
        {
            return Contexto.Usuarios.ToList();
        }
    }
}
