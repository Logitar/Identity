using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public class SessionEventHandler : ISessionEventHandler
{
  private readonly IdentityContext _context;

  public SessionEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task HandleAsync(SessionCreatedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (session == null)
    {
      UserEntity user = await _context.Users
        .SingleOrDefaultAsync(x => x.AggregateId == @event.UserId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

      session = new(user, @event);
      user.Sessions.Add(session);

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
