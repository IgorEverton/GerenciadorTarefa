using FluentValidation;
using GerenciadorTarefas.Model;

namespace GerenciadorTarefas.Validation
{
    public class TarefaValidator: AbstractValidator<Tarefa>
    {
        public TarefaValidator() 
        {
            RuleFor(task => task.Titulo).NotEmpty().WithMessage("Titulo não pode ser vazio");
            RuleFor(task => task.Descricao).MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres");
            RuleFor(task => task.DataFinalizacao).GreaterThan(task => task.DataCriacao).WithMessage("A data de finalização deve ser posterior a data de criação");
        }
    }
}
