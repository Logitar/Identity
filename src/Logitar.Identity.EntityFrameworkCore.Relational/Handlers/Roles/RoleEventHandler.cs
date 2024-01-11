using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleEventHandler : EventHandler, IRoleEventHandler
{
  public RoleEventHandler(IdentityContext context) : base(context)
  {
  }

  public virtual async Task HandleAsync(RoleCreatedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await Context.Roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (role == null)
    {
      role = new(@event);

      Context.Roles.Add(role);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleDeletedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await Context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (role != null)
    {
      Context.Roles.Remove(role);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(RoleUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity role = await Context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

    role.SetUniqueName(@event);

    await Context.SaveChangesAsync(cancellationToken);
  }

  public virtual async Task HandleAsync(RoleUpdatedEvent @event, CancellationToken cancellationToken)
  {
    RoleEntity role = await Context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

    role.Update(@event);

    await Context.SaveChangesAsync(cancellationToken);
  }
}
