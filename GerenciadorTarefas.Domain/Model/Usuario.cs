using System;

namespace GerenciadorTarefas.Domain.Model
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;


        public Usuario() { }

        public Usuario(string name, string email, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
            DataCriacao = DateTime.UtcNow;
        }
    }

}
