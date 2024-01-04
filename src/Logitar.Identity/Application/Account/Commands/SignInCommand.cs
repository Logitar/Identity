using Logitar.Identity.Contracts.Account;
using Logitar.Identity.Contracts.Sessions;
using MediatR;

namespace Logitar.Identity.Application.Account.Commands;

internal record SignInCommand(SignInPayload Payload) : IRequest<Session>;
