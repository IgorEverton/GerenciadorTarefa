using GerenciadorTarefas.Domain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Domain.Repository.Interface
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync(Guid userId, int pageNumber, int pageSize);
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> CreateAsync(Tarefa tarefa);
        Task<bool> UpdateAsync(Tarefa tarefa);
        Task<bool> DeleteAsync(Guid id);
        Task<int> GetTotalCountAsync(Guid userId);
    }
}