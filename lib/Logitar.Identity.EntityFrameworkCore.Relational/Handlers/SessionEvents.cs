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

  public SessionEvents(IdentityContext context)
  {
    _context = context;
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
    }
  }

  public async Task Handle(SessionDeleted @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (session != null)
    {
      _context.Sessions.Remove(session);

      await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): delete CustomAttributes
    }
  }

  public async Task Handle(SessionRenewed @event, CancellationToken cancellationToken)
  {
    SessionEntity session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The session entity 'StreamId={@event.StreamId}' could not be found.");

    session.Renew(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(SessionSignedOut @event, CancellationToken cancellationToken)
  {
    SessionEntity session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The session entity 'StreamId={@event.StreamId}' could not be found.");

    session.SignOut(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(SessionUpdated @event, CancellationToken cancellationToken)
  {
    SessionEntity session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The session entity 'StreamId={@event.StreamId}' could not be found.");

    session.Update(@event);

    await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): save CustomAttributes
  }
}
