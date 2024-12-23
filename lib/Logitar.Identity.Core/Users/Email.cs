using FluentValidation;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents an email address.
/// </summary>
public record Email : Contact, IEmail
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
  /// Initializes a new instance of the <see cref="Email"/> class.
  /// </summary>
  /// <param name="address">The email address.</param>
  /// <param name="isVerified">A value indicating whether or not the contact is verified.</param>
  public Email(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
    new EmailValidator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a string representation of the email.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Address;
}
