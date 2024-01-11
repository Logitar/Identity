using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The exception raised when an user cannot be found.
/// </summary>
public class UserNotFoundException : InvalidCredentialsException
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public new const string ErrorMessage = "The specified user could not be found.";

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the user was searched.
  /// </summary>
  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  /// <summary>
  /// Gets or sets the unique name of the user who was searched.
  /// </summary>
  public string UniqueName
  {
    get => (string)Data[nameof(UniqueName)]!;
    private set => Data[nameof(UniqueName)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the user was searched.</param>
  /// <param name="uniqueName">The unique name of the user who was searched.</param>
  public UserNotFoundException(string? tenantId, string uniqueName) : base(BuildMessage(tenantId, uniqueName))
  {
    TenantId = tenantId;
    UniqueName = uniqueName;
  }

  private static string BuildMessage(string? tenantId, string uniqueName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TenantId), tenantId, "<null>")
    .AddData(nameof(UniqueName), uniqueName)
    .Build();
}
