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
        Task<Tarefa> UpdateAsync(Tarefa tarefa);
        Task<bool> Delete(Guid id);

    }
}