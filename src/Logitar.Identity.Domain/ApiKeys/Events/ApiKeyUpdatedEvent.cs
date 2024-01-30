using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

/// <summary>
/// The event raised when an existing API key is modified.
/// </summary>
public record ApiKeyUpdatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the display name of the API key.
  /// </summary>
  public DisplayNameUnit? DisplayName { get; internal set; }
  /// <summary>
  /// Gets or sets the description of the API key.
  /// </summary>
  public Modification<DescriptionUnit>? Description { get; internal set; }
  /// <summary>
  /// Gets or sets the expiration date and time of the API key.
  /// </summary>
  public DateTime? ExpiresOn { get; internal set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the API key.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the API key is being modified.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => DisplayName != null || Description != null || ExpiresOn != null || CustomAttributes.Count > 0;
}
