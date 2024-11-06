using Dapper;
using GerenciadorTarefas.Data;
using GerenciadorTarefas.Model;
using GerenciadorTarefas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly IDbConnection _connection;
        public TarefaRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            var sqlQuery = "SELECT * FROM Tarefa";
            return await _connection.QueryAsync<Tarefa>(sqlQuery);
        }

        public async Task<Tarefa> GetByIdAsync(Guid id)
        {
            string sqlQuery = "SELECT FROM Tarefa WHERE Id = @id";
            return await _connection.QuerySingleOrDefaultAsync<Tarefa>(sqlQuery, new {id});
        }

        public async Task<Tarefa> CreateAsync(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            tarefa.DataCriacao = DateTime.Now;
            string sqlQuery = "INSERT INTO Tarefas (Titulo, Descricao, DataCriacao, DataFinalizacao, Status) " +
                "VALUES (@Id, @Titulo, @Descricao, @DataCriacao, @DataFinalizacao, @Status)";

            var linhasAfetadas =  await _connection.ExecuteAsync(sqlQuery, tarefa);
            return linhasAfetadas > 0 ? tarefa : null;

        }
        public async Task<bool> UpdateAsync(Tarefa tarefa)
        {
            string sqlString = "UPDATE Tarefa SET @Titulo, @Descricao, @DataCriacao, @DataFinalizacao, @Status WHERE Id = @Id";

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
            string sqlQuery = "DELETE FROM Tarefa WHERE ID = @id";
            var linhasAfetadas = await _connection.ExecuteAsync(sqlQuery, new {id});

            return linhasAfetadas > 0;
        }


    }
}
