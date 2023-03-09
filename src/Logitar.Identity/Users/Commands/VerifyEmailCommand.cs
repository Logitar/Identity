using MediatR;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The command raised to verify the email address of an user.
/// </summary>
/// <param name="Id">The identifier of the user.</param>
internal record VerifyEmailCommand(Guid Id) : IRequest<User>;
