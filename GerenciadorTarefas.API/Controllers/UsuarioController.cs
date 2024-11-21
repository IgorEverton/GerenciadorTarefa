using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Communication.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using GerenciadorTarefas.Application.Authentication.Inteface;

namespace GerenciadorTarefas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UsuarioController(IUsuarioService usuarioService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioService = usuarioService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }


        [HttpPost("register-user")]
        public async Task<IActionResult> CreateUser([FromBody] RequestUsuario request)
        {
            if(request == null) return BadRequest("Dados inválidos");

            var usuarioEncontrado = _usuarioService.GetByEmailAsync(request.Email);

            if(usuarioEncontrado != null)
            {
                return Conflict("Email já cadastrado");
            }

            var novoUsuario = await _usuarioService.CreateUserAsync(request);

            var token = _jwtTokenGenerator.GenerateToken(novoUsuario.Id, novoUsuario.Email, novoUsuario.Name);

            return CreatedAtAction(nameof(GetUserById), new { id = novoUsuario.Id }, new
            {
                usuario = novoUsuario,
                token
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Dados inválidos");

            var usuarioEncontrado = _usuarioService.AuthenticateAsync(request.Email, request.Password);
            if(usuarioEncontrado != null)
            {
                return Unauthorized("Email ou senha incorreto");
            }

            var token = await _usuarioService.AuthenticateAsync(request.Email, request.Password);

            return Ok(new { Token = token });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RequestUsuario request)
        {
            var updated = await _usuarioService.UpdateUserAsync(request);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _usuarioService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
