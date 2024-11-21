using System;

namespace GerenciadorTarefas.Application.Authentication.Inteface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, string name);
    }
}
