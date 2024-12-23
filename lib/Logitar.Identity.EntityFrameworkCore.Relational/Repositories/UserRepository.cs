using Logitar.EventSourcing;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class UserRepository : Repository, IUserRepository
{
  private readonly IdentityContext _context;

  public UserRepository(IdentityContext context, IEventStore eventStore) : base(eventStore)
  {
    _context = context;
  }

  public async Task<User?> LoadAsync(UserId id, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, isDeleted: null, cancellationToken);
  }
  public async Task<User?> LoadAsync(UserId id, long? version, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version, isDeleted: null, cancellationToken);
  }
  public async Task<User?> LoadAsync(UserId id, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync(id, version: null, isDeleted, cancellationToken);
  }
  public async Task<User?> LoadAsync(UserId id, long? version, bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<User>(id.StreamId, version, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<User>> LoadAsync(CancellationToken cancellationToken)
  {
    return await LoadAsync(isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<User>> LoadAsync(bool? isDeleted, CancellationToken cancellationToken)
  {
    return await LoadAsync<User>(isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<User>> LoadAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken)
  {
    return await LoadAsync(ids, isDeleted: null, cancellationToken);
  }
  public async Task<IReadOnlyCollection<User>> LoadAsync(IEnumerable<UserId> ids, bool? isDeleted, CancellationToken cancellationToken)
  {
    IEnumerable<StreamId> streamIds = ids.Select(id => id.StreamId);
    return await LoadAsync<User>(streamIds, isDeleted, cancellationToken);
  }

  public async Task<IReadOnlyCollection<User>> LoadAsync(TenantId? tenantId, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;

    IEnumerable<StreamId> streamIds = (await _context.Users.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<User>(streamIds, cancellationToken);
  }

  public async Task<User?> LoadAsync(TenantId? tenantId, UniqueName uniqueName, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;
    string uniqueNameNormalized = IdentityDb.Helper.Normalize(uniqueName.Value);

    string? streamId = await _context.Users.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue && x.UniqueNameNormalized == uniqueNameNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);
    if (streamId == null)
    {
      return null;
    }

    return await LoadAsync<User>(new StreamId(streamId), cancellationToken);
  }
  public async Task<IReadOnlyCollection<User>> LoadAsync(TenantId? tenantId, Email email, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;
    string emailAddressNormalized = IdentityDb.Helper.Normalize(email.Address);

    IEnumerable<StreamId> streamIds = (await _context.Users.AsNoTracking()
      .Where(x => x.TenantId == tenantIdValue && x.EmailAddressNormalized == emailAddressNormalized)
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<User>(streamIds, cancellationToken);
  }
  public async Task<User?> LoadAsync(TenantId? tenantId, Identifier identifierKey, CustomIdentifier identifierValue, CancellationToken cancellationToken)
  {
    string? tenantIdValue = tenantId?.Value;

    string? streamId = await _context.UserIdentifiers.AsNoTracking()
      .Include(x => x.User)
      .Where(x => x.TenantId == tenantIdValue && x.Key == identifierKey.Value && x.Value == identifierValue.Value)
      .Select(x => x.User!.StreamId)
      .SingleOrDefaultAsync(cancellationToken);
    if (streamId == null)
    {
      return null;
    }

    return await LoadAsync<User>(new StreamId(streamId), cancellationToken);
  }

  public async Task<IReadOnlyCollection<User>> LoadAsync(Role role, CancellationToken cancellationToken)
  {
    string streamId = role.Id.Value;

    IEnumerable<StreamId> streamIds = (await _context.Users.AsNoTracking()
      .Include(x => x.Roles)
      .Where(x => x.Roles.Any(role => role.StreamId == streamId))
      .Select(x => x.StreamId)
      .ToArrayAsync(cancellationToken)).Select(value => new StreamId(value));

    return await LoadAsync<User>(streamIds, cancellationToken);
  }
  public async Task<User> LoadAsync(Session session, CancellationToken cancellationToken)
  {
    return await LoadAsync<User>(session.UserId.StreamId, cancellationToken)
      ?? throw new InvalidOperationException($"The user aggregate 'Id={session.UserId}' could not be loaded.");
  }

  public async Task SaveAsync(User user, CancellationToken cancellationToken)
  {
    await base.SaveAsync(user, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<User> users, CancellationToken cancellationToken)
  {
    await base.SaveAsync(users, cancellationToken);
  }
}
