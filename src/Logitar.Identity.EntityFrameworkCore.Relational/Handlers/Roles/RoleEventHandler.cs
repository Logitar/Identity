using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleEventHandler : EventHandler, IRoleEventHandler
{
  protected const string EntityType = nameof(IdentityContext.Roles);

  public RoleEventHandler(IdentityContext context) : base(context)
  {
  }

  public virtual async Task HandleAsync(RoleCreatedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (role == null)
    {
      role = new(@event);

      Context.Roles.Add(role);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleDeletedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (role != null)
    {
      Context.Roles.Remove(role);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity role = await LoadAsync(@event.AggregateId, cancellationToken);

    role.SetUniqueName(@event);

    await Context.SaveChangesAsync(cancellationToken);
  }

  public virtual async Task HandleAsync(RoleUpdatedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity role = await LoadAsync(@event.AggregateId, cancellationToken);

    role.Update(@event);

    await SynchronizeCustomAttributesAsync(EntityType, role.RoleId, @event.CustomAttributes, cancellationToken);
    await Context.SaveChangesAsync(cancellationToken);
  }

  protected virtual async Task<RoleEntity> LoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await TryLoadAsync(aggregateId, cancellationToken)
      ?? throw new InvalidOperationException($"The role entity 'AggregateId={aggregateId}' could not be found.");
  }
  protected virtual async Task<RoleEntity?> TryLoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await Context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId.Value, cancellationToken);
  }
}
