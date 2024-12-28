using Logitar.Identity.Core.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public sealed class SessionEvents : INotificationHandler<SessionCreated>,
  INotificationHandler<SessionDeleted>,
  INotificationHandler<SessionRenewed>,
  INotificationHandler<SessionSignedOut>,
  INotificationHandler<SessionUpdated>
{
  private readonly IdentityContext _context;
  private readonly ICustomAttributeService _customAttributes;
  private readonly IMediator _mediator;

  public SessionEvents(IdentityContext context, ICustomAttributeService customAttributes, IMediator mediator)
  {
    _context = context;
    _customAttributes = customAttributes;
    _mediator = mediator;
  }

  public async Task Handle(SessionCreated @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (session == null)
    {
      UserEntity user = await _context.Users
        .SingleOrDefaultAsync(x => x.StreamId == @event.UserId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The user entity 'StreamId={@event.UserId}' could not be found.");

      session = new(user, @event);
      user.Sessions.Add(session);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
    else
    {
      await _mediator.Publish(new EventNotHandled(@event, session), cancellationToken);
    }
  }

  public async Task Handle(SessionDeleted @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (session == null)
    {
      await _mediator.Publish(new EventNotHandled(@event, session), cancellationToken);
    }
    else
    {
      _context.Sessions.Remove(session);

      await _customAttributes.RemoveAsync(EntityType.Session, session.SessionId, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(SessionRenewed @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (session == null || session.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, session), cancellationToken);
    }
    else
    {
      session.Renew(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(SessionSignedOut @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (session == null || session.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, session), cancellationToken);
    }
    else
    {
      session.SignOut(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(SessionUpdated @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (session == null || session.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, session), cancellationToken);
    }
    else
    {
      session.Update(@event);

      await _customAttributes.UpdateAsync(EntityType.Session, session.SessionId, @event.CustomAttributes, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }
}
