using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Model;

namespace GerenciadorTarefas.Service.Mapper
{
    public class MapToTarefa
    {
        public MapToTarefa(RequestTarefa request) => new Tarefa
        {
            Id = request.Id,
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            DataCriacao = request.DataCriacao,
            DataFinalizacao = request.DataFinalizacao,
            Status = request.Status
        };

    }
}
