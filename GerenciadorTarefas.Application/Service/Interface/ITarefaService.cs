using GerenciadorTarefas.Model;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using GerenciadorTarefas.Communication.Request;

namespace GerenciadorTarefas.Service.Interface
{
    public interface ITarefaService
    {
        Task<IEnumerable<RequestTarefa>> GetAllAsync();
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> CreateAsync(RequestTarefa tarefa);
        Task<bool> UpdateAsync(RequestTarefa tarefa);
        Task<bool> DeleteAsync(Guid id);
    }
}
