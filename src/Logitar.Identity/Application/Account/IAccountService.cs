using Logitar.Identity.Contracts.Account;

namespace Logitar.Identity.Application.Account;

public interface IAccountService
{
  Task RegisterAsync(RegisterPayload payload, CancellationToken cancellationToken = default);
}
