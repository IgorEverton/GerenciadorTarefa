using System.Collections.Generic;

namespace GerenciadorTarefas.Exceptions.MenssageExceptions.ExceptionBase
{
    public class ErrorOnValidationExceptions : GerenciadorTarefasExceptions
    {
        public IList<string> ErrorMenssages { get; set; }
        public ErrorOnValidationExceptions(IList<string> errorMensasges)
        {
            ErrorMenssages = errorMensasges;
        }
    }
}
