using Logitar.Identity.Sessions.Events;

namespace Logitar.Identity.Sessions.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="SessionRefreshedEvent"/> class.
/// </summary>
internal class SessionRefreshedValidator : SessionSavedValidator<SessionRefreshedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionRefreshedValidator"/> class.
  /// </summary>
  public SessionRefreshedValidator() : base()
  {
  }
}
