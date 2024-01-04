using Logitar.Identity.Application.Account.Commands;
using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Contracts.Sessions;
using MediatR;

namespace Logitar.Identity.Application.Account;

internal class AccountService : IAccountService
{
  private readonly IMediator _mediator;

  public AccountService(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task RegisterAsync(RegisterPayload payload, CancellationToken cancellationToken)
  {
    await _mediator.Send(new RegisterCommand(payload), cancellationToken);
  }

  public async Task<Session> SignInAsync(SignInPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SignInCommand(payload), cancellationToken);
  }
}
