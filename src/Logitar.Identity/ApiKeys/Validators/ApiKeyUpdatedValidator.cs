using Logitar.Identity.ApiKeys.Events;

namespace Logitar.Identity.ApiKeys.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ApiKeyUpdatedEvent"/> class.
/// </summary>
internal class ApiKeyUpdatedValidator : ApiKeySavedValidator<ApiKeyUpdatedEvent>
{
}
