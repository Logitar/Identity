using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserUpdatedEvent : DomainEvent, INotification
{
  public Modification<PersonNameUnit>? FirstName { get; set; }
  public Modification<PersonNameUnit>? MiddleName { get; set; }
  public Modification<PersonNameUnit>? LastName { get; set; }
  public Modification<string>? FullName { get; set; }
  public Modification<PersonNameUnit>? Nickname { get; set; }

  public Modification<DateTime?>? Birthdate { get; set; }
  public Modification<GenderUnit>? Gender { get; set; }
  public Modification<LocaleUnit>? Locale { get; set; }
  public Modification<TimeZoneUnit>? TimeZone { get; set; }

  public bool HasChanges => FirstName != null || MiddleName != null || LastName != null || FullName != null || Nickname != null
    || Birthdate != null || Gender != null || Locale != null || TimeZone != null;
}
