using System;

namespace GerenciadorTarefas.Domain.Model
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }


        public Usuario() { }

        public Usuario(string nome, string email, string password)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Password = password;
            DataCriacao = DateTime.UtcNow;
        }
    }

}
