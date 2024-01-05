namespace Logitar.Identity.Domain.Shared;

public class InvalidCredentialsException : Exception
{
  public const string ErrorMessage = "The specified credentials did not match.";

  public InvalidCredentialsException(string message = ErrorMessage, Exception? innerException = null) : base(message, innerException)
  {
  }
}
