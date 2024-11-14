using System;

namespace GerenciadorTarefas.Model
{
    public class Tarefa
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataFinalizacao { get; set; }
        public bool Status { get; set; } = true;
    }
}
