namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// The exception raised when an expired API key is authenticated.
/// </summary>
public class ApiKeyIsExpiredException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified API key is expired.";

  /// <summary>
  /// Gets the identifier of the expired API key.
  /// </summary>
  public ApiKeyId ApiKeyId // TODO(fpion): do we really want this?
  {
    get => new((string)Data[nameof(ApiKeyId)]!);
    private set => Data[nameof(ApiKeyId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyIsExpiredException"/> class.
  /// </summary>
  /// <param name="apiKey">The API key that is expired.</param>
  public ApiKeyIsExpiredException(ApiKey apiKey) : base(BuildMessage(apiKey))
  {
    ApiKeyId = apiKey.Id;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="apiKey">The API key that is expired.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(ApiKey apiKey) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKeyId), apiKey.Id.Value)
    .Build();
}
