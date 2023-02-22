using Logitar.Identity.Realms.Events;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="RealmUpdatedEvent"/> class.
/// </summary>
internal class RealmUpdatedValidator : RealmSavedValidator<RealmUpdatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmUpdatedValidator"/> class.
  /// </summary>
  public RealmUpdatedValidator()
  {
  }
}
