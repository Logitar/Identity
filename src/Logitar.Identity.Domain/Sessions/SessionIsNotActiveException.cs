using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Sessions;

public class SessionIsNotActiveException : InvalidCredentialsException
{
  public new const string ErrorMessage = "The specified session is not active.";

  public string Session
  {
    get => (string)Data[nameof(Session)]!;
    private set => Data[nameof(Session)] = value;
  }

  public SessionIsNotActiveException(SessionAggregate session) : base(BuildMessage(session))
  {
    Session = session.ToString();
  }

  private static string BuildMessage(SessionAggregate session) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(Session), session.ToString())
    .Build();
}
