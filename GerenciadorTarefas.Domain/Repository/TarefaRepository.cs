using Dapper;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Domain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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


        public async Task<IEnumerable<Tarefa>>GetAllAsync(Guid usuarioId, int pageNumber, int pageSize)
        {

            Console.WriteLine($"Consulta de tarefas para o usuário: {usuarioId}, Pág.: {pageNumber}, Tamanho: {pageSize}");
            var offset = (pageNumber - 1) * pageSize;

            var sqlQuery = @"
                SELECT * FROM Tarefas
                WHERE UsuarioId = @usuarioId
                ORDER BY DataCriacao DESC
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            var tarefas = await _connection.QueryAsync<Tarefa>(sqlQuery, new { UsuarioId = usuarioId, Offset = offset, PageSize = pageSize });
            Console.WriteLine($"Tarefas retornadas do banco: {tarefas?.Count()}");
            return tarefas;
        }


        public async Task<int> GetTotalCountAsync(Guid usuarioId)
        {
            var  sqlQuery = "SELECT COUNT(*) FROM Tarefas WHERE UsuarioId = @usuarioId";
            return await _connection.ExecuteScalarAsync<int>(sqlQuery, new { UsuarioId = usuarioId });
        }


        public async Task<Tarefa> GetByIdAsync (Guid id)
        {
            string sqlQuery = "SELECT * FROM Tarefas WHERE Id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Tarefa>(sqlQuery, new { Id = id });
        }


        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            string sqlQuery = "INSERT INTO Tarefas (Id, Titulo, Descricao, DataCriacao, DataFinalizacao, Status, UsuarioId) " +
                "VALUES (@Id, @Titulo, @Descricao, @DataCriacao, @DataFinalizacao, @Status, @UsuarioId)";

            var linhasAfetadas = await _connection.ExecuteAsync(sqlQuery, tarefa);
            if (linhasAfetadas == 0)
            {
                throw new Exception("Falha ao criar a tarefa no banco de dados.");
            }

            return tarefa;
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

            string sqlQuery = "DELETE FROM Tarefas WHERE Id = @id";
            var linhasAfetadas = await _connection.ExecuteAsync(sqlQuery, new { id });

            return linhasAfetadas > 0;
        }


    }
}
