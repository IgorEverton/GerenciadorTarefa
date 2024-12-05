using Azure.Core;
using FluentValidation;
using GerenciadorTarefas.Application.Mapper;
using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Communication.Response;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Domain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Application.Service
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;
        private readonly IValidator<RequestTarefa> _validator;
        private readonly MappingTo _mapper;

        public TarefaService(ITarefaRepository repository, IValidator<RequestTarefa> validator, MappingTo mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ResponseTarefa> Tarefas, int TotalCount)> GetAllAsync(Guid usuarioId, int pageNumber, int pageSize)
        {

            var tarefas = await _repository.GetAllAsync(usuarioId, pageNumber, pageSize);
            Console.WriteLine($"Total de tarefas obtidas do repositório: {tarefas?.Count() ?? 0}");
            
            var totalCount = await _repository.GetTotalCountAsync(usuarioId);
            Console.WriteLine($"TotalCount obtido: {totalCount}");
            var responseTarefas = tarefas.Select(tarefa =>
            {
                var mapped = _mapper.MapToResponseTarefa(tarefa);
                Console.WriteLine($"Tarefa mapeada: Titulo={mapped?.Titulo}, Status={mapped?.Status}");
                return mapped;
            }).ToList();

            return (responseTarefas, totalCount);
        }

        public Task<Tarefa> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<Tarefa> CreateAsync(RequestTarefa request, Guid usuarioId)
        {
            var resultValidator = await _validator.ValidateAsync(request);

            if (!resultValidator.IsValid)
            {
                throw new ValidationException(resultValidator.Errors);
            }
            var tarefa = _mapper.MapToTarefa(request);

            return await _repository.CreateAsync(tarefa);
        }


        public async Task<bool> UpdateAsync(Guid usuarioId, RequestTarefa request)
        {

            var resultValidator = await _validator.ValidateAsync(request);
            if (!resultValidator.IsValid)
            {
                throw new ValidationException(resultValidator.Errors);
            }

            var tarefa = await _repository.GetByIdAsync(request.Id);

            if (tarefa == null || tarefa.UsuarioId != usuarioId)
                return false;

            return await _repository.UpdateAsync(_mapper.MapToTarefa(request));
        }

        public async Task<bool> DeleteAsync(Guid id, Guid usuarioId)
        {
            var tarefa = await _repository.GetByIdAsync(id);

            if (tarefa == null || tarefa.UsuarioId != usuarioId)
                return false;

            return await _repository.DeleteAsync(id);
        }
    }



}
