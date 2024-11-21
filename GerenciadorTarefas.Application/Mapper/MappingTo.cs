using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Communication.Response;
using GerenciadorTarefas.Domain.Model;

namespace GerenciadorTarefas.Application.Mapper
{
    public class MappingTo
    {
        public RequestTarefa MapToRequestTarefa(Tarefa tarefa)
        {
            return new RequestTarefa
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataCriacao = tarefa.DataCriacao,
                DataFinalizacao = tarefa.DataFinalizacao,
                Status = tarefa.Status
            };
        }

        public Tarefa MapToTarefa(RequestTarefa request)
        {
            return new Tarefa
            {
                Id = request.Id,
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataCriacao = request.DataCriacao,
                DataFinalizacao = request.DataFinalizacao,
                Status = request.Status
            };
        }

        public ResponseTarefa MapToResponseTarefa(Tarefa tarefa)
        {
            return new ResponseTarefa
            {
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataFinalizacao = tarefa.DataFinalizacao,
                Status = tarefa.Status
            };
        }

        public RequestUsuario MapToRequestUsuario(Usuario usuario)
        {
            return new RequestUsuario
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                Password = usuario.Password,
                DataCriacao = usuario.DataCriacao,
                IsActive = usuario.IsActive
            };
        }

        public Usuario MapToUsuario(RequestUsuario usuario)
        {
            return new Usuario
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                Password = usuario.Password,
                DataCriacao = usuario.DataCriacao,
                IsActive = usuario.IsActive
            };
        }

        public ResponseUsuario MapToResponseUsuario(Usuario usuario)
        {
            return new ResponseUsuario
            {
                Name = usuario.Name,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
            };
        }
    }
}
