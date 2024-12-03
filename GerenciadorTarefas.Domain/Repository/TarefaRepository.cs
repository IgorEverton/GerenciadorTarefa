using Dapper;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Domain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Domain.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly IDbConnection _connection;
        public TarefaRepository(IDbConnection connection)
        {
            _connection = connection;
        }


        public async Task<IEnumerable<Tarefa>> GetAllAsync(Guid usuarioId, int pageNumber, int pageSize)
        {

            var offset = (pageNumber - 1) * pageSize;

            var sqlQuery = @"
                SELECT * FROM Tarefas
                WHERE UsuarioId = @usuarioId
                ORDER BY DataCriacao DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            return await _connection.QueryAsync<Tarefa>(sqlQuery, new { UsuarioId = usuarioId, Offset = offset, PageSize = pageSize });
        }


        public async Task<int> GetTotalCountAsync(Guid usuarioId)
        {
            var  sqlQuery = "SELECT COUNT(*) FROM Tarefas WHERE UsuarioId = @usuarioId";
            return await _connection.ExecuteScalarAsync<int>(sqlQuery, new { UsuarioId = usuarioId });
        }


        public async Task<Tarefa> GetByIdAsync(Guid usuarioId)
        {
            string sqlQuery = "SELECT * FROM Tarefas WHERE Id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Tarefa>(sqlQuery, new { UsuarioId = usuarioId });
        }


        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            string sqlQuery = "INSERT INTO Tarefas (Id, Titulo, Descricao, DataCriacao, DataFinalizacao, Status) " +
                "VALUES (@Id, @Titulo, @Descricao, @DataCriacao, @DataFinalizacao, @Status)";

            var linhasAfetadas = await _connection.ExecuteAsync(sqlQuery, tarefa);
            return linhasAfetadas > 0 ? tarefa : null;

        }


        public async Task<bool> UpdateAsync(Tarefa tarefa)
        {
            string sqlString = "UPDATE Tarefas SET Titulo = @Titulo, Descricao = @Descricao, DataCriacao = @DataCriacao, DataFinalizacao = @DataFinalizacao, Status = @Status WHERE Id = @Id";

            var linhasAlteradas = await _connection.ExecuteAsync(sqlString, new
            {
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Descricao,
                tarefa.DataCriacao,
                tarefa.DataFinalizacao,
                tarefa.Status
            });

            return linhasAlteradas > 0;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            string sqlQueryVerificar = "SELECT COUNT(1) FROM Tarefas WHERE Id = @id";
            var tarefaEncontrada = await _connection.ExecuteScalarAsync<int>(sqlQueryVerificar, new { id });

            if (tarefaEncontrada < 0) return false;

            string sqlQuery = "DELETE FROM Tarefas WHERE ID = @id";
            var linhasAfetadas = await _connection.ExecuteAsync(sqlQuery, new { id });

            return linhasAfetadas > 0;
        }


    }
}
