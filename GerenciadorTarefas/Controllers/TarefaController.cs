using GerenciadorTarefas.Model;
using GerenciadorTarefas.Service;
using GerenciadorTarefas.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }
        [HttpGet("retorna-tarefas")]
        public async Task<IActionResult> GetAll()
        {
            var tarefas = await _tarefaService.GetAllAsync();
            return Ok(tarefas);
        }
        [HttpGet("retornar-tarefa/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tarefaEncontrada = await _tarefaService.GetByIdAsync(id);
            if(tarefaEncontrada != null)
            {
                return Ok(tarefaEncontrada);
            }
            return BadRequest();
        }
        [HttpPost("inserir-tarefa")]
        public async Task<IActionResult> PostTarefa([FromBody]Tarefa tarefa)
        {
            if (tarefa == null) return BadRequest("Campos não podem ser nulos");

            try
            {
                tarefa.Id = Guid.NewGuid();
                tarefa.DataCriacao = DateTime.Now;
                var tarefaCriada = await _tarefaService.CreateAsync(tarefa);

                // Teste de serialização manual
                var serializedData = System.Text.Json.JsonSerializer.Serialize(tarefaCriada);

                return CreatedAtAction(nameof(GetById), new { id = tarefaCriada.Id }, tarefaCriada);
            
                //CreatedAtAction(nameof(GetById), new {id = tarefaCriada.Id}, tarefaCriada);
            }
            catch (Exception ex) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("atualizar-tarefa")]
        public async Task<IActionResult> PutTarefa([FromBody]Tarefa tarefa)
        {
            if (tarefa == null) return BadRequest("Tarefa não pode ser nula");
            try
            {
                var linhasAtualizas = await _tarefaService.UpdateAsync(tarefa);
                if(linhasAtualizas != null)
                {
                    return Ok();
                }
                return BadRequest("Tafera não encontrada");
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
