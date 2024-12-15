using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when a disabled user is authenticated or signs-in.
/// </summary>
public class UserIsDisabledException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified user is disabled.";

  /// <summary>
  /// Gets the identifier of the disabled user.
  /// </summary>
  public UserId UserId
  {
    get => new((string)Data[nameof(UserId)]!);
    private set => Data[nameof(UserId)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserIsDisabledException"/> class.
  /// </summary>
  /// <param name="user">The user that is disabled.</param>
  public UserIsDisabledException(UserAggregate user) : base(BuildMessage(user))
  {
    UserId = user.Id;
  }

  private static string BuildMessage(UserAggregate user) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(UserId), user.Id.Value)
    .Build();
}
