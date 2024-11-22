using System;

namespace GerenciadorTarefas.Domain.Model
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public DateTime DataCriacao { get; set; }
        public bool IsActive { get; set; } 

    }

}
