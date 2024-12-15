using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// The exception raised when the maximum number of attempts has been reached for a One-Time Password (OTP).
/// </summary>
public class MaximumAttemptsReachedException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The maximum number of attempts has been reached for this One-Time Password (OTP).";

  /// <summary>
  /// Gets or sets the identifier of the One-Time Password (OTP).
  /// </summary>
  public OneTimePasswordId OneTimePasswordId
  {
    get => new((string)Data[nameof(OneTimePasswordId)]!);
    private set => Data[nameof(OneTimePasswordId)] = value.Value;
  }
  /// <summary>
  /// Gets or sets the number of attempts.
  /// </summary>
  public int AttemptCount
  {
    get => (int)Data[nameof(AttemptCount)]!;
    private set => Data[nameof(AttemptCount)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MaximumAttemptsReachedException"/> class.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP).</param>
  /// <param name="attemptCount">The number of attempts.</param>
  public MaximumAttemptsReachedException(OneTimePasswordAggregate oneTimePassword, int attemptCount)
    : base(BuildMessage(oneTimePassword, attemptCount))
  {
    AttemptCount = attemptCount;
    OneTimePasswordId = oneTimePassword.Id;
  }

  private static string BuildMessage(OneTimePasswordAggregate oneTimePassword, int attemptCount) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(OneTimePasswordId), oneTimePassword.Id.Value)
    .AddData(nameof(AttemptCount), attemptCount)
    .Build();
}
