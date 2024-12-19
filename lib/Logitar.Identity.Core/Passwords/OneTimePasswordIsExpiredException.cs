namespace Logitar.Identity.Core.Passwords;

/// <summary>
/// The exception raised when an expired One-Time Password (OTP) is validated.
/// </summary>
public class OneTimePasswordIsExpiredException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified One-Time Password (OTP) is expired.";

  /// <summary>
  /// Gets the identifier of the expired One-Time Password (OTP).
  /// </summary>
  public string OneTimePasswordId
  {
    get => (string)Data[nameof(OneTimePasswordId)]!;
    private set => Data[nameof(OneTimePasswordId)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordIsExpiredException"/> class.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP) that is expired.</param>
  public OneTimePasswordIsExpiredException(OneTimePassword oneTimePassword) : base(BuildMessage(oneTimePassword))
  {
    OneTimePasswordId = oneTimePassword.Id.Value;
  }

  private static string BuildMessage(OneTimePassword oneTimePassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(OneTimePasswordId), oneTimePassword.Id)
    .Build();
}
