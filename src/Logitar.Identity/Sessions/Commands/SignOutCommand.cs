using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The command raised to sign-out an user session.
/// </summary>
/// <param name="Id">The identifier of the session.</param>
internal record SignOutCommand(Guid Id) : IRequest<Session>;
