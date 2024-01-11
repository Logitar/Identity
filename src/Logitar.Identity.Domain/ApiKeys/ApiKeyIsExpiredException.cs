using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.ApiKeys;

public class ApiKeyIsExpiredException : InvalidCredentialsException
{
  public new const string ErrorMessage = "The specified API key is expired.";

  public string ApiKey
  {
    get => (string)Data[nameof(ApiKey)]!;
    private set => Data[nameof(ApiKey)] = value;
  }

  public ApiKeyIsExpiredException(ApiKeyAggregate apiKey) : base(BuildMessage(apiKey))
  {
    ApiKey = apiKey.ToString();
  }

  private static string BuildMessage(ApiKeyAggregate apiKey) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ApiKey), apiKey.ToString())
    .Build();
}
