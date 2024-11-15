using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Communication.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Application.Service.Mapper;

namespace GerenciadorTarefas.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        private readonly MappingTo _mapper;
        public TarefaController(ITarefaService tarefaService, MappingTo mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }

        [HttpGet("retorna-tarefas")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            var (tarefas, totalPages) = await _tarefaService.GetAllAsync(pageNumber, pageSize);

            if (tarefas == null)
            {
                return NoContent();
            }

            var response = new
            {
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = tarefas
            };

            return Ok(response);
        }

        [HttpGet("retornar-tarefa/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tarefaEncontrada = await _tarefaService.GetByIdAsync(id);
            if(tarefaEncontrada != null)
            {
                return Ok(tarefaEncontrada);
            }
            return NotFound("Tarefa não encontrada");

        }

        [HttpPost("inserir-tarefa")]
        [ProducesResponseType(typeof(ResponseTarefa), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostTarefa([FromBody]RequestTarefa request)
        {

            if (request == null) return BadRequest("Campos não podem ser nulos");

            try
            {
                request.Id = Guid.NewGuid();
                var result = await _tarefaService.CreateAsync(request);

                // Teste de serialização manual
                var serializedData = System.Text.Json.JsonSerializer.Serialize(result);

                var response = _mapper.MapToResponseTarefa(result);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
            
            }
            catch (Exception ex) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("atualizar-tarefa")]
        public async Task<IActionResult> PutTarefa([FromBody] RequestTarefa tarefa)
        {
            if (tarefa == null) return BadRequest("Tarefa não pode ser nula");
            try
            {
                var linhasAtualizas = await _tarefaService.UpdateAsync(tarefa);
                if(linhasAtualizas != false)
                {
                    return Ok();
                }
                return NotFound("Tafera não encontrada");
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete-tarefa/{id}")]
        public async Task<IActionResult> DeleteTarefa(Guid id)
        {
            try
            {
                var tarefaEncontrada = await _tarefaService.GetByIdAsync(id);
                if (tarefaEncontrada != null)
                {
                    await _tarefaService.DeleteAsync(id);
                    return NoContent();
                }
                return NotFound("Tarefa não encontrada");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
