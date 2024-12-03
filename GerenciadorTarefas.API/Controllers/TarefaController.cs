using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Communication.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Application.Mapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GerenciadorTarefas.Domain.Model;
using System.Linq;

namespace GerenciadorTarefas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        private readonly MappingTo _mapper;
        public TarefaController(ITarefaService tarefaService, MappingTo mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }


        [HttpGet("tarefa")]
        [ProducesResponseType(typeof(PagedResponse<ResponseTarefa>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Usuário não autenticado.");

                var (tarefas, totalCount) = await _tarefaService.GetAllAsync(Guid.Parse(userId), pageNumber, pageSize);

                if (tarefas == null || !tarefas.Any())
                {
                    return NoContent();
                }

                var response = new PagedResponse<ResponseTarefa>(tarefas, pageNumber, pageSize, totalCount);

                return Ok(response);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }

        }


        [HttpGet("retornar/{id}")]
        [ProducesResponseType(typeof(ResponseTarefa), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Usuário não autenticado.");

            var tarefaEncontrada = await _tarefaService.GetByIdAsync(id, Guid.Parse(userId));
            if(tarefaEncontrada != null)
            {
                return Ok(tarefaEncontrada);
            }
            return NotFound("Tarefa não encontrada");

        }

        [HttpPost("inserir")]
        [ProducesResponseType(typeof(ResponseTarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTarefa([FromBody]RequestTarefa request)
        {

            if (request == null) return BadRequest("Campos não podem ser nulos");

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Usuário não autenticado.");


                request.UsuarioId = Guid.Parse(userId);
                request.Id = Guid.NewGuid();
                var result = await _tarefaService.CreateAsync(request);

                var serializedData = System.Text.Json.JsonSerializer.Serialize(result);

                var response = _mapper.MapToResponseTarefa(result);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
            
            }
            catch (Exception ex) 
            { 
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTarefa(Guid id, [FromBody] RequestTarefa request)
        {
            if (request == null) return BadRequest("Tarefa não pode ser nula");

            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(usuarioId)) return Unauthorized("Usuário não autenticado.");

                request.Id = id;
                var linhasAtualizas = await _tarefaService.UpdateAsync(Guid.Parse(usuarioId), request);
                if(linhasAtualizas != false)
                {
                    return Ok();
                }
                return NotFound("Tarefa não encontrada");
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("deletar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTarefa(Guid id)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(usuarioId)) return Unauthorized("Usuário não autenticado.");

                var tarefaEncontrada = await _tarefaService.GetByIdAsync(id, Guid.Parse(usuarioId));
                if (tarefaEncontrada != null)
                {
                    await _tarefaService.DeleteAsync(tarefaEncontrada.Id, Guid.Parse(usuarioId));
                    return NoContent();
                }
                return NotFound("Tarefa não encontrada");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
