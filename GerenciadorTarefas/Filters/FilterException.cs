using GerenciadorTarefas.Communication.Response;
using GerenciadorTarefas.MenssageExceptions;
using GerenciadorTarefas.MenssageExceptions.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace GerenciadorTarefas.Filters
{
    public class FilterException : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            throw new System.NotImplementedException();
        }
        private void HandleProjectException(ExceptionContext context)
        {
            if(context.Exception is ErrorOnValidationExceptions)
            {
                var exception = context.Exception as ErrorOnValidationExceptions;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMenssages));
            }
        }
        private void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMenssagesException.UNKNOW_ERROR));
        }
    }
}
