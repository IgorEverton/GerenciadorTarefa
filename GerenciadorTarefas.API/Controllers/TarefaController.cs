﻿using GerenciadorTarefas.Communication.Request;
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
using System.IdentityModel.Tokens.Jwt;

namespace GerenciadorTarefas.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tarefas")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        private readonly MappingTo _mapper;
        public TarefaController(ITarefaService tarefaService, MappingTo mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ResponseTarefa>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                //var userId = "3FA85F64-5717-4562-B3FC-2C963F66AFA6";
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autenticado ou ID inválido.");
                }

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
                Console.WriteLine($"Erro: {ex.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }

        }


        //[HttpGet("retornar/{id}")]
        //[ProducesResponseType(typeof(ResponseTarefa), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var usuarioId))
        //    {
        //        return Unauthorized("Usuário não autenticado ou ID inválido.");
        //    }

        //    var tarefaEncontrada = await _tarefaService.GetByIdAsync(id, Guid.Parse(userId));
        //    if(tarefaEncontrada != null)
        //    {
        //        return Ok(tarefaEncontrada);
        //    }
        //    return NotFound("Tarefa não encontrada");

        //}

        [HttpPost]
        [ProducesResponseType(typeof(ResponseTarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTarefa([FromBody]RequestTarefa request)
        {

            if (request == null) return BadRequest("Campos não podem ser nulos");
            //var userId = "3FA85F64-5717-4562-B3FC-2C963F66AFA6";

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"chave recuperada{userId}");
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var usuarioId))
            {
                return Unauthorized("Usuário não autenticado ou ID inválido.");
            }

            try
            {
                var result = await _tarefaService.CreateAsync(request, Guid.Parse(userId));

                var serializedData = System.Text.Json.JsonSerializer.Serialize(result);

                var response = _mapper.MapToResponseTarefa(result);

                return CreatedAtAction(nameof(GetAll), new { id = result.Id }, response);
            
            }
            catch (Exception ex) 
            { 
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTarefa(Guid id, [FromBody] RequestTarefa request)
        {
            if (request == null) return BadRequest("Tarefa não pode ser nula");

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //var userId = "3FA85F64-5717-4562-B3FC-2C963F66AFA6";
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var usuarioId))
                {
                    return Unauthorized("Usuário não autenticado ou ID inválido.");
                }

                request.Id = id;
                var linhasAtualizas = await _tarefaService.UpdateAsync(Guid.Parse(userId), request);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTarefa(Guid id)
        {
            try
            {
                //var userId = "3FA85F64-5717-4562-B3FC-2C963F66AFA6";
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var usuarioId))
                {
                    return Unauthorized("Usuário não autenticado ou ID inválido.");
                }

                var tarefaEncontrada = await _tarefaService.GetByIdAsync(id);
                if (tarefaEncontrada != null)
                {
                    await _tarefaService.DeleteAsync(tarefaEncontrada.Id, Guid.Parse(userId));
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
