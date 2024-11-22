using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Domain.Model;

namespace GerenciadorTarefas.Application.Service.Interface
{
    public interface ITarefaService
    {
        Task<(IEnumerable<RequestTarefa> Tarefas, int TotalCount)> GetAllAsync(int pageNumber, int pageSize);
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> CreateAsync(RequestTarefa tarefa);
        Task<bool> UpdateAsync(RequestTarefa tarefa);
        Task<bool> DeleteAsync(Guid id);
    }
}
