using GerenciadorTarefas.Data;
using GerenciadorTarefas.Model;
using GerenciadorTarefas.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly DbContext _context;
        public TarefaRepository(DbContext context)
        {
            _context = context;
        }
        public Task<Tarefa> CreateAsync(Tarefa tarefa)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tarefa> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Tarefa> UpdateAsync(Tarefa tarefa)
        {
            throw new NotImplementedException();
        }
    }
}
