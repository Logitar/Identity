using MediatR;

namespace Logitar.Identity.Tokens.Commands;

/// <summary>
/// The command raised to validate a token.
/// </summary>
/// <param name="Input">The validation input data.</param>
/// <param name="Consume">If true, the token will be consumed. A consumed token cannot be used again.</param>
internal record ValidateTokenCommand(ValidateTokenInput Input, bool Consume) : IRequest<IEnumerable<Claim>>;
