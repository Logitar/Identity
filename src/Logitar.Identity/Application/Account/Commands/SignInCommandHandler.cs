using Logitar.EventSourcing;
using Logitar.Identity.Application.Sessions;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Contracts.Sessions;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Application.Account.Commands;

internal class SignInCommandHandler : IRequestHandler<SignInCommand, Session>
{
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IUserSettings _userSettings;

  public SignInCommandHandler(ISessionQuerier sessionQuerier, ISessionRepository sessionRepository, IUserRepository userRepository, IUserSettings userSettings)
  {
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<Session> Handle(SignInCommand command, CancellationToken cancellationToken)
  {
    SignInPayload payload = command.Payload;
    TenantId? tenantId = TenantId.TryCreate(payload.TenantId); // TODO(fpion): shouldn't validate
    UniqueNameUnit uniqueName = new(_userSettings.UniqueNameSettings, payload.UniqueName); // TODO(fpion): shouldn't validate

    UserAggregate? user = await _userRepository.LoadAsync(tenantId, uniqueName, cancellationToken);
    if (user == null && _userSettings.RequireUniqueEmail)
    {
      EmailUnit email = new(payload.UniqueName); // TODO(fpion): shouldn't validate
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(tenantId, email, cancellationToken);
      if (users.Count() == 1)
      {
        user = users.Single();
      }
    }
    if (user == null)
    {
      throw new NotImplementedException(); // TODO(fpion): user could not be found
    }
    ActorId actorId = new(user.Id.Value);

    if (user.HasPassword)
    {
      if (payload.Password == null)
      {
        throw new NotImplementedException(); // TODO(fpion): ask for password and end flow
      }
      else
      {
        user.SignIn(payload.Password, actorId);

        if (_userSettings.RequireConfirmedAccount && !user.IsConfirmed)
        {
          throw new NotImplementedException(); // TODO(fpion): link with token if not confirmed?
        }

        // TODO(fpion): OTP-code if 2FAMode is set?

        SessionAggregate session = new(user, actorId); // TODO(fpion): same CreatedOn as user AuthenticatedOn?

        await _userRepository.SaveAsync(user, cancellationToken);
        await _sessionRepository.SaveAsync(session, cancellationToken);

        return await _sessionQuerier.ReadAsync(session, cancellationToken);
      }
    }
    else if (payload.Password == null)
    {
      throw new NotImplementedException(); // TODO(fpion): fall into passwordless flow and end flow (if unique emails and user has email)
    }
    else
    {
      throw new NotImplementedException(); // TODO(fpion): throw new NoPasswordRequiredException(user);
    }
  }
}
