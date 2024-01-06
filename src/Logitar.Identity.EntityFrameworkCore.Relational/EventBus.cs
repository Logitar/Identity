using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;
using MediatR;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class EventBus : IEventBus
{
  public EventBus(IPublisher publisher, ISessionEventHandler sessionEventHandler, IUserEventHandler userEventHandler)
  {
    Publisher = publisher;
    SessionEventHandler = sessionEventHandler;
    UserEventHandler = userEventHandler;
  }

  protected IPublisher Publisher { get; }
  protected ISessionEventHandler SessionEventHandler { get; }
  protected IUserEventHandler UserEventHandler { get; }

  public virtual async Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken)
  {
    await Publisher.Publish(@event, cancellationToken);

    switch (@event)
    {
      case SessionCreatedEvent sessionCreated:
        await SessionEventHandler.HandleAsync(sessionCreated, cancellationToken);
        break;
      case SessionDeletedEvent sessionDeleted:
        await SessionEventHandler.HandleAsync(sessionDeleted, cancellationToken);
        break;
      case UserCreatedEvent userCreated:
        await UserEventHandler.HandleAsync(userCreated, cancellationToken);
        break;
      case UserDeletedEvent userDeleted:
        await UserEventHandler.HandleAsync(userDeleted, cancellationToken);
        break;
      case UserPasswordChangedEvent userPasswordChanged:
        await UserEventHandler.HandleAsync(userPasswordChanged, cancellationToken);
        break;
      case UserSignedInEvent userSignedIn:
        await UserEventHandler.HandleAsync(userSignedIn, cancellationToken);
        break;
      case UserUniqueNameChangedEvent userUniqueNameChanged:
        await UserEventHandler.HandleAsync(userUniqueNameChanged, cancellationToken);
        break;
      case UserUpdatedEvent userUpdated:
        await UserEventHandler.HandleAsync(userUpdated, cancellationToken);
        break;
    }
  }
}
