using FluentValidation.Results;
using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

/// <summary>
/// The exception raised when an API key secret check fails.
/// </summary>
public class IncorrectApiKeySecretException : InvalidCredentialsException, IValidationException
{
  private const string ErrorMessage = "The specified secret did not match the API key.";

  /// <summary>
  /// Gets or sets the attempted secret.
  /// </summary>
  public string AttemptedSecret
  {
    get => (string)Data[nameof(AttemptedSecret)]!;
    private set => Data[nameof(AttemptedSecret)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the API key.
  /// </summary>
  public ApiKeyId ApiKeyId
  {
    get => new((string)Data[nameof(ApiKeyId)]!);
    private set => Data[nameof(ApiKeyId)] = value.Value;
  }
  /// <summary>
  /// Gets or sets the name of the validated property.
  /// </summary>
  public string? PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  /// <summary>
  /// Gets the validation failure of the exception.
  /// </summary>
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, AttemptedSecret)
  {
    ErrorMessage = this.GetErrorCode(),
    CustomState = new
    {
      ApiKeyId = ApiKeyId.Value
    }
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="IncorrectApiKeySecretException"/> class.
  /// </summary>
  /// <param name="attemptedSecret">The attempted secret.</param>
  /// <param name="apiKey">The API key.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public IncorrectApiKeySecretException(string attemptedSecret, ApiKeyAggregate apiKey, string? propertyName = null)
    : base(BuildMessage(attemptedSecret, apiKey, propertyName))
  {
    AttemptedSecret = attemptedSecret;
    ApiKeyId = apiKey.Id;
    PropertyName = propertyName;
  }

  private static string BuildMessage(string attemptedSecret, ApiKeyAggregate apiKey, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(AttemptedSecret), attemptedSecret)
    .AddData(nameof(ApiKeyId), apiKey.Id.Value)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
