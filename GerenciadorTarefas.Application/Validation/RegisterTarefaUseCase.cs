using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Communication.Response;
using GerenciadorTarefas.Exceptions.MenssageExceptions.ExceptionBase;
using System.Linq;

namespace GerenciadorTarefas.Application.Validation
{
    public class RegisterTarefaUseCase
    {
        public ResponseTarefa Execute(RequestTarefa request)
        {
            Validate(request);

            return new ResponseTarefa
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataFinalizacao = request.DataFinalizacao,
                Status = request.Status
            };
        }
        private void Validate(RequestTarefa request)
        {
            var validator = new TarefaValidator();

            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var erroMensagens = result.Errors.Select(erro => erro.ErrorMessage).ToList();
                throw new ErrorOnValidationExceptions(erroMensagens);
            }
        }

    }
}
