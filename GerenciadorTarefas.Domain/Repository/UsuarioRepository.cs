using Dapper;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Domain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Domain.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _connection;

        public UsuarioRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Usuarios WHERE Id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Id = id });
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var query = "SELECT * FROM Usuarios";
            return await _connection.QueryAsync<Usuario>(query);
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            var query = @"
            INSERT INTO Usuarios (Id, Nome, Email, Password)
            VALUES (@Id, @Nome, @Email, @Password)";
            return await _connection.ExecuteAsync(query, usuario);
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var query = @"
            UPDATE Usuarios 
            SET Nome = @Nome, Email = @Email, Password = @Password
            WHERE Id = @Id";
            return await _connection.ExecuteAsync(query, usuario) > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var query = "DELETE FROM Usuarios WHERE Id = @Id";
            return await _connection.ExecuteAsync(query, new { Id = id }) > 0;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            var query = "SELECT * FROM Usuarios WHERE Email = @Email";
            return await _connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Email = email });
        }
    }

}
