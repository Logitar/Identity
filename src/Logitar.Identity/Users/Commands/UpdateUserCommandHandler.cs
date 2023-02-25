using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using MediatR;
using System.Globalization;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The handler for <see cref="UpdateUserCommand"/> commands.
/// </summary>
internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
{
  /// <summary>
  /// The current actor.
  /// </summary>
  private readonly ICurrentActor _currentActor;
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
  /// The user repository.
  /// </summary>
  private readonly IUserRepository _userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="userHelper">The user helper.</param>
  /// <param name="userQuerier">The user querier.</param>
  /// <param name="userRepository">The user repository.</param>
  public UpdateUserCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    IUserHelper userHelper,
    IUserQuerier userQuerier,
    IUserRepository userRepository)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _userHelper = userHelper;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The updated user.</returns>
  /// <exception cref="AggregateNotFoundException{UserAggregate}">The specified user could not be found.</exception>
  /// <exception cref="InvalidOperationException">The user's realm or user output could not be found.</exception>
  public async Task<User> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    UserAggregate user = await _eventStore.LoadAsync<UserAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(id);
    RealmAggregate realm = await _eventStore.LoadAsync<RealmAggregate>(user.RealmId, cancellationToken)
      ?? throw new InvalidOperationException($"The realm 'Id={user.RealmId}' could not be found.");

    UpdateUserInput input = command.Input;

    if (realm.RequireUniqueEmail && input.Email != null)
    {
      IEnumerable<UserAggregate> users = await _userRepository.LoadByEmailAsync(realm, input.Email.Address, cancellationToken);
      if (users.Any(u => !u.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(realm, input.Email.Address, nameof(input.Email));
      }
    }

    string? passwordHash = input.Password == null ? null : _userHelper.ValidateAndHashPassword(realm, input.Password);
    ReadOnlyAddress? address = input.Address == null ? null : new ReadOnlyAddress(input.Address.Line1,
      input.Address.Locality, input.Address.Country, input.Address.Line2, input.Address.PostalCode,
      input.Address.Region, input.Address.Verify);
    ReadOnlyEmail? email = input.Email == null ? null : new ReadOnlyEmail(input.Email.Address, input.Email.Verify);
    ReadOnlyPhone? phone = input.Phone == null ? null
      : new ReadOnlyPhone(input.Phone.Number, input.Phone.CountryCode, input.Phone.Extension, input.Phone.Verify);
    Gender? gender = input.Gender == null ? null : new Gender(input.Gender);
    CultureInfo? locale = input.Locale?.GetCultureInfo();
    Dictionary<string, string>? customAttributes = input.CustomAttributes?.ToDictionary();
    IEnumerable<RoleAggregate>? roles = await _userHelper.GetRolesAsync(realm, input, cancellationToken);

    user.Update(_currentActor.Id, passwordHash, address, email, phone, input.FirstName, input.MiddleName,
      input.LastName, input.Nickname, input.Birthdate, gender, locale, input.TimeZone, input.Picture,
      input.Profile, input.Website, customAttributes, roles);

    await _eventStore.SaveAsync(user, cancellationToken);

    return await _userQuerier.GetAsync(user.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user output (Id={user.Id}) could not be found.");
  }
}
