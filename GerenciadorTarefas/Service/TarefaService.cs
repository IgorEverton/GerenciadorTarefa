using Azure.Core;
using FluentValidation;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Model;
using GerenciadorTarefas.Repository;
using GerenciadorTarefas.Repository.Interface;
using GerenciadorTarefas.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Service
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;
        private readonly IValidator<Tarefa> _validator;

        public TarefaService(ITarefaRepository repository, IValidator<Tarefa> validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public TarefaService(ITarefaRepository @object)
        {
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            if (_repository == null)
            {
                throw new InvalidOperationException("Repositório não foi injetado corretamente.");
            }

            var tarefas = await _repository.GetAllAsync();
            return tarefas ?? Enumerable.Empty<Tarefa>(); // Garante que nunca será nulo
        }

        public Task<Tarefa> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        private Tarefa MapToTarefa(RequestTarefa request)
        {
            return new Tarefa
            {
                Id = request.Id,
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataCriacao = request.DataCriacao,
                DataFinalizacao = request.DataFinalizacao,
                Status = request.Status
            };
        }


        public async Task<Tarefa> CreateAsync(RequestTarefa request)
        {

            var tarefa = MapToTarefa(request);
            var resultValidator = await _validator.ValidateAsync(tarefa);

            if (!resultValidator.IsValid)
            {
                throw new ValidationException(resultValidator.Errors);
            }

            return await _repository.CreateAsync(tarefa);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> UpdateAsync(Tarefa tarefa)
        {
            var resultValidator = await _validator.ValidateAsync(tarefa);
            if (!resultValidator.IsValid)
            {
                throw new ValidationException(resultValidator.Errors);
            }
            return await _repository.UpdateAsync(tarefa);
        }
    }


}
