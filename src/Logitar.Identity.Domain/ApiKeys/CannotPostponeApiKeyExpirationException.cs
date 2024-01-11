namespace Logitar.Identity.Domain.ApiKeys;

public class CannotPostponeApiKeyExpirationException : Exception
{
  public const string ErrorMessage = "The API key expiration cannot be postponed nor removed.";

  public string ApiKey
  {
    get => (string)Data[nameof(ApiKey)]!;
    private set => Data[nameof(ApiKey)] = value;
  }
  public DateTime? ExpiresOn
  {
    get => (DateTime?)Data[nameof(ExpiresOn)];
    private set => Data[nameof(ExpiresOn)] = value;
  }

  public CannotPostponeApiKeyExpirationException(ApiKeyAggregate apiKey, DateTime? expiresOn) : base(BuildMessage(apiKey, expiresOn))
  {
    ApiKey = apiKey.ToString();
    ExpiresOn = expiresOn;
  }

  private static string BuildMessage(ApiKeyAggregate apiKey, DateTime? expiresOn) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKey), apiKey.ToString())
    .AddData(nameof(ExpiresOn), expiresOn, "<null>")
    .Build();
}
