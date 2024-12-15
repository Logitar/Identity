using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// The exception raised when a One-Time Password (OTP) has already been used.
/// </summary>
public class OneTimePasswordAlreadyUsedException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified One-Time Password (OTP) has already been used.";

  /// <summary>
  /// Gets or sets the identifier of the One-Time Password (OTP).
  /// </summary>
  public OneTimePasswordId OneTimePasswordId
  {
    get => new((string)Data[nameof(OneTimePasswordId)]!);
    private set => Data[nameof(OneTimePasswordId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="OneTimePasswordAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP).</param>
  public OneTimePasswordAlreadyUsedException(OneTimePasswordAggregate oneTimePassword)
    : base(BuildMessage(oneTimePassword))
  {
    OneTimePasswordId = oneTimePassword.Id;
  }

  private static string BuildMessage(OneTimePasswordAggregate oneTimePassword) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(oneTimePassword), oneTimePassword.Id)
    .Build();
}
