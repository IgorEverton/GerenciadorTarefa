using FluentValidation;
using GerenciadorTarefas.Model;
using GerenciadorTarefas.MenssageExceptions;
using GerenciadorTarefas.Communication.Request;

namespace GerenciadorTarefas.Validation
{
    public class TarefaValidator: AbstractValidator<RequestTarefa>
    {
        public TarefaValidator() 
        {
            RuleFor(task => task.Titulo).NotEmpty().WithMessage(ResourceMenssagesException.TITLE_EMPTY);
            RuleFor(task => task.Descricao).MaximumLength(500).WithMessage(ResourceMenssagesException.DESCRIPTION_MAX_CARAC);
            RuleFor(task => task.DataFinalizacao).GreaterThan(task => task.DataCriacao).WithMessage(ResourceMenssagesException.COMPLETION_DATE);
        }
    }
}
