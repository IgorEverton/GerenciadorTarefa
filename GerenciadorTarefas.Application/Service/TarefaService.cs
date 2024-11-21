using FluentValidation;
using GerenciadorTarefas.Application.Mapper;
using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Communication.Request;
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

        public async Task<(IEnumerable<RequestTarefa> Tarefas, int TotalCount)> GetAllAsync(int pageNumber, int pageSize)
        {

            var TotalCount = await _repository.GetTotalCountAsync();

            var tarefas = await _repository.GetAllAsync(pageNumber, pageSize);

            var requestTarefas = tarefas.Select(_mapper.MapToRequestTarefa);

            return (requestTarefas, TotalCount);
        }

        public Task<Tarefa> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<Tarefa> CreateAsync(RequestTarefa request)
        {
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
