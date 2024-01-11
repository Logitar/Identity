using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

public class IncorrectApiKeySecretException : InvalidCredentialsException
{
  public new const string ErrorMessage = "The specified secret did not match the specified API key.";

  public string ApiKey
  {
    get => (string)Data[nameof(ApiKey)]!;
    private set => Data[nameof(ApiKey)] = value;
  }
  public string Secret
  {
    get => (string)Data[nameof(Secret)]!;
    private set => Data[nameof(Secret)] = value;
  }

  public IncorrectApiKeySecretException(ApiKeyAggregate apiKey, byte[] secret) : base(BuildMessage(apiKey, secret))
  {
    ApiKey = apiKey.ToString();
    Secret = Convert.ToBase64String(secret);
  }

  private static string BuildMessage(ApiKeyAggregate apiKey, byte[] secret) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKey), apiKey.ToString())
    .AddData(nameof(Secret), Convert.ToBase64String(secret))
    .Build();
}
