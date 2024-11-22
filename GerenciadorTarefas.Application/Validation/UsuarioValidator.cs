using FluentValidation;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Exceptions.MenssageExceptions;

namespace GerenciadorTarefas.Application.Validation
{
    public class UsuarioValidator : AbstractValidator<RequestUsuario>
    {
        public UsuarioValidator()
        {
            RuleFor(task => task.Name).NotEmpty().WithMessage(ResourceMenssagesException.USER_NAME_EMPTY);
            RuleFor(task => task.Email).NotEmpty().WithMessage(ResourceMenssagesException.USER_EMAIL_EMPTY);
            RuleFor(task => task.Email).EmailAddress().WithMessage(ResourceMenssagesException.USER_EMAIL_INVALID);
            RuleFor(task => task.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMenssagesException.USER_PASSWORD_EMPTY);
        }
    }
}