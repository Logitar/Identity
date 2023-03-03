using MediatR;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The command raised to change an user's password.
/// </summary>
/// <param name="Id">The identifier of the user.</param>
/// <param name="Input">The password change input data.</param>
internal record ChangePasswordCommand(Guid Id, ChangePasswordInput Input) : IRequest<User>;
