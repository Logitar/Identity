using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The handler for <see cref="RefreshSessionCommand"/> commands.
/// </summary>
internal class RefreshSessionCommandHandler : IRequestHandler<RefreshSessionCommand, Session>
{
  /// <summary>
  /// The event store.
  /// </summary>
  private readonly IEventStore _eventStore;
  /// <summary>
  /// The session helper.
  /// </summary>
  private readonly ISessionHelper _sessionHelper;
  /// <summary>
  /// The session querier.
  /// </summary>
  private readonly ISessionQuerier _sessionQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="RefreshSessionCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="eventStore">The event store.</param>
  /// <param name="sessionHelper">The session helper.</param>
  /// <param name="sessionQuerier">The session querier.</param>
  public RefreshSessionCommandHandler(IEventStore eventStore,
    ISessionHelper sessionHelper,
    ISessionQuerier sessionQuerier)
  {
    _eventStore = eventStore;
    _sessionHelper = sessionHelper;
    _sessionQuerier = sessionQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The refreshed session.</returns>
  /// <exception cref="InvalidCredentialsException">The refresh token could not be parsed, or does not match an user session.</exception>
  /// <exception cref="InvalidOperationException">The user session output could not be found.</exception>
  public async Task<Session> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
  {
    RefreshToken refreshToken;
    try
    {
      refreshToken = RefreshToken.Parse(request.Input.RefreshToken);
    }
    catch (Exception innerException)
    {
      throw new InvalidCredentialsException(innerException);
    }

    SessionAggregate? session = await _eventStore.LoadAsync<SessionAggregate>(new AggregateId(refreshToken.Id), cancellationToken);
    if (session == null || !_sessionHelper.IsMatch(session, refreshToken.Key))
    {
      throw new InvalidCredentialsException();
    }

    string keyHash = _sessionHelper.GenerateKey(out byte[] keyBytes);
    Dictionary<string, string>? customAttributes = request.Input.CustomAttributes?.ToDictionary();

    session.Refresh(keyHash, customAttributes);

    await _eventStore.SaveAsync(session, cancellationToken);

    Session output = await _sessionQuerier.GetAsync(session.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user session output (Id={session.Id}) could not be found.");

    output.RefreshToken = new RefreshToken(output.Id, keyBytes).ToString();

    return output;
  }
}
