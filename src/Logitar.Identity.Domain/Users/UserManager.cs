using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.Domain.Users;

public class UserManager : IUserManager
{
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;

  public UserManager(ISessionRepository sessionRepository, IUserRepository userRepository)
  {
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
  }

  public async Task SaveAsync(UserAggregate user, ActorId actorId, CancellationToken cancellationToken)
  {
    bool hasBeenDeleted = false;
    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in user.Changes)
    {
      if (change is UserCreatedEvent || change is UserUniqueNameChangedEvent)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is UserDeletedEvent)
      {
        hasBeenDeleted = true;
      }
    }

    // TODO(fpion): enforce email address unicity

    if (hasUniqueNameChanged)
    {
      UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, user.UniqueName);
      }
    }

    if (hasBeenDeleted)
    {
      IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(user, cancellationToken);
      foreach (SessionAggregate session in sessions)
      {
        session.Delete(actorId);
      }
      await _sessionRepository.SaveAsync(sessions, cancellationToken);
    }

    await _userRepository.SaveAsync(user, cancellationToken);
  }
}
