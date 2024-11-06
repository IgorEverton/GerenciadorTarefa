using GerenciadorTarefas.Model;
using GerenciadorTarefas.Service.Interface;
using System.Threading.Tasks;
using System;

namespace GerenciadorTarefas.Service
{
    public class TarefaTestService
    {
        private readonly ITarefaService _tarefaService;

        public TarefaTestService(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        public async Task CriarTarefaExemplo()
        {
            var tarefa = new Tarefa
            {
                Titulo = "Nova Tarefa",
                Descricao = "Descrição da tarefa",
                DataCriacao = DateTime.Now,
                DataFinalizacao = DateTime.Now.AddDays(2),
                Status = false
            };

            var tarefaCriada = await _tarefaService.CreateAsync(tarefa);
            Console.WriteLine($"Tarefa criada com ID: {tarefaCriada.Id}");
        }
    }
}
