using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The command raised to sign-out the sessions of an user.
/// </summary>
/// <param name="Id">The identifier of the user.</param>
internal record SignOutUserCommand(Guid Id) : IRequest<IEnumerable<Session>>;
