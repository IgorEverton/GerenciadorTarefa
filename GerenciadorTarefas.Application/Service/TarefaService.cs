using FluentValidation;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Model;
using GerenciadorTarefas.Repository.Interface;
using GerenciadorTarefas.Service.Interface;
using GerenciadorTarefas.Service.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Service
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

        public async Task<IEnumerable<RequestTarefa>> GetAllAsync()
        {
            if (_repository == null)
            {
                throw new InvalidOperationException("Repositório não foi injetado corretamente.");
            }

            var tarefas = await _repository.GetAllAsync();

            var requestTarefas = tarefas.Select(_mapper.MapToRequestTarefa);

            return requestTarefas;
        }

        public Task<Tarefa> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<Tarefa> CreateAsync(RequestTarefa request)
        {

            //var tarefa = MapToTarefa(request);
            var resultValidator = await _validator.ValidateAsync(request);

            if (!resultValidator.IsValid)
            {
                throw new ValidationException(resultValidator.Errors);
            }
            var tarefa = _mapper.MapToTarefa(request);

            return await _repository.CreateAsync(tarefa);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> UpdateAsync(RequestTarefa request)
        {
            var resultValidator = await _validator.ValidateAsync(request);
            if (!resultValidator.IsValid)
            {
                throw new ValidationException(resultValidator.Errors);
            }
            var tarefa = _mapper.MapToTarefa(request);
            return await _repository.UpdateAsync(tarefa);
        }
    }


}
