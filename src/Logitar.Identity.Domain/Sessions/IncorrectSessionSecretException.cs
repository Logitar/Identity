using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

public class IncorrectSessionSecretException : InvalidCredentialsException
{
  public new const string ErrorMessage = "The specified secret did not match the specified session.";

  public string Session
  {
    get => (string)Data[nameof(Session)]!;
    private set => Data[nameof(Session)] = value;
  }
  public string Secret
  {
    get => (string)Data[nameof(Secret)]!;
    private set => Data[nameof(Secret)] = value;
  }

  public IncorrectSessionSecretException(SessionAggregate session, byte[] secret) : base(BuildMessage(session, secret))
  {
    Session = session.ToString();
    Secret = Convert.ToBase64String(secret);
  }

  private static string BuildMessage(SessionAggregate session, byte[] secret) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(Session), session.ToString())
    .AddData(nameof(Secret), Convert.ToBase64String(secret))
    .Build();
}
