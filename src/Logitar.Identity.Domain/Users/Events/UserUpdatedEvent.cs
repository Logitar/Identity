﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users.Events;

/// <summary>
/// The event raised when an existing user is modified.
/// </summary>
public record UserUpdatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public Modification<AddressUnit>? Address { get; internal set; }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public Modification<EmailUnit>? Email { get; internal set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public Modification<PhoneUnit>? Phone { get; internal set; }

  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? FirstName { get; internal set; }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? MiddleName { get; internal set; }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public Modification<PersonNameUnit>? LastName { get; internal set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public Modification<string>? FullName { get; internal set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public Modification<PersonNameUnit>? Nickname { get; internal set; }

  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public Modification<DateTime?>? Birthdate { get; internal set; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Modification<GenderUnit>? Gender { get; internal set; }
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public Modification<LocaleUnit>? Locale { get; internal set; }
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public Modification<TimeZoneUnit>? TimeZone { get; internal set; }

  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public Modification<UrlUnit>? Picture { get; internal set; }
  /// <summary>
  /// Gets or sets the URL to the profile page of the user.
  /// </summary>
  public Modification<UrlUnit>? Profile { get; internal set; }
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public Modification<UrlUnit>? Website { get; internal set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the user.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; } = new();

  /// <summary>
  /// Gets a value indicating whether or not the user is being modified.
  /// </summary>
  public bool HasChanges => Address != null || Email != null || Phone != null
    || FirstName != null || MiddleName != null || LastName != null || FullName != null || Nickname != null
    || Birthdate != null || Gender != null || Locale != null || TimeZone != null
    || Picture != null || Profile != null || Website != null
    || CustomAttributes.Any();
}