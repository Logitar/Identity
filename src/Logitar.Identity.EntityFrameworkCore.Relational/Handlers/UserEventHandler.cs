using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public class UserEventHandler : IUserEventHandler
{
  private readonly IdentityContext _context;

  public UserEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (user == null)
    {
      user = new(@event);

      _context.Users.Add(user);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task HandleAsync(UserDeletedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (user != null)
    {
      _context.Users.Remove(user);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public async Task HandleAsync(UserDisabledEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.Disable(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task HandleAsync(UserEnabledEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.Enable(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.SetUniqueName(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
