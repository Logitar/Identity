using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when an email address conflict occurs.
/// </summary>
public class EmailAddressAlreadyUsedException : Exception
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public const string ErrorMessage = "The specified email address is already used.";

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the conflict occurred.
  /// </summary>
  public TenantId? TenantId
  {
    get => TenantId.TryCreate((string?)Data[nameof(TenantId)]);
    private set => Data[nameof(TenantId)] = value?.Value;
  }
  /// <summary>
  /// Gets or sets the conflicting email address.
  /// </summary>
  public EmailUnit Email
  {
    get => new((string)Data[nameof(Email)]!);
    private set => Data[nameof(Email)] = value.Address;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="EmailAddressAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="email">The conflicting email.</param>
  public EmailAddressAlreadyUsedException(TenantId? tenantId, EmailUnit email)
    : base(BuildMessage(tenantId, email))
  {
    TenantId = tenantId;
    Email = email;
  }

  private static string BuildMessage(TenantId? tenantId, EmailUnit email) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TenantId), tenantId?.Value, "<null>")
    .AddData(nameof(Email), email.Address)
    .Build();
}
