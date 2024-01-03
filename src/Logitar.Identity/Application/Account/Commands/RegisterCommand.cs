using Logitar.Identity.Contracts.Account;
using MediatR;

namespace Logitar.Identity.Application.Account.Commands;

internal record RegisterCommand(RegisterPayload Payload) : IRequest<Unit>;
