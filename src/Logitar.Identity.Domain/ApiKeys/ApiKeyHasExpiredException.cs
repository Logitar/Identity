using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

/// <summary>
/// The exception raised when an expired API key is authenticated.
/// </summary>
public class ApiKeyHasExpiredException : Exception
{
  private const string ErrorMessage = "The specified API key has expired.";

  /// <summary>
  /// Gets the identifier of the expired API key.
  /// </summary>
  public ApiKeyId ApiKeyId
  {
    get => new((string)Data[nameof(ApiKeyId)]!);
    private set => Data[nameof(ApiKeyId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyHasExpiredException"/> class.
  /// </summary>
  /// <param name="apiKey">The API key that has expired.</param>
  public ApiKeyHasExpiredException(ApiKeyAggregate apiKey) : base(BuildMessage(apiKey))
  {
    ApiKeyId = apiKey.Id;
  }

  private static string BuildMessage(ApiKeyAggregate apiKey) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKeyId), apiKey.Id.Value)
    .Build();
}
