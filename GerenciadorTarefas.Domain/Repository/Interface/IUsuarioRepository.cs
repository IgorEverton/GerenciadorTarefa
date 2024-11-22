using GerenciadorTarefas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Domain.Repository.Interface
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByIdAsync(Guid id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<int> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(Guid id);
        Task<Usuario> GetByEmailAsync(string email);
    }

}
