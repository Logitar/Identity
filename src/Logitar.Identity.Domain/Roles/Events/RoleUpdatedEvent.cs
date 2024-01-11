using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleUpdatedEvent : DomainEvent
{
  public Modification<DisplayNameUnit>? DisplayName { get; set; }
  public Modification<DescriptionUnit>? Description { get; set; }

  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  public bool HasChanges => DisplayName != null || Description != null || CustomAttributes.Count > 0;
}
