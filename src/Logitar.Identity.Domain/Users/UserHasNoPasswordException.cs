using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public class UserHasNoPasswordException : InvalidCredentialsException
{
  public new const string ErrorMessage = "The specified user has no password.";

  public string User
  {
    get => (string)Data[nameof(User)]!;
    private set => Data[nameof(User)] = value;
  }

  public UserHasNoPasswordException(UserAggregate user) : base(BuildMessage(user))
  {
    User = user.ToString();
  }

  private static string BuildMessage(UserAggregate user) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(User), user.ToString())
    .Build();
}
