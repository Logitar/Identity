namespace Logitar.Identity.Core.Passwords;

/// <summary>
/// The exception raised when a One-Time Password (OTP) has already been used.
/// </summary>
public class OneTimePasswordAlreadyUsedException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified One-Time Password (OTP) has already been used.";

  /// <summary>
  /// Gets or sets the identifier of the One-Time Password (OTP).
  /// </summary>
  public string OneTimePasswordId
  {
    get => (string)Data[nameof(OneTimePasswordId)]!;
    private set => Data[nameof(OneTimePasswordId)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP).</param>
  public OneTimePasswordAlreadyUsedException(OneTimePassword oneTimePassword)
    : base(BuildMessage(oneTimePassword))
  {
    OneTimePasswordId = oneTimePassword.Id.Value;
  }

  private static string BuildMessage(OneTimePassword oneTimePassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(oneTimePassword), oneTimePassword.Id)
    .Build();
}
