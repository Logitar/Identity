using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.Passwords.Events;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.ApiKeys;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Passwords;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;
using Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;
using MediatR;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class EventBus : IEventBus
{
  public EventBus(IPublisher publisher, IApiKeyEventHandler apiKeyEventHandler, IOneTimePasswordEventHandler oneTimePasswordEventHandler,
    IRoleEventHandler roleEventHandler, ISessionEventHandler sessionEventHandler, IUserEventHandler userEventHandler)
  {
    Publisher = publisher;
    ApiKeyEventHandler = apiKeyEventHandler;
    OneTimePasswordEventHandler = oneTimePasswordEventHandler;
    RoleEventHandler = roleEventHandler;
    SessionEventHandler = sessionEventHandler;
    UserEventHandler = userEventHandler;
  }

  protected IPublisher Publisher { get; }
  protected IApiKeyEventHandler ApiKeyEventHandler { get; }
  protected IOneTimePasswordEventHandler OneTimePasswordEventHandler { get; }
  protected IRoleEventHandler RoleEventHandler { get; }
  protected ISessionEventHandler SessionEventHandler { get; }
  protected IUserEventHandler UserEventHandler { get; }

  public virtual async Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken)
  {
    switch (@event)
    {
      #region ApiKeys
      case ApiKeyAuthenticatedEvent apiKeyAuthenticated:
        await ApiKeyEventHandler.HandleAsync(apiKeyAuthenticated, cancellationToken);
        break;
      case ApiKeyCreatedEvent apiKeyCreated:
        await ApiKeyEventHandler.HandleAsync(apiKeyCreated, cancellationToken);
        break;
      case ApiKeyDeletedEvent apiKeyDeleted:
        await ApiKeyEventHandler.HandleAsync(apiKeyDeleted, cancellationToken);
        break;
      case ApiKeyRoleAddedEvent apiKeyRoleAdded:
        await ApiKeyEventHandler.HandleAsync(apiKeyRoleAdded, cancellationToken);
        break;
      case ApiKeyRoleRemovedEvent apiKeyRoleRemoved:
        await ApiKeyEventHandler.HandleAsync(apiKeyRoleRemoved, cancellationToken);
        break;
      case ApiKeyUpdatedEvent apiKeyUpdated:
        await ApiKeyEventHandler.HandleAsync(apiKeyUpdated, cancellationToken);
        break;
      #endregion
      #region OneTimePasswords
      case OneTimePasswordCreatedEvent oneTimePasswordCreated:
        await OneTimePasswordEventHandler.HandleAsync(oneTimePasswordCreated, cancellationToken);
        break;
      case OneTimePasswordDeletedEvent oneTimePasswordDeleted:
        await OneTimePasswordEventHandler.HandleAsync(oneTimePasswordDeleted, cancellationToken);
        break;
      case OneTimePasswordUpdatedEvent oneTimePasswordUpdated:
        await OneTimePasswordEventHandler.HandleAsync(oneTimePasswordUpdated, cancellationToken);
        break;
      case OneTimePasswordValidationFailedEvent oneTimePasswordValidationFailed:
        await OneTimePasswordEventHandler.HandleAsync(oneTimePasswordValidationFailed, cancellationToken);
        break;
      case OneTimePasswordValidationSucceededEvent oneTimePasswordValidationSucceeded:
        await OneTimePasswordEventHandler.HandleAsync(oneTimePasswordValidationSucceeded, cancellationToken);
        break;
      #endregion
      #region Roles
      case RoleCreatedEvent roleCreated:
        await RoleEventHandler.HandleAsync(roleCreated, cancellationToken);
        break;
      case RoleDeletedEvent roleDeleted:
        await RoleEventHandler.HandleAsync(roleDeleted, cancellationToken);
        break;
      case RoleUniqueNameChangedEvent roleUniqueNameChanged:
        await RoleEventHandler.HandleAsync(roleUniqueNameChanged, cancellationToken);
        break;
      case RoleUpdatedEvent roleUpdated:
        await RoleEventHandler.HandleAsync(roleUpdated, cancellationToken);
        break;
      #endregion
      #region Sessions
      case SessionCreatedEvent sessionCreated:
        await SessionEventHandler.HandleAsync(sessionCreated, cancellationToken);
        break;
      case SessionDeletedEvent sessionDeleted:
        await SessionEventHandler.HandleAsync(sessionDeleted, cancellationToken);
        break;
      case SessionRenewedEvent sessionRenewed:
        await SessionEventHandler.HandleAsync(sessionRenewed, cancellationToken);
        break;
      case SessionSignedOutEvent sessionSignedOut:
        await SessionEventHandler.HandleAsync(sessionSignedOut, cancellationToken);
        break;
      case SessionUpdatedEvent sessionUpdated:
        await SessionEventHandler.HandleAsync(sessionUpdated, cancellationToken);
        break;
      #endregion
      #region Users
      case UserAddressChangedEvent userAddressChanged:
        await UserEventHandler.HandleAsync(userAddressChanged, cancellationToken);
        break;
      case UserAuthenticatedEvent userAuthenticated:
        await UserEventHandler.HandleAsync(userAuthenticated, cancellationToken);
        break;
      case UserCreatedEvent userCreated:
        await UserEventHandler.HandleAsync(userCreated, cancellationToken);
        break;
      case UserDeletedEvent userDeleted:
        await UserEventHandler.HandleAsync(userDeleted, cancellationToken);
        break;
      case UserDisabledEvent userDisabled:
        await UserEventHandler.HandleAsync(userDisabled, cancellationToken);
        break;
      case UserEmailChangedEvent userEmailChanged:
        await UserEventHandler.HandleAsync(userEmailChanged, cancellationToken);
        break;
      case UserEnabledEvent userEnabled:
        await UserEventHandler.HandleAsync(userEnabled, cancellationToken);
        break;
      case UserIdentifierChangedEvent userIdentifierChanged:
        await UserEventHandler.HandleAsync(userIdentifierChanged, cancellationToken);
        break;
      case UserIdentifierRemovedEvent userIdentifierRemoved:
        await UserEventHandler.HandleAsync(userIdentifierRemoved, cancellationToken);
        break;
      case UserPasswordChangedEvent userPasswordChanged:
        await UserEventHandler.HandleAsync(userPasswordChanged, cancellationToken);
        break;
      case UserPasswordResetEvent userPasswordReset:
        await UserEventHandler.HandleAsync(userPasswordReset, cancellationToken);
        break;
      case UserPasswordUpdatedEvent userPasswordUpdated:
        await UserEventHandler.HandleAsync(userPasswordUpdated, cancellationToken);
        break;
      case UserPhoneChangedEvent userPhoneChanged:
        await UserEventHandler.HandleAsync(userPhoneChanged, cancellationToken);
        break;
      case UserRoleAddedEvent userRoleAdded:
        await UserEventHandler.HandleAsync(userRoleAdded, cancellationToken);
        break;
      case UserRoleRemovedEvent userRoleRemoved:
        await UserEventHandler.HandleAsync(userRoleRemoved, cancellationToken);
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
        #endregion
    }

    await Publisher.Publish(@event, cancellationToken);
  }
}
