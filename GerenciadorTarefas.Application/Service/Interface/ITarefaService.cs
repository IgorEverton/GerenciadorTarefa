using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Communication.Response;

namespace GerenciadorTarefas.Application.Service.Interface
{
    public interface ITarefaService
    {
        Task<(IEnumerable<ResponseTarefa> Tarefas, int TotalCount)> GetAllAsync(Guid usuarioId, int pageNumber, int pageSize);
        Task<Tarefa> GetByIdAsync(Guid id);
        Task<Tarefa> CreateAsync(RequestTarefa tarefa, Guid usuarioId);
        Task<bool> UpdateAsync(Guid usuarioId, RequestTarefa tarefa);
        Task<bool> DeleteAsync(Guid id, Guid usuarioId);
    }
}
