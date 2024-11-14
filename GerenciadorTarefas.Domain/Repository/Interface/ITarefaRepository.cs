using GerenciadorTarefas.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Repository.Interface
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync();
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> CreateAsync(Tarefa tarefa);
        Task<bool> UpdateAsync(Tarefa tarefa);
        Task<bool> DeleteAsync(Guid id);

    }
}