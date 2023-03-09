using MediatR;

namespace Logitar.Identity.Tokens.Commands;

/// <summary>
/// The command raised to create a new token.
/// </summary>
/// <param name="Input">The creation input data.</param>
internal record CreateTokenCommand(CreateTokenInput Input) : IRequest<string>;
