using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an existing user is modified.
/// </summary>
public record UserUpdated : DomainEvent, INotification
{
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public Change<PersonName>? FirstName { get; set; }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public Change<PersonName>? MiddleName { get; set; }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public Change<PersonName>? LastName { get; set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public Change<string>? FullName { get; set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public Change<PersonName>? Nickname { get; set; }

  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public Change<DateTime?>? Birthdate { get; set; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Change<Gender>? Gender { get; set; }
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public Change<Locale>? Locale { get; set; }
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public Change<TimeZone>? TimeZone { get; set; }

  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public Change<Url>? Picture { get; set; }
  /// <summary>
  /// Gets or sets the URL to the profile page of the user.
  /// </summary>
  public Change<Url>? Profile { get; set; }
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public Change<Url>? Website { get; set; }

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
