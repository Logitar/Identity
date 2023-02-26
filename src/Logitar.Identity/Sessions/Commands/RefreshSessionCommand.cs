using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The command raised to refresh an user session.
/// </summary>
/// <param name="Input">The refresh input data.</param>
internal record RefreshSessionCommand(RefreshSessionInput Input) : IRequest<Session>;
