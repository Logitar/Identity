using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Application.Account.Validators;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Shared;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.Settings;
using MediatR;

namespace Logitar.Identity.Application.Account.Commands;

internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
{
  private readonly IPasswordManager _passwordManager;
  private readonly RegisterSettings _registerSettings;
  private readonly IUserManager _userManager;
  private readonly IUserSettings _userSettings;

  public RegisterCommandHandler(IConfiguration configuration, IPasswordManager passwordManager, IUserManager userManager, IUserSettings userSettings)
  {
    _registerSettings = configuration.GetSection("Register").Get<RegisterSettings>() ?? new();
    _passwordManager = passwordManager;
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

    if (_registerSettings.DisableUserOnRegistration)
    {
      user.Disable(actorId);
    }

    if (payload.Password != null)
    {
      Password password = _passwordManager.Create(payload.Password);
      user.SetPassword(password, actorId);
    }

    if (!string.IsNullOrWhiteSpace(payload.EmailAddress))
    {
      user.SetEmail(new EmailUnit(payload.EmailAddress), actorId);
    }

    user.FirstName = PersonNameUnit.TryCreate(payload.FirstName);
    user.LastName = PersonNameUnit.TryCreate(payload.LastName);
    user.Update(actorId);

    await _userManager.SaveAsync(user, cancellationToken);

    // TODO(fpion): send 2FA link/OTP-code?

    // TODO(fpion): issue session?

    return Unit.Value;
  }
}
