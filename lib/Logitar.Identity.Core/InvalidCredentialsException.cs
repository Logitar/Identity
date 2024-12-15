namespace Logitar.Identity.Core;

/// <summary>
/// The exception raised when credential validation failed.
/// </summary>
public class InvalidCredentialsException : IdentityException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified credentials did not match.";

  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class.
  /// </summary>
  /// <param name="message">The error message.</param>
  /// <param name="innerException">The inner exception.</param>
  public InvalidCredentialsException(string message = ErrorMessage, Exception? innerException = null) : base(message, innerException)
  {
  }
}
