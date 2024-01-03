using FluentValidation;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logitar.Identity.Filters;

public class IdentityExceptionFilter : ExceptionFilterAttribute
{
  private static readonly Dictionary<Type, Func<ExceptionContext, IActionResult>> _handlers = new()
  {
    [typeof(ValidationException)] = HandleValidationException
  };

  public override void OnException(ExceptionContext context)
  {
    if (_handlers.TryGetValue(context.Exception.GetType(), out Func<ExceptionContext, IActionResult>? handler))
    {
      context.Result = handler(context);
      context.ExceptionHandled = true;
    }
    else if (context.Exception is UniqueNameAlreadyUsedException uniqueNameAlreadyUsed)
    {
      Error error = new(GetErrorCode(uniqueNameAlreadyUsed), UniqueNameAlreadyUsedException.BaseMessage, new ErrorData[]
      {
        new(nameof(UniqueNameAlreadyUsedException.TenantId), uniqueNameAlreadyUsed.TenantId),
        new(nameof(UniqueNameAlreadyUsedException.UniqueName), uniqueNameAlreadyUsed.UniqueName)
      });
      context.Result = new ConflictObjectResult(error);
      context.ExceptionHandled = true;
    }
    else
    {
      base.OnException(context);
    }
  }

  private static BadRequestObjectResult HandleValidationException(ExceptionContext context)
    => new(new { ((ValidationException)context.Exception).Errors });

  private static string GetErrorCode(Exception exception)
  {
    string code = exception.GetErrorCode();

    int index = code.IndexOf('`');

    return index >= 0 ? code[..index] : code;
  }
}
