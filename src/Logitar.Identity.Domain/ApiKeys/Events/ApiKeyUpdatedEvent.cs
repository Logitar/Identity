using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyUpdatedEvent : DomainEvent
{
  public DisplayNameUnit? DisplayName { get; set; }
  public Modification<DescriptionUnit>? Description { get; set; }
  public DateTime? ExpiresOn { get; set; }

  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  public bool HasChanges => DisplayName != null || Description != null || CustomAttributes.Count > 0;
}
