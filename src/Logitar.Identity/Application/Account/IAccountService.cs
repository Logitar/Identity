using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Contracts.Sessions;

namespace Logitar.Identity.Application.Account;

public interface IAccountService
{
  Task RegisterAsync(RegisterPayload payload, CancellationToken cancellationToken = default);
  Task<Session> SignInAsync(SignInPayload payload, CancellationToken cancellationToken = default);
}
