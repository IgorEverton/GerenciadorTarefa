using System;

namespace GerenciadorTarefas.Communication.Request
{
    public class RequestUsuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
