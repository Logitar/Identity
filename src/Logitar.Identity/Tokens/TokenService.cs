using Logitar.Identity.Tokens.Commands;

namespace Logitar.Identity.Tokens;

/// <summary>
/// Implements methods to manage tokens in the identity system.
/// </summary>
internal class TokenService : ITokenService
{
  /// <summary>
  /// The request pipeline.
  /// </summary>
  private readonly IRequestPipeline _requestPipeline;

  /// <summary>
  /// Initializes a new instance of the <see cref="TokenService"/> using the specified arguments.
  /// </summary>
  /// <param name="requestPipeline">The request pipeline.</param>
  public TokenService(IRequestPipeline requestPipeline)
  {
    _requestPipeline = requestPipeline;
  }

  /// <summary>
  /// Creates a new token.
  /// </summary>
  /// <param name="input">The input creation arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly created token string.</returns>
  public async Task<string> CreateAsync(CreateTokenInput input, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new CreateTokenCommand(input), cancellationToken);
  }

  /// <summary>
  /// Validates a security token.
  /// </summary>
  /// <param name="input">The input validation arguments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token claims.</returns>
  public async Task<IEnumerable<Claim>> ValidateAsync(ValidateTokenInput input, CancellationToken cancellationToken)
  {
    return await _requestPipeline.ExecuteAsync(new ValidateTokenCommand(input, Consume: false), cancellationToken);
  }
}
