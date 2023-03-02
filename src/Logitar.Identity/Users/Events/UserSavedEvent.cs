using Logitar.EventSourcing;
using MediatR;
using System.Globalization;

namespace Logitar.Identity.Users.Events;

/// <summary>
/// The base <see cref="UserSavedEvent"/> save event.
/// </summary>
public abstract record UserSavedEvent : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the salted and hashed password of the user.
  /// </summary>
  public string? PasswordHash { get; init; }

  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public ReadOnlyAddress? Address { get; init; }
  /// <summary>
  /// Gets or sets the postal address verification action performed by the event.
  /// </summary>
  public VerificationAction AddressVerification { get; init; }

  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public ReadOnlyEmail? Email { get; init; }
  /// <summary>
  /// Gets or sets the email address verification action performed by the event.
  /// </summary>
  public VerificationAction EmailVerification { get; init; }

  /// <summary>
  /// Gets or sets the phone nunber of the user.
  /// </summary>
  public ReadOnlyPhone? Phone { get; init; }
  /// <summary>
  /// Gets or sets the phone number verification action performed by the event.
  /// </summary>
  public VerificationAction PhoneVerification { get; init; }

  /// <summary>
  /// Gets or sets the first name(s) or given name(s) of the user.
  /// </summary>
  public string? FirstName { get; init; }
  /// <summary>
  /// Gets or sets the middle name(s) of the user.
  /// </summary>
  public string? MiddleName { get; init; }
  /// <summary>
  /// Gets or sets the last name(s) or surname(s) of the user.
  /// </summary>
  public string? LastName { get; init; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public string? FullName { get; init; }
  /// <summary>
  /// Gets or sets the nickname(s) or casual name(s) or the user.
  /// </summary>
  public string? Nickname { get; init; }

  /// <summary>
  /// Gets or sets the birtdate of the user.
  /// </summary>
  public DateTime? Birthdate { get; init; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Gender? Gender { get; init; }

  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public CultureInfo? Locale { get; init; }
  /// <summary>
  /// Gets or sets the time zone the user. It should match the name of a time zone in the tz database.
  /// </summary>
  public string? TimeZone { get; init; }

  /// <summary>
  /// Gets or sets a link (URL) to the picture of the user.
  /// </summary>
  public string? Picture { get; init; }
  /// <summary>
  /// Gets or sets a link (URL) to the profile of the user.
  /// </summary>
  public string? Profile { get; init; }
  /// <summary>
  /// Gets or sets a link (URL) to the website of the user.
  /// </summary>
  public string? Website { get; init; }

  /// <summary>
  /// Gets or sets the custom attributes of the user.
  /// </summary>
  public Dictionary<string, string> CustomAttributes { get; init; } = new();

  /// <summary>
  /// Gets or sets the role identifiers of the user.
  /// </summary>
  public IEnumerable<AggregateId> Roles { get; init; } = Enumerable.Empty<AggregateId>();
}
