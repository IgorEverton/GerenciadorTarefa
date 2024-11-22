using System;

namespace GerenciadorTarefas.Communication.Request
{
    public class RequestUsuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }
    }

}
