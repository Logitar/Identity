﻿using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The exception raised when an unique name conflict occurs.
/// </summary>
public class UniqueNameAlreadyUsedException : Exception
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public const string ErrorMessage = "The specified unique name is already used.";

  private static readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = null // NOTE(fpion): strict validation is not required when deserializing an unique name.
  };

  /// <summary>
  /// Gets or sets the name of the type of the object that caused the conflict.
  /// </summary>
  public string TypeName
  {
    get => (string)Data[nameof(TypeName)]!;
    private set => Data[nameof(TypeName)] = value;
  }
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the conflict occurred.
  /// </summary>
  public TenantId? TenantId
  {
    get => TenantId.TryCreate((string?)Data[nameof(TenantId)]);
    private set => Data[nameof(TenantId)] = value?.Value;
  }
  /// <summary>
  /// Gets or sets the conflicting unique name.
  /// </summary>
  public UniqueNameUnit UniqueName
  {
    get => new(_uniqueNameSettings, (string)Data[nameof(UniqueName)]!);
    private set => Data[nameof(UniqueName)] = value.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="type">The type of the object that caused the conflict.</param>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="uniqueName">The conflicting unique name.</param>
  public UniqueNameAlreadyUsedException(Type type, TenantId? tenantId, UniqueNameUnit uniqueName)
    : base(BuildMessage(type, tenantId, uniqueName))
  {
    TypeName = type.GetNamespaceQualifiedName();
    TenantId = tenantId;
    UniqueName = uniqueName;
  }

  private static string BuildMessage(Type type, TenantId? tenantId, UniqueNameUnit uniqueName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TypeName), type.GetNamespaceQualifiedName())
    .AddData(nameof(TenantId), tenantId?.Value, "<null>")
    .AddData(nameof(UniqueName), uniqueName.Value)
    .Build();
}


/// <summary>
/// The typed exception raised when an unique name conflict occurs.
/// </summary>
public class UniqueNameAlreadyUsedException<T> : UniqueNameAlreadyUsedException
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the conflict occurred.</param>
  /// <param name="uniqueName">The conflicting unique name.</param>
  public UniqueNameAlreadyUsedException(TenantId? tenantId, UniqueNameUnit uniqueName)
    : base(typeof(T), tenantId, uniqueName)
  {
  }
}
