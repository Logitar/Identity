using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

/// <summary>
/// The exception raised when an API key secret check fails.
/// </summary>
public class IncorrectApiKeySecretException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified secret did not match the API key.";

  /// <summary>
  /// Gets or sets the identifier of the API key.
  /// </summary>
  public ApiKeyId ApiKeyId
  {
    get => new((string)Data[nameof(ApiKeyId)]!);
    private set => Data[nameof(ApiKeyId)] = value.Value;
  }
  /// <summary>
  /// Gets or sets the attempted secret.
  /// </summary>
  public string AttemptedSecret
  {
    get => (string)Data[nameof(AttemptedSecret)]!;
    private set => Data[nameof(AttemptedSecret)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectApiKeySecretException"/> class.
  /// </summary>
  /// <param name="apiKey">The API key.</param>
  /// <param name="attemptedSecret">The attempted secret.</param>
  public IncorrectApiKeySecretException(ApiKeyAggregate apiKey, string attemptedSecret)
    : base(BuildMessage(apiKey, attemptedSecret))
  {
    ApiKeyId = apiKey.Id;
    AttemptedSecret = attemptedSecret;
  }

  private static string BuildMessage(ApiKeyAggregate apiKey, string attemptedSecret) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKeyId), apiKey.Id.Value)
    .AddData(nameof(AttemptedSecret), attemptedSecret)
    .Build();
}
