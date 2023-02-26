using Logitar.EventSourcing;
using Logitar.Identity.Realms;
using Logitar.Identity.Roles;
using MediatR;
using System.Globalization;

namespace Logitar.Identity.Users.Commands;

/// <summary>
/// The handler for <see cref="CreateUserCommand"/> commands.
/// </summary>
internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
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
  /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="userHelper">The user helper.</param>
  /// <param name="userQuerier">The user querier.</param>
  /// <param name="userRepository">The user repository.</param>
  public CreateUserCommandHandler(ICurrentActor currentActor,
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
  /// <returns>The created user.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  /// <exception cref="UniqueNameAlreadyUsedException">The specified unique name is already used.</exception>
  /// <exception cref="InvalidOperationException">The user output could not be found.</exception>
  public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
  {
    CreateUserInput input = command.Input;

    AggregateId realmId = new(input.RealmId);
    RealmAggregate realm = await _eventStore.LoadAsync<RealmAggregate>(realmId, cancellationToken)
      ?? throw new AggregateNotFoundException<RealmAggregate>(realmId, nameof(input.RealmId));

    if (await _userRepository.LoadAsync(realm, input.Username, cancellationToken) != null)
    {
      throw new UsernameAlreadyUsedException(realm, input.Username, nameof(input.Username));
    }

    if (realm.RequireUniqueEmail && input.Email != null)
    {
      if ((await _userRepository.LoadByEmailAsync(realm, input.Email.Address, cancellationToken)).Any())
      {
        throw new EmailAddressAlreadyUsedException(realm, input.Email.Address, nameof(input.Email));
      }
    }

    string? passwordHash = input.Password == null ? null : _userHelper.ValidateAndHashPassword(realm, input.Password);
    ReadOnlyAddress? address = input.Address == null ? null : new ReadOnlyAddress(input.Address.Line1,
      input.Address.Locality, input.Address.Country, input.Address.Line2, input.Address.PostalCode,
      input.Address.Region, input.Address.IsVerified);
    ReadOnlyEmail? email = input.Email == null ? null : new ReadOnlyEmail(input.Email.Address, input.Email.IsVerified);
    ReadOnlyPhone? phone = input.Phone == null ? null
      : new ReadOnlyPhone(input.Phone.Number, input.Phone.CountryCode, input.Phone.Extension, input.Phone.IsVerified);
    Gender? gender = input.Gender == null ? null : new Gender(input.Gender);
    CultureInfo? locale = input.Locale?.GetCultureInfo();
    Dictionary<string, string>? customAttributes = input.CustomAttributes?.ToDictionary();
    IEnumerable<RoleAggregate>? roles = await _userHelper.GetRolesAsync(realm, input, cancellationToken);

    UserAggregate user = new(_currentActor.Id, realm, input.Username, passwordHash, address, email, phone,
      input.FirstName, input.MiddleName, input.LastName, input.Nickname, input.Birthdate, gender,
      locale, input.TimeZone, input.Picture, input.Profile, input.Website, customAttributes, roles);

    await _eventStore.SaveAsync(user, cancellationToken);

    return await _userQuerier.GetAsync(user.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The user output (Id={user.Id}) could not be found.");
  }
}
