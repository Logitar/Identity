using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

namespace Logitar.Identity.Infrastructure;

internal class EventBus : IEventBus
{
  private readonly ISessionEventHandler _sessionEventHandler;
  private readonly IUserEventHandler _userEventHandler;

  public EventBus(ISessionEventHandler sessionEventHandler, IUserEventHandler userEventHandler)
  {
    _sessionEventHandler = sessionEventHandler;
    _userEventHandler = userEventHandler;
  }

  public async Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken)
  {
    switch (@event)
    {
      case SessionCreatedEvent sessionCreated:
        await _sessionEventHandler.HandleAsync(sessionCreated, cancellationToken);
        break;
      case UserCreatedEvent userCreated:
        await _userEventHandler.HandleAsync(userCreated, cancellationToken);
        break;
      case UserDeletedEvent userDeleted:
        await _userEventHandler.HandleAsync(userDeleted, cancellationToken);
        break;
      case UserDisabledEvent userDisabled:
        await _userEventHandler.HandleAsync(userDisabled, cancellationToken);
        break;
      case UserEmailChangedEvent userEmailChanged:
        await _userEventHandler.HandleAsync(userEmailChanged, cancellationToken);
        break;
      case UserEnabledEvent userEnabled:
        await _userEventHandler.HandleAsync(userEnabled, cancellationToken);
        break;
      case UserPasswordChangedEvent userPasswordChanged:
        await _userEventHandler.HandleAsync(userPasswordChanged, cancellationToken);
        break;
      case UserSignedInEvent userSignedIn:
        await _userEventHandler.HandleAsync(userSignedIn, cancellationToken);
        break;
      case UserUniqueNameChangedEvent userUniqueNameChanged:
        await _userEventHandler.HandleAsync(userUniqueNameChanged, cancellationToken);
        break;
      case UserUpdatedEvent userUpdated:
        await _userEventHandler.HandleAsync(userUpdated, cancellationToken);
        break;
      default:
        throw new NotSupportedException($"The event type '{@event.GetType().GetNamespaceQualifiedName()}' is not supported.");
    }
  }
}
