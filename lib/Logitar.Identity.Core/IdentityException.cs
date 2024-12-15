namespace Logitar.Identity.Core;

/// <summary>
/// The base class of Identity exceptions.
/// </summary>
public abstract class IdentityException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="IdentityException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="innerException">The inner exception.</param>
  protected IdentityException(string message, Exception? innerException = null) : base(message, innerException)
  {
  }
}
