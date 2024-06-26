﻿using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Domain.Shared;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an existing user is modified.
/// </summary>
public class UserUpdatedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? FirstName { get; set; }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? MiddleName { get; set; }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? LastName { get; set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public Modification<string>? FullName { get; set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public Modification<PersonNameUnit>? Nickname { get; set; }

  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public Modification<DateTime?>? Birthdate { get; set; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Modification<GenderUnit>? Gender { get; set; }
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public Modification<LocaleUnit>? Locale { get; set; }
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public Modification<TimeZoneUnit>? TimeZone { get; set; }

  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public Modification<UrlUnit>? Picture { get; set; }
  /// <summary>
  /// Gets or sets the URL to the profile page of the user.
  /// </summary>
  public Modification<UrlUnit>? Profile { get; set; }
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public Modification<UrlUnit>? Website { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the user.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = [];

  /// <summary>
  /// Gets a value indicating whether or not the user is being modified.
  /// </summary>
  [JsonIgnore]
  public bool HasChanges => FirstName != null || MiddleName != null || LastName != null || FullName != null || Nickname != null
    || Birthdate != null || Gender != null || Locale != null || TimeZone != null
    || Picture != null || Profile != null || Website != null
    || CustomAttributes.Count > 0;
}
