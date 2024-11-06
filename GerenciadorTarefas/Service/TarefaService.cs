using FluentValidation;
using GerenciadorTarefas.Model;
using GerenciadorTarefas.Repository.Interface;
using GerenciadorTarefas.Service.Interface;
using System;
using System.Collections.Generic;
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


        public Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Tarefa> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {

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
