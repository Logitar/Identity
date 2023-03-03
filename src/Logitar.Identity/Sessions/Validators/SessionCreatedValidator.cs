using Logitar.Identity.Sessions.Events;

namespace Logitar.Identity.Sessions.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="SessionCreatedEvent"/> class.
/// </summary>
internal class SessionCreatedValidator : SessionSavedValidator<SessionCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionCreatedValidator"/> class.
  /// </summary>
  public SessionCreatedValidator() : base()
  {
  }
}
