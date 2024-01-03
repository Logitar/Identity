using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Application.Account.Validators;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Application.Account.Commands;

internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
{
  private readonly IUserManager _userManager;
  private readonly IUserSettings _userSettings;

  public RegisterCommandHandler(IUserManager userManager, IUserSettings userSettings)
  {
    _userManager = userManager;
    _userSettings = userSettings;
  }

  public async Task<Unit> Handle(RegisterCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = _userSettings.UniqueNameSettings;

    RegisterPayload payload = command.Payload;
    new RegisterPayloadValidator(uniqueNameSettings).ValidateAndThrow(payload);

    UniqueNameUnit uniqueName = new(uniqueNameSettings, payload.UniqueName);
    UserId id = UserId.NewId();
    ActorId actorId = new(id.Value);
    UserAggregate user = new(uniqueName, tenantId: null, actorId, id);

    await _userManager.SaveAsync(user, cancellationToken);

    return Unit.Value;
  }
}
