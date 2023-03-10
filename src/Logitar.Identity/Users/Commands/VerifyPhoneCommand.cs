using MediatR;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The command raised to verify the phone number of an user.
/// </summary>
/// <param name="Id">The identifier of the user.</param>
internal record VerifyPhoneCommand(Guid Id) : IRequest<User>;
