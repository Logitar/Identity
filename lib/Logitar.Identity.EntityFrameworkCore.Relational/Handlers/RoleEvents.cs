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

  public RoleEvents(IdentityContext context)
  {
    _context = context;
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
    }
  }

  public async Task Handle(RoleDeleted @event, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (role != null)
    {
      _context.Roles.Remove(role);

      await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): delete CustomAttributes
    }
  }

  public async Task Handle(RoleUniqueNameChanged @event, CancellationToken cancellationToken)
  {
    RoleEntity role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The role entity 'StreamId={@event.StreamId}' could not be found.");

    role.SetUniqueName(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(RoleUpdated @event, CancellationToken cancellationToken)
  {
    RoleEntity role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The role entity 'StreamId={@event.StreamId}' could not be found.");

    role.Update(@event);

    await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): save CustomAttributes
  }
}
