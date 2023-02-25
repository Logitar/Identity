namespace Logitar.Identity.Users;

/// <summary>
/// Represents the possible sort values for users.
/// </summary>
public enum UserSort
{
  /// <summary>
  /// The users will be sorted by their formatted postal address.
  /// </summary>
  AddressFormatted,

  /// <summary>
  /// The users will be sorted by the date and time they were disabled.
  /// </summary>
  DisabledOn,

  /// <summary>
  /// The users will be sorted by their email address.
  /// </summary>
  EmailAddress,

  /// <summary>
  /// The users will be sorted by their full name.
  /// </summary>
  FullName,

  /// <summary>
  /// The users will be sorted by their last name(s), then by their first name(s), and finally their middle name(s).
  /// </summary>
  LastFirstMiddleName,

  /// <summary>
  /// The users will be sorted by their latest password change date and time.
  /// </summary>
  PasswordChangedOn,

  /// <summary>
  /// The users will be sorted by their E.164 formatted phone number.
  /// </summary>
  PhoneE164Formatted,

  /// <summary>
  /// The users will be sorted by their latest successful sign-in date and time.
  /// </summary>
  SignedInOn,

  /// <summary>
  /// The users will be sorted by their latest update date and time, including creation.
  /// </summary>
  UpdatedOn,

  /// <summary>
  /// The users will be sorted by their unique name.
  /// </summary>
  Username
}
