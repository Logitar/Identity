using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an API key is updated.
/// </summary>
public record ApiKeyUpdated : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the new display name of the API key.
  /// </summary>
  public DisplayName? DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the new description of the API key.
  /// </summary>
  public Change<Description>? Description { get; set; }
  /// <summary>
  /// Gets or sets the new expiration date and time of the API key.
  /// </summary>
  public DateTime? ExpiresOn { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the API key.
  /// </summary>
  public Dictionary<Identifier, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the API key has been updated.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => DisplayName != null || Description != null || ExpiresOn != null || CustomAttributes.Count > 0;
}
