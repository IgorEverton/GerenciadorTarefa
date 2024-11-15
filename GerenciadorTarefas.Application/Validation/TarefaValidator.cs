using FluentValidation;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Exceptions.MenssageExceptions;

namespace GerenciadorTarefas.Application.Validation
{
    public class TarefaValidator : AbstractValidator<RequestTarefa>
    {
        public TarefaValidator()
        {
            RuleFor(task => task.Titulo).NotEmpty().WithMessage(ResourceMenssagesException.TITLE_EMPTY);
            RuleFor(task => task.Descricao).MaximumLength(500).WithMessage(ResourceMenssagesException.DESCRIPTION_MAX_CARAC);
            RuleFor(task => task.DataFinalizacao).GreaterThan(task => task.DataCriacao).WithMessage(ResourceMenssagesException.COMPLETION_DATE);
        }
    }
}
