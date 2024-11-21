using GerenciadorTarefas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefas.Communication.Request;

namespace GerenciadorTarefas.Application.Service.Interface
{
    public interface IUsuarioService
    {
        Task<Usuario> GetByIdAsync(Guid id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> CreateAsync(RequestUsuario request);
        Task<bool> UpdateAsync(RequestUsuario request);
        Task<bool> DeleteAsync(Guid id);
        Task<string> AuthenticateAsync(string email, string senha); // Geração de token
    }

}
