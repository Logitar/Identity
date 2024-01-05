using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserEventHandler : IUserEventHandler
{
  private readonly IdentityContext _context;

  public UserEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public virtual async Task HandleAsync(UserCreatedEvent @event, CancellationToken cancellationToken)
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

  public virtual async Task HandleAsync(UserDeletedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken);
    if (user != null)
    {
      _context.Users.Remove(user);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(UserSignedInEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

    user.SignIn(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public virtual async Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

    user.SetUniqueName(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public virtual async Task HandleAsync(UserUpdatedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

    user.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
