﻿using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Passwords;

/// <summary>
/// Defines methods to retrieve and store One-Time Passwords (OTP) to an event store.
/// </summary>
public interface IOneTimePasswordRepository
{
  /// <summary>
  /// Loads a One-Time Password (OTP) by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The One-Time Password (OTP), if found.</returns>
  Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a One-Time Password (OTP) by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the One-Time Password (OTP).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The One-Time Password (OTP), if found.</returns>
  Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, long? version, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a One-Time Password (OTP) by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the One-Time Password (OTP) if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The One-Time Password (OTP), if found.</returns>
  Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, bool includeDeleted, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a One-Time Password (OTP) by the specified unique identifier.
  /// </summary>
  /// <param name="id">The unique identifier.</param>
  /// <param name="version">The version at which to load the One-Time Password (OTP).</param>
  /// <param name="includeDeleted">A value indicating whether or not to load the One-Time Password (OTP) if it is deleted.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The One-Time Password (OTP), if found.</returns>
  Task<OneTimePasswordAggregate?> LoadAsync(OneTimePasswordId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the One-Time Password (OTP)s from the event store.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found One-Time Passwords (OTP).</returns>
  Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the One-Time Password (OTP)s from the event store.
  /// </summary>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted One-Time Passwords (OTP).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found One-Time Passwords (OTP).</returns>
  Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the One-Time Password (OTP)s by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found One-Time Passwords (OTP).</returns>
  Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(IEnumerable<OneTimePasswordId> ids, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the One-Time Password (OTP)s by the specified list of unique identifiers.
  /// </summary>
  /// <param name="ids">The unique identifiers.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted One-Time Passwords (OTP).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found One-Time Passwords (OTP).</returns>
  Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(IEnumerable<OneTimePasswordId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Loads the One-Time Password (OTP)s in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found One-Time Passwords (OTP).</returns>
  Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads the One-Time Password (OTP)s in the specified tenant.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant.</param>
  /// <param name="includeDeleted">A value indicating whether or not to load deleted One-Time Passwords (OTP).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The found One-Time Passwords (OTP).</returns>
  Task<IEnumerable<OneTimePasswordAggregate>> LoadAsync(TenantId? tenantId, bool includeDeleted, CancellationToken cancellationToken = default);

  /// <summary>
  /// Saves the specified One-Time Password (OTP) into the store.
  /// </summary>
  /// <param name="oneTimePassword">The One-Time Password (OTP) to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(OneTimePasswordAggregate oneTimePassword, CancellationToken cancellationToken = default);
  /// <summary>
  /// Saves the specified One-Time Passwords (OTP) into the store.
  /// </summary>
  /// <param name="oneTimePasswords">The One-Time Password (OTP)s to save.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<OneTimePasswordAggregate> oneTimePasswords, CancellationToken cancellationToken = default);
}
