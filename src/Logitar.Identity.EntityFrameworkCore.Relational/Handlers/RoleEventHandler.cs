using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public class RoleEventHandler : IRoleEventHandler
{
  protected const string EntityType = nameof(IdentityContext.Roles);

  protected virtual IdentityContext Context { get; }
  protected virtual ICustomAttributeService CustomAttributes { get; }

  public RoleEventHandler(IdentityContext context, ICustomAttributeService customAttributes)
  {
    Context = context;
    CustomAttributes = customAttributes;
  }

  public virtual async Task HandleAsync(RoleCreatedEvent @event, CancellationToken cancellationToken)
  {
    var role = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (role == null)
    {
      role = new(@event);

      Context.Roles.Add(role);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleDeletedEvent @event, CancellationToken cancellationToken)
  {
    var role = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (role != null)
    {
      Context.Roles.Remove(role);

      await CustomAttributes.RemoveAsync(EntityType, role.RoleId, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    var role = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (role != null)
    {
      role.SetUniqueName(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleUpdatedEvent @event, CancellationToken cancellationToken)
  {
    var role = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (role != null)
    {
      role.Update(@event);

      await CustomAttributes.SynchronizeAsync(EntityType, role.RoleId, @event.CustomAttributes, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
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
