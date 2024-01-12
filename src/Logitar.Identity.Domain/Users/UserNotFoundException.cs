using Logitar.Identity.Domain.Settings;
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

  private static readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = null // NOTE(fpion): strict validation is not required when deserializing an unique name.
  };

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the user was searched.
  /// </summary>
  public TenantId? TenantId
  {
    get => TenantId.TryCreate((string?)Data[nameof(TenantId)]);
    private set => Data[nameof(TenantId)] = value?.Value;
  }
  /// <summary>
  /// Gets or sets the unique name of the user who was searched.
  /// </summary>
  public UniqueNameUnit UniqueName
  {
    get => new(_uniqueNameSettings, (string)Data[nameof(UniqueName)]!);
    private set => Data[nameof(UniqueName)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the user was searched.</param>
  /// <param name="uniqueName">The unique name of the user who was searched.</param>
  public UserNotFoundException(TenantId? tenantId, UniqueNameUnit uniqueName) : base(BuildMessage(tenantId, uniqueName))
  {
    TenantId = tenantId;
    UniqueName = uniqueName;
  }

  private static string BuildMessage(TenantId? tenantId, UniqueNameUnit uniqueName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TenantId), tenantId?.Value, "<null>")
    .AddData(nameof(UniqueName), uniqueName.Value)
    .Build();
}
