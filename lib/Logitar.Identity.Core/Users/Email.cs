using FluentValidation;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents an email address.
/// </summary>
public record Email : Contact
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
  /// <param name="isVerified">A value indicating whether the contact is verified or not.</param>
  public Email(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
    new Validator().ValidateAndThrow(this);
  }

  /// <summary>
  /// Returns a string representation of the email.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Address;

  /// <summary>
  /// Represents the validator for instances of <see cref="Email"/>.
  /// </summary>
  private class Validator : AbstractValidator<Email>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Validator"/> class.
    /// </summary>
    public Validator()
    {
      RuleFor(x => x.Address).NotEmpty().MaximumLength(MaximumLength).EmailAddress();
    }
  }
}
