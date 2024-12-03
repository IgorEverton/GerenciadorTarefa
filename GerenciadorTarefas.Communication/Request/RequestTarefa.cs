using System;

namespace GerenciadorTarefas.Communication.Request
{
    public class RequestTarefa
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataFinalizacao { get; set; }
        public bool Status { get; set; } = true;
        public Guid UsuarioId { get; set; }
    }
}
