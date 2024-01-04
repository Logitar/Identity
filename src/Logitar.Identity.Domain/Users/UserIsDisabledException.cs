using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public class UserIsDisabledException : InvalidCredentialsException
{
  public const string BaseMessage = "The specified user is disabled.";

  public string User
  {
    get => (string)Data[nameof(User)]!;
    private set => Data[nameof(User)] = value;
  }

  public UserIsDisabledException(UserAggregate user) : base(BuildMessage(user))
  {
    User = user.ToString();
  }

  private static string BuildMessage(UserAggregate user) => new ErrorMessageBuilder(BaseMessage)
    .AddData(nameof(User), user.ToString())
    .Build();
}
