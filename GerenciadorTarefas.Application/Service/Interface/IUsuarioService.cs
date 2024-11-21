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
        Task<Usuario> CreateUserAsync(RequestUsuario request);
        Task<bool> UpdateUserAsync(RequestUsuario request);
        Task<bool> DeleteUserAsync(Guid id);
        Task<string> AuthenticateAsync(string email, string senha);
        Task<bool> GetByEmailAsync(string email);
    }

}
