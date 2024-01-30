using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserEventHandler : IUserEventHandler
{
  protected const string EntityType = nameof(IdentityContext.Users);

  protected virtual IdentityContext Context { get; }
  protected virtual ICustomAttributeService CustomAttributes { get; }

  public UserEventHandler(IdentityContext context, ICustomAttributeService customAttributes)
  {
    Context = context;
    CustomAttributes = customAttributes;
  }

  public virtual async Task HandleAsync(UserAddressChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SetAddress(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserAuthenticatedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.Authenticate(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user == null)
    {
      user = new(@event);

      Context.Users.Add(user);

      await SaveActorAsync(user, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserDeletedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      Context.Users.Remove(user);

      await DeleteActorAsync(user, cancellationToken);
      await CustomAttributes.RemoveAsync(EntityType, user.UserId, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserDisabledEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.Disable(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserEmailChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SetEmail(@event);

      await SaveActorAsync(user, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserEnabledEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.Enable(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserIdentifierChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SetCustomIdentifier(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserIdentifierRemovedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.RemoveCustomIdentifier(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserPasswordEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SetPassword(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserPhoneChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SetPhone(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserRoleAddedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      RoleEntity role = await Context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == @event.RoleId.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The role entity 'AggregateId={@event.AggregateId}' could not be found.");

      user.AddRole(role, @event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserRoleRemovedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.RemoveRole(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserSignedInEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SignIn(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.SetUniqueName(@event);

      await SaveActorAsync(user, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserUpdatedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (user != null)
    {
      user.Update(@event);

      await CustomAttributes.SynchronizeAsync(EntityType, user.UserId, @event.CustomAttributes, cancellationToken);
      await SaveActorAsync(user, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  protected virtual async Task<UserEntity> LoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await TryLoadAsync(aggregateId, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'AggregateId={aggregateId}' could not be found.");
  }
  protected virtual async Task<UserEntity?> TryLoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await Context.Users
      .Include(x => x.Identifiers)
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId.Value, cancellationToken);
  }

  protected virtual async Task DeleteActorAsync(UserEntity user, CancellationToken cancellationToken)
    => await SaveActorAsync(user, isDeleted: true, cancellationToken);
  protected virtual async Task SaveActorAsync(UserEntity user, CancellationToken cancellationToken)
    => await SaveActorAsync(user, isDeleted: false, cancellationToken);
  protected virtual async Task SaveActorAsync(UserEntity user, bool isDeleted, CancellationToken cancellationToken)
  {
    ActorEntity? actor = await Context.Actors
      .SingleOrDefaultAsync(x => x.Id == user.AggregateId, cancellationToken);

    if (actor == null)
    {
      actor = new()
      {
        Id = user.AggregateId,
        Type = ActorType.User
      };
      Context.Actors.Add(actor);
    }

    actor.IsDeleted = isDeleted;

    actor.DisplayName = user.FullName ?? user.UniqueName;
    actor.EmailAddress = user.EmailAddress;
    actor.PictureUrl = user.Picture;
  }
}
