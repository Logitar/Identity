namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The exception raised when credential validation failed.
/// </summary>
public class InvalidCredentialsException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class.
  /// </summary>
  /// <param name="message">The error message.</param>
  /// <param name="innerException">The inner exception.</param>
  public InvalidCredentialsException(string message = "The specified credentials did not match.", Exception? innerException = null)
    : base(message, innerException)
  {
  }
}
