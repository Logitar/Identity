using Logitar.Identity.Core.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public sealed class RoleEvents : INotificationHandler<RoleCreated>,
  INotificationHandler<RoleDeleted>,
  INotificationHandler<RoleUniqueNameChanged>,
  INotificationHandler<RoleUpdated>
{
  private readonly IdentityContext _context;
  private readonly ICustomAttributeService _customAttributes;
  private readonly IMediator _mediator;

  public RoleEvents(IdentityContext context, ICustomAttributeService customAttributes, IMediator mediator)
  {
    _context = context;
    _customAttributes = customAttributes;
    _mediator = mediator;
  }

  public async Task Handle(RoleCreated @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (role == null)
    {
      role = new(@event);

      _context.Roles.Add(role);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
    else
    {
      await _mediator.Publish(new EventNotHandled(@event, role), cancellationToken);
    }
  }

  public async Task Handle(RoleDeleted @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (role == null)
    {
      await _mediator.Publish(new EventNotHandled(@event, role), cancellationToken);
    }
    else
    {
      _context.Roles.Remove(role);

      await _customAttributes.RemoveAsync(EntityType.Role, role.RoleId, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(RoleUniqueNameChanged @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (role == null || role.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, role), cancellationToken);
    }
    else
    {
      role.SetUniqueName(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(RoleUpdated @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (role == null || role.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, role), cancellationToken);
    }
    else
    {
      role.Update(@event);

      await _customAttributes.UpdateAsync(EntityType.Role, role.RoleId, @event.CustomAttributes, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }
}
