using Logitar.Identity.ApiKeys.Events;

namespace Logitar.Identity.ApiKeys.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ApiKeyCreatedEvent"/> class.
/// </summary>
internal class ApiKeyCreatedValidator : ApiKeySavedValidator<ApiKeyCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyCreatedValidator"/> class.
  /// </summary>
  public ApiKeyCreatedValidator() : base()
  {
  }
}
