using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Communication.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using GerenciadorTarefas.Application.Authentication.Inteface;
using GerenciadorTarefas.Communication.Response;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Application.Service;

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
            if (usuario == null) return NotFound("Usuário não encontrada");
            return Ok(usuario);
        }


        [HttpPost("register-user")]
        [ProducesResponseType(typeof(ResponseUsuario), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] RequestUsuario request)
        {
            if(request == null) return BadRequest("Dados inválidos");

            try
            {
                request.Id = Guid.NewGuid();
                var usuarioEncontrado = await _usuarioService.GetByEmailAsync(request.Email);

                if (usuarioEncontrado != false) return Conflict("Email já cadastrado");


                var novoUsuario = await _usuarioService.CreateUserAsync(request);

                var token = _jwtTokenGenerator.GenerateToken(novoUsuario.Id, novoUsuario.Email, novoUsuario.Name);

                return CreatedAtAction(nameof(GetUserById), new { id = novoUsuario.Id }, new
                {
                    usuario = novoUsuario,
                    token
                });
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Dados inválidos");

            var usuarioEncontrado = await _usuarioService.AuthenticateAsync(request.Email, request.Password);
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
            if (request == null) return BadRequest("Usuário não pode ser nulo");

            try
            {
                var updated = await _usuarioService.UpdateUserAsync(request);
                if (!updated) return NotFound("Usuário não encontrado");
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("delete-usuario/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var usuarioEncontrado = await _usuarioService.GetByIdAsync(id);

                if (usuarioEncontrado != null)
                {
                    await _usuarioService.DeleteUserAsync(id);
                    return NoContent();
                }
                return NotFound("Usuário não encontrado");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }

}
