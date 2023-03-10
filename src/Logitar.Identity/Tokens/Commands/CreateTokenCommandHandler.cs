using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Tokens.Validators;
using MediatR;
using System.Security.Claims;

namespace Logitar.Identity.Tokens.Commands;

/// <summary>
/// The handler for <see cref="CreateTokenCommand"/> commands.
/// </summary>
internal class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, string>
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
  /// Initializes a new instance of the <see cref="CreateTokenCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="realmRepository">The realm repository.</param>
  /// <param name="tokenManager">The class managing security tokens.</param>
  public CreateTokenCommandHandler(IRealmRepository realmRepository, ITokenManager tokenManager)
  {
    _realmRepository = realmRepository;
    _tokenManager = tokenManager;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The token string.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  public async Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
  {
    CreateTokenInput input = request.Input;
    new CreateTokenValidator().ValidateAndThrow(input);

    RealmAggregate? realm = input.Realm == null ? null
      : await _realmRepository.LoadAsync(input.Realm, cancellationToken)
          ?? throw new AggregateNotFoundException<RealmAggregate>(new AggregateId(input.Realm), nameof(input.Realm));

    ClaimsIdentity identity = new();

    DateTime? expires = input.Lifetime.HasValue
      ? DateTime.UtcNow.AddSeconds(input.Lifetime.Value)
      : null;

    if (input.IsConsumable)
    {
      identity.AddClaim(CreateClaim(Rfc7519ClaimTypes.JwtId, Guid.NewGuid().ToString()));
    }

    if (input.Purpose != null)
    {
      identity.AddClaim(CreateClaim(CustomClaimTypes.Purpose, input.Purpose.ToLower()));
    }

    if (input.Subject != null)
    {
      identity.AddClaim(CreateClaim(Rfc7519ClaimTypes.Subject, input.Subject));
    }

    if (input.Claims != null)
    {
      identity.AddClaims(input.Claims.Select(CreateClaim));
    }

    string? audience = input.Audience?.Format(realm) ?? realm?.GetAudience();
    string? issuer = input.Issuer?.Format(realm) ?? realm?.GetIssuer();
    string? secret = realm?.JwtSecret ?? input.Secret ?? string.Empty;

    return _tokenManager.Create(identity, secret, input.Algorithm, expires, audience, issuer);
  }

  /// <summary>
  /// Creates a security claim using the specified claim input data.
  /// </summary>
  /// <param name="claim">The claim input data.</param>
  /// <returns>The security claim.</returns>
  private static System.Security.Claims.Claim CreateClaim(Claim claim)
    => CreateClaim(claim.Type, claim.Value, claim.ValueType);
  /// <summary>
  /// Creates a security claim using the specified arguments.
  /// </summary>
  /// <param name="type">The type of the claim.</param>
  /// <param name="value">The value of the claim.</param>
  /// <param name="valueType">The type of the claim's value.</param>
  /// <returns>The security claim.</returns>
  private static System.Security.Claims.Claim CreateClaim(string type, string value, string? valueType = null)
    => new(type, value, valueType);
}
