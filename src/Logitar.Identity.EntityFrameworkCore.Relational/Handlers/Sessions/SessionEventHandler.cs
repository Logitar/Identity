using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionEventHandler : EventHandler, ISessionEventHandler
{
  protected const string EntityType = nameof(IdentityContext.Sessions);

  public SessionEventHandler(IdentityContext context) : base(context)
  {
  }

  public virtual async Task HandleAsync(SessionCreatedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await Context.Sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (session == null)
    {
      UserEntity user = await Context.Users
        .SingleOrDefaultAsync(x => x.AggregateId == @event.UserId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

      session = new(user, @event);
      user.Sessions.Add(session);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(SessionDeletedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await Context.Sessions
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (session != null)
    {
      Context.Sessions.Remove(session);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(SessionRenewedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity session = await Context.Sessions
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The session entity 'AggregateId={@event.AggregateId}' could not be found.");

    session.Renew(@event);

    await Context.SaveChangesAsync(cancellationToken);
  }

  public virtual async Task HandleAsync(SessionSignedOutEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity session = await Context.Sessions
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The session entity 'AggregateId={@event.AggregateId}' could not be found.");

    session.SignOut(@event);

    await Context.SaveChangesAsync(cancellationToken);
  }

  public virtual async Task HandleAsync(SessionUpdatedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity session = await Context.Sessions
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The session entity 'AggregateId={@event.AggregateId}' could not be found.");

    session.Update(@event);

    await SynchronizeCustomAttributesAsync(EntityType, session.SessionId, @event.CustomAttributes, cancellationToken);
    await Context.SaveChangesAsync(cancellationToken);
  }
}
