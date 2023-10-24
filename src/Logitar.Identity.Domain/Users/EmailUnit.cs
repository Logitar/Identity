using FluentValidation;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Represents an email address.
/// </summary>
public record EmailUnit : ContactUnit, IEmail
{
  /// <summary>
  /// The maximum length of email addresses.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Gets the email address.
  /// </summary>
  public string Address { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="EmailUnit"/> class.
  /// </summary>
  /// <param name="address">The email address.</param>
  /// <param name="isVerified">A value indicating whether or not the email address is verified.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public EmailUnit(string address, bool isVerified = false, string? propertyName = null) : base(isVerified)
  {
    Address = address.Trim();

    new EmailValidator(propertyName).ValidateAndThrow(this);
  }
}
