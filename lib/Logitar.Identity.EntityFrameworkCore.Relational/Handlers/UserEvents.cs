using Logitar.Identity.Core.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public sealed class UserEvents : INotificationHandler<UserAddressChanged>,
  INotificationHandler<UserAuthenticated>,
  INotificationHandler<UserCreated>,
  INotificationHandler<UserDeleted>,
  INotificationHandler<UserDisabled>,
  INotificationHandler<UserEmailChanged>,
  INotificationHandler<UserEnabled>,
  INotificationHandler<UserIdentifierChanged>,
  INotificationHandler<UserIdentifierRemoved>,
  INotificationHandler<UserPasswordChanged>,
  INotificationHandler<UserPasswordRemoved>,
  INotificationHandler<UserPasswordReset>,
  INotificationHandler<UserPasswordUpdated>,
  INotificationHandler<UserPhoneChanged>,
  INotificationHandler<UserRoleAdded>,
  INotificationHandler<UserRoleRemoved>,
  INotificationHandler<UserSignedIn>,
  INotificationHandler<UserUniqueNameChanged>,
  INotificationHandler<UserUpdated>
{
  private readonly IdentityContext _context;
  private readonly ICustomAttributeService _customAttributes;

  public UserEvents(IdentityContext context, ICustomAttributeService customAttributes)
  {
    _context = context;
    _customAttributes = customAttributes;
  }

  public async Task Handle(UserAddressChanged @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetAddress(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserAuthenticated @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.Authenticate(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserCreated @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (user == null)
    {
      user = new(@event);

      _context.Users.Add(user);

      await SaveActorAsync(user, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(UserDeleted @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (user != null)
    {
      _context.Users.Remove(user);

      await DeleteActorAsync(user, cancellationToken);
      await _customAttributes.RemoveAsync(EntityType.User, user.UserId, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task Handle(UserDisabled @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.Disable(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserEmailChanged @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetEmail(@event);

    await SaveActorAsync(user, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserEnabled @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.Enable(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserIdentifierChanged @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .Include(x => x.Identifiers)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetCustomIdentifier(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserIdentifierRemoved @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .Include(x => x.Identifiers)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.RemoveCustomIdentifier(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserPasswordChanged @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetPassword(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserPasswordRemoved @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.RemovePassword(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserPasswordReset @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetPassword(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserPasswordUpdated @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetPassword(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserPhoneChanged @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetPhone(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserRoleAdded @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    RoleEntity role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.RoleId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The role entity 'StreamId={@event.RoleId}' could not be found.");

    user.AddRole(role, @event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserRoleRemoved @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.RemoveRole(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserSignedIn @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SignIn(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserUniqueNameChanged @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.SetUniqueName(@event);

    await SaveActorAsync(user, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(UserUpdated @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.StreamId}' could not be found.");

    user.Update(@event);

    await SaveActorAsync(user, cancellationToken);
    await _customAttributes.UpdateAsync(EntityType.User, user.UserId, @event.CustomAttributes, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
  }

  private async Task DeleteActorAsync(UserEntity user, CancellationToken cancellationToken)
  {
    await SaveActorAsync(user, isDeleted: true, cancellationToken);
  }
  private async Task SaveActorAsync(UserEntity user, CancellationToken cancellationToken)
  {
    await SaveActorAsync(user, isDeleted: false, cancellationToken);
  }
  private async Task SaveActorAsync(UserEntity user, bool isDeleted, CancellationToken cancellationToken)
  {
    ActorEntity? actor = await _context.Actors.SingleOrDefaultAsync(x => x.Id == user.StreamId, cancellationToken);
    if (actor == null)
    {
      actor = new()
      {
        Id = user.StreamId,
        Type = ActorType.User
      };
      _context.Actors.Add(actor);
    }

    actor.IsDeleted = isDeleted;

    actor.DisplayName = user.FullName ?? user.UniqueName;
    actor.EmailAddress = user.EmailAddress;
    actor.PictureUrl = user.Picture;
  }
}
