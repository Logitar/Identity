namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// The exception raised when an API key secret check fails.
/// </summary>
public class IncorrectApiKeySecretException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified secret did not match the API key.";

  /// <summary>
  /// Gets or sets the identifier of the API key.
  /// </summary>
  public string ApiKeyId
  {
    get => (string)Data[nameof(ApiKeyId)]!;
    private set => Data[nameof(ApiKeyId)] = value;
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
  public IncorrectApiKeySecretException(ApiKey apiKey, string attemptedSecret)
    : base(BuildMessage(apiKey, attemptedSecret))
  {
    ApiKeyId = apiKey.Id.Value;
    AttemptedSecret = attemptedSecret;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="apiKey">The API key.</param>
  /// <param name="attemptedSecret">The attempted secret.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(ApiKey apiKey, string attemptedSecret) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKeyId), apiKey.Id)
    .AddData(nameof(AttemptedSecret), attemptedSecret)
    .Build();
}
