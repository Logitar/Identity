using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using MediatR;
using System.Security.Claims;

namespace Logitar.Identity.Tokens.Commands;

/// <summary>
/// The handler for <see cref="ValidateTokenCommand"/> commands.
/// </summary>
internal class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, IEnumerable<Claim>>
{
  /// <summary>
  /// The realm repository.
  /// </summary>
  private readonly IRealmRepository _realmRepository;
  /// <summary>
  /// The class managing security tokens.
  /// </summary>
  private readonly ITokenManager _tokenManager;

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidateTokenCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="realmRepository">The realm repository.</param>
  /// <param name="tokenManager">The class managing security tokens.</param>
  public ValidateTokenCommandHandler(IRealmRepository realmRepository, ITokenManager tokenManager)
  {
    _realmRepository = realmRepository;
    _tokenManager = tokenManager;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The validated token claims.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  public async Task<IEnumerable<Claim>> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
  {
    ValidateTokenInput input = request.Input;
    if (input.Realm == null)
    {
      throw new NotImplementedException(); // TODO(fpion): allow customization (Secret)
    }

    RealmAggregate? realm = input.Realm == null ? null
     : await _realmRepository.LoadAsync(input.Realm, cancellationToken)
       ?? throw new AggregateNotFoundException<RealmAggregate>(new AggregateId(input.Realm), nameof(input.Realm));

    string? audience = realm?.GetAudience(); // TODO(fpion): allow customization (AudienceFormat)
    string? issuer = realm?.GetIssuer(); // TODO(fpion): allow customization (IssuerFormat)
    string? secret = realm?.JwtSecret; // TODO(fpion): allow customization (Secret)

    ClaimsPrincipal principal = await _tokenManager.ValidateAsync(input.Token, secret, audience,
      issuer, input.Purpose, consume: false, cancellationToken);

    return principal.Claims.Select(claim => new Claim
    {
      Type = claim.Type,
      Value = claim.Value,
      ValueType = claim.ValueType
    });
  }
}
