using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using MediatR;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The handler for <see cref="ChangePasswordCommand"/> commands.
/// </summary>
internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, User>
{
  /// <summary>
  /// The event store.
  /// </summary>
  private readonly IEventStore _eventStore;
  /// <summary>
  /// The user helper.
  /// </summary>
  private readonly IUserHelper _userHelper;
  /// <summary>
  /// The user querier.
  /// </summary>
  private readonly IUserQuerier _userQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="ChangePasswordCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="eventStore">The event store.</param>
  /// <param name="userHelper">The user helper.</param>
  /// <param name="userQuerier">The user querier.</param>
  public ChangePasswordCommandHandler(IEventStore eventStore,
    IUserHelper userHelper,
    IUserQuerier userQuerier)
  {
    _eventStore = eventStore;
    _userHelper = userHelper;
    _userQuerier = userQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="request">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The updated user.</returns>
  /// <exception cref="AggregateNotFoundException{UserAggregate}">The specified user could not be found.</exception>
  /// <exception cref="InvalidOperationException">The user's realm or user output could not be found.</exception>
  /// <exception cref="InvalidCredentialsException">The actual user password did not match.</exception>
  public async Task<User> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
  {
    AggregateId userId = new(request.Id);
    UserAggregate user = await _eventStore.LoadAsync<UserAggregate>(userId, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(userId);
    RealmAggregate realm = await _eventStore.LoadAsync<RealmAggregate>(user.RealmId, cancellationToken)
      ?? throw new InvalidOperationException($"The realm 'Id={user.RealmId}' could not be found.");

    if (!_userHelper.IsMatch(user, request.Input.Current))
    {
      throw new InvalidCredentialsException();
    }

    string passwordHash = _userHelper.ValidateAndHashPassword(realm, request.Input.Password);

    user.ChangePassword(passwordHash);

    await _eventStore.SaveAsync(user, cancellationToken);

    return await _userQuerier.GetAsync(user.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user output (Id={user.Id}) could not be found.");
  }
}
