using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// The exception raised when an expired One-Time Password (OTP) is validated.
/// </summary>
public class OneTimePasswordIsExpiredException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified One-Time Password (OTP) is expired.";

  /// <summary>
  /// Gets the identifier of the expired One-Time Password (OTP).
  /// </summary>
  public OneTimePasswordId OneTimePasswordId
  {
    get => new((string)Data[nameof(OneTimePasswordId)]!);
    private set => Data[nameof(OneTimePasswordId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordIsExpiredException"/> class.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP) that is expired.</param>
  public OneTimePasswordIsExpiredException(OneTimePasswordAggregate oneTimePassword) : base(BuildMessage(oneTimePassword))
  {
    OneTimePasswordId = oneTimePassword.Id;
  }

  private static string BuildMessage(OneTimePasswordAggregate oneTimePassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(OneTimePasswordId), oneTimePassword.Id.Value)
    .Build();
}
