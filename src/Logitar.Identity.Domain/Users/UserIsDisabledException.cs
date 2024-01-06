using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public class UserIsDisabledException : InvalidCredentialsException
{
  public new const string ErrorMessage = "The specified is disabled.";

  public string User
  {
    get => (string)Data[nameof(User)]!;
    private set => Data[nameof(User)] = value;
  }

  public UserIsDisabledException(UserAggregate user) : base(BuildMessage(user))
  {
    User = user.ToString();
  }

  private static string BuildMessage(UserAggregate user) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(User), user.ToString())
    .Build();
}
