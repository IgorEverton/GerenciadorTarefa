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

        [HttpGet("retornar-usuario/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound("Usuário não encontrada");
            return Ok(usuario);
        }


        [HttpPost("registrar-usuario")]
        [ProducesResponseType(typeof(ResponseUsuario), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] RequestUsuario request)
        {
            if(request == null) return BadRequest("Dados inválidos");

            try
            {
                var usuarioEncontrado = await _usuarioService.GetByEmailAsync(request.Email);

                if (usuarioEncontrado) return Conflict("Email já cadastrado");


                var novoUsuario = await _usuarioService.CreateUserAsync(request);

                var linkAtualizacao = Url.Action(nameof(Update), "Usuario", new { id = novoUsuario.Id }, Request.Scheme);

                var token = _jwtTokenGenerator.GenerateToken(novoUsuario.Id, novoUsuario.Email, novoUsuario.Name);

                return CreatedAtAction(nameof(GetUserById), new { id = novoUsuario.Id }, new
                {
                    usuario = novoUsuario,
                    token,
                    links = new
                    {
                        atualizar = linkAtualizacao
                    }
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
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Dados inválidos.");
            }
            try
            {
                var token = await _usuarioService.AuthenticateAsync(request.Email, request.Password);

                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Email ou senha incorretos.");
            }
            catch (Exception ex) 
            {
                  return StatusCode(500, ex.Message);          
            }


        }

        [HttpPatch("atualizar-usuario/{id}")]
        public async Task<IActionResult> Update([FromBody] RequestUsuario request)
        {
            if (request == null) return BadRequest("Usuário não pode ser nulo");

            try
            {
                var usuarioAtualizado = await _usuarioService.UpdateUserAsync(request);
                if (!usuarioAtualizado) return NotFound("Usuário não encontrado");
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
