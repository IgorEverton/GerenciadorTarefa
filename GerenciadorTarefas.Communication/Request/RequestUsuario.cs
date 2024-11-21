using System;

namespace GerenciadorTarefas.Communication.Request
{
    public class RequestUsuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public DateTime DataCriacao { get; set; }

        public bool IsActive { get; set; }
    }

}
