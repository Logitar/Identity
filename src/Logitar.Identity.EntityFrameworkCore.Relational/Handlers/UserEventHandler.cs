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

      await SaveActorAsync(user, cancellationToken);
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

      await DeleteActorAsync(user, cancellationToken);
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

  public async Task HandleAsync(UserEmailChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.SetEmail(@event);

    await _context.SaveChangesAsync(cancellationToken);

    await SaveActorAsync(user, cancellationToken);
  }

  public async Task HandleAsync(UserEnabledEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.Enable(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task HandleAsync(UserPasswordChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.SetPassword(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task HandleAsync(UserSignedInEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.SignIn(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task HandleAsync(UserUniqueNameChangedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.SetUniqueName(@event);

    await _context.SaveChangesAsync(cancellationToken);

    await SaveActorAsync(user, cancellationToken);
  }

  public async Task HandleAsync(UserUpdatedEvent @event, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
     .SingleOrDefaultAsync(x => x.AggregateId == @event.AggregateId.Value, cancellationToken)
     ?? throw new InvalidOperationException($"The user 'AggregateId={@event.AggregateId}' could not be found.");

    user.Update(@event);

    await _context.SaveChangesAsync(cancellationToken);

    await SaveActorAsync(user, cancellationToken);
  }

  private async Task DeleteActorAsync(UserEntity user, CancellationToken cancellationToken)
    => await SaveActorAsync(user, isDeleted: true, cancellationToken);
  private async Task SaveActorAsync(UserEntity user, CancellationToken cancellationToken)
    => await SaveActorAsync(user, isDeleted: false, cancellationToken);
  private async Task SaveActorAsync(UserEntity user, bool isDeleted, CancellationToken cancellationToken)
  {
    ActorEntity? actor = await _context.Actors
      .SingleOrDefaultAsync(x => x.Id == user.AggregateId, cancellationToken);

    if (actor == null)
    {
      actor = new(user, isDeleted);

      _context.Actors.Add(actor);
    }
    else
    {
      actor.Update(user, isDeleted);
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
