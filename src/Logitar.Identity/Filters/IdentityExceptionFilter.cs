using FluentValidation;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logitar.Identity.Filters;

public class IdentityExceptionFilter : ExceptionFilterAttribute
{
  private static readonly Dictionary<Type, Func<ExceptionContext, IActionResult>> _handlers = new()
  {
    [typeof(EmailAddressAlreadyUsedException)] = HandleEmailAddressAlreadyUsedException,
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
    else if (context.Exception is InvalidCredentialsException)
    {
      Error error = new(code: "InvalidCredentials", message: "The specified credentials did not match.");
      context.Result = new BadRequestObjectResult(error);
      context.ExceptionHandled = true;
    }
    else
    {
      base.OnException(context);
    }
  }


  private static ConflictObjectResult HandleEmailAddressAlreadyUsedException(ExceptionContext context)
  {
    EmailAddressAlreadyUsedException exception = (EmailAddressAlreadyUsedException)context.Exception;
    Error error = new(GetErrorCode(exception), EmailAddressAlreadyUsedException.BaseMessage, new ErrorData[]
    {
      new(nameof(EmailAddressAlreadyUsedException.TenantId), exception.TenantId),
      new(nameof(EmailAddressAlreadyUsedException.EmailAddress), exception.EmailAddress)
    });
    return new ConflictObjectResult(error);
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
