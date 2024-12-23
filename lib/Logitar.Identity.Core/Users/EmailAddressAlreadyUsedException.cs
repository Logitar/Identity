namespace Logitar.Identity.Core.Users;

/// <summary>
/// The exception raised when an email address conflict occurs.
/// </summary>
public class EmailAddressAlreadyUsedException : IdentityException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified email address is already used.";

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the conflict occurred.
  /// </summary>
  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the user which already owns the email.
  /// </summary>
  public string ConflictId
  {
    get => (string)Data[nameof(ConflictId)]!;
    private set => Data[nameof(ConflictId)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the user which caused the conflict.
  /// </summary>
  public string UserId
  {
    get => (string)Data[nameof(UserId)]!;
    private set => Data[nameof(UserId)] = value;
  }
  /// <summary>
  /// Gets or sets the conflicting email address.
  /// </summary>
  public string EmailAddress
  {
    get => (string)Data[nameof(EmailAddress)]!;
    private set => Data[nameof(EmailAddress)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="EmailAddressAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="user">The user which caused the conflict.</param>
  /// <param name="conflict">The user which already owns the email.</param>
  /// <exception cref="ArgumentException">The user email is null.</exception>
  public EmailAddressAlreadyUsedException(User user, User conflict) : base(BuildMessage(user, conflict))
  {
    TenantId = user.TenantId?.Value;
    ConflictId = conflict.EntityId.Value;
    UserId = user.EntityId.Value;
    EmailAddress = user.Email?.Address ?? throw new ArgumentException($"The {nameof(user.Email)} is required.", nameof(user));
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="user">The user which caused the conflict.</param>
  /// <param name="conflict">The user which already owns the email.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(User user, User conflict) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TenantId), user.TenantId, "<null>")
    .AddData(nameof(ConflictId), conflict.EntityId)
    .AddData(nameof(UserId), user.EntityId)
    .AddData(nameof(EmailAddress), user.Email)
    .Build();
}
