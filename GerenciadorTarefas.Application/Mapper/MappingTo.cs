using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Communication.Response;
using GerenciadorTarefas.Domain.Model;
using System;

namespace GerenciadorTarefas.Application.Mapper
{
    public class MappingTo
    {
        public RequestTarefa MapToRequestTarefa(Tarefa tarefa)
        {
            return new RequestTarefa
            {
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataFinalizacao = tarefa.DataFinalizacao,
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
                Status = request.Status,
                UsuarioId = request.UsuarioId,
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
                Name = usuario.Name,
                Email = usuario.Email,
                Password = usuario.Password,
            };
        }

        public Usuario MapToUsuario(RequestUsuario usuario)
        {
            return new Usuario
            {
                Name = usuario.Name,
                Email = usuario.Email,
                Password = usuario.Password,
            };
        }

        public ResponseUsuario MapToResponseUsuario(Usuario usuario)
        {
            return new ResponseUsuario
            {
                Name = usuario.Name,
                Email = usuario.Email,
            };
        }


    }
}
