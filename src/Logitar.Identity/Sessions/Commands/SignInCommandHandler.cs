using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Users;
using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The handler for <see cref="SignInCommand"/> commands.
/// </summary>
internal class SignInCommandHandler : IRequestHandler<SignInCommand, Session>
{
  /// <summary>
  /// The event store.
  /// </summary>
  private readonly IEventStore _eventStore;
  /// <summary>
  /// The realm repository.
  /// </summary>
  private readonly IRealmRepository _realmRepository;
  /// <summary>
  /// The session helper.
  /// </summary>
  private readonly ISessionHelper _sessionHelper;
  /// <summary>
  /// The session querier.
  /// </summary>
  private readonly ISessionQuerier _sessionQuerier;
  /// <summary>
  /// The user helper.
  /// </summary>
  private readonly IUserHelper _userHelper;
  /// <summary>
  /// The user repository.
  /// </summary>
  private readonly IUserRepository _userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="SignInCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="eventStore">The event store.</param>
  /// <param name="realmRepository">The realm repository.</param>
  /// <param name="sessionHelper">The session helper.</param>
  /// <param name="sessionQuerier">The session querier.</param>
  /// <param name="userHelper">The user helper.</param>
  /// <param name="userRepository">The user repository.</param>
  public SignInCommandHandler(IEventStore eventStore,
    IRealmRepository realmRepository,
    ISessionHelper sessionHelper,
    ISessionQuerier sessionQuerier,
    IUserHelper userHelper,
    IUserRepository userRepository)
  {
    _eventStore = eventStore;
    _realmRepository = realmRepository;
    _sessionHelper = sessionHelper;
    _sessionQuerier = sessionQuerier;
    _userHelper = userHelper;
    _userRepository = userRepository;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The newly opened session.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  /// <exception cref="InvalidCredentialsException">The specified credentials did not match a single user.</exception>
  /// <exception cref="InvalidOperationException">The user session output could not be found.</exception>
  public async Task<Session> Handle(SignInCommand request, CancellationToken cancellationToken)
  {
    SignInInput input = request.Input;

    RealmAggregate realm = await _realmRepository.LoadAsync(input.Realm, cancellationToken)
      ?? throw new AggregateNotFoundException<RealmAggregate>(new AggregateId(input.Realm), nameof(input.Realm));

    UserAggregate? user = await _userRepository.LoadAsync(realm, input.Username, cancellationToken);
    if (user == null && realm.RequireUniqueEmail)
    {
      try
      {
        user = (await _userRepository.LoadByEmailAsync(realm, input.Username, cancellationToken)).SingleOrDefault();
      }
      catch (InvalidOperationException innerException)
      {
        throw new InvalidCredentialsException(innerException);
      }
    }

    if (user == null || !_userHelper.IsMatch(user, input.Password))
    {
      throw new InvalidCredentialsException();
    }

    byte[]? keyBytes = null;
    string? keyHash = input.Remember ? _sessionHelper.GenerateKey(out keyBytes) : null;
    Dictionary<string, string>? customAttributes = input.CustomAttributes?.ToDictionary();

    SessionAggregate session = new(user, keyHash, customAttributes);
    user.SignIn(realm, session.CreatedOn);

    await _eventStore.SaveAsync(new AggregateRoot[] { user, session }, cancellationToken);

    Session output = await _sessionQuerier.GetAsync(session.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user session output (Id={session.Id}) could not be found.");

    if (keyBytes != null)
    {
      output.RefreshToken = new RefreshToken(output.Id, keyBytes).ToString();
    }

    return output;
  }
}
