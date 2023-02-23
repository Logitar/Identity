using Logitar.EventSourcing;
using Logitar.Identity.Accounts;
using MediatR;
using System.Globalization;

namespace Logitar.Identity.Realms.Commands;

/// <summary>
/// The handler for <see cref="UpdateRealmCommand"/> commands.
/// </summary>
internal class UpdateRealmCommandHandler : IRequestHandler<UpdateRealmCommand, Realm>
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
  /// The realm querier.
  /// </summary>
  private readonly IRealmQuerier _realmQuerier;

  /// <summary>
  /// Initializes a new instance of the <see cref="UpdateRealmCommandHandler"/> class using the specified arguments.
  /// </summary>
  /// <param name="currentActor">The current actor.</param>
  /// <param name="eventStore">The event store.</param>
  /// <param name="realmQuerier">The realm querier.</param>
  public UpdateRealmCommandHandler(ICurrentActor currentActor,
    IEventStore eventStore,
    IRealmQuerier realmQuerier)
  {
    _currentActor = currentActor;
    _eventStore = eventStore;
    _realmQuerier = realmQuerier;
  }

  /// <summary>
  /// Handles the specified command instance.
  /// </summary>
  /// <param name="command">The command to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The updated realm.</returns>
  /// <exception cref="AggregateNotFoundException{RealmAggregate}">The specified realm could not be found.</exception>
  /// <exception cref="InvalidOperationException">The realm output could not be found.</exception>
  public async Task<Realm> Handle(UpdateRealmCommand command, CancellationToken cancellationToken)
  {
    AggregateId id = new(command.Id);
    RealmAggregate realm = await _eventStore.LoadAsync<RealmAggregate>(id, cancellationToken)
      ?? throw new AggregateNotFoundException<RealmAggregate>(id);

    UpdateRealmInput input = command.Input;

    CultureInfo? defaultLocale = input.DefaultLocale?.GetCultureInfo();
    ReadOnlyUsernameSettings? usernameSettings = input.UsernameSettings == null ? null : new(input.UsernameSettings);
    ReadOnlyPasswordSettings? passwordSettings = input.PasswordSettings == null ? null : new(input.PasswordSettings);
    Dictionary<string, ReadOnlyClaimMapping>? claimMappings = RealmHelper.GetClaimMappings(input);
    Dictionary<string, string>? customAttributes = input.CustomAttributes?.ToDictionary();
    Dictionary<ExternalProvider, ExternalProviderConfiguration> externalProviders = RealmHelper
      .GetExternalProviders(input.GoogleOAuth2Configuration);

    realm.Update(_currentActor.Id, input.DisplayName, input.Description, defaultLocale, input.Url,
      input.RequireConfirmedAccount, input.RequireUniqueEmail, usernameSettings, passwordSettings,
      input.JwtSecret, claimMappings, customAttributes, externalProviders);

    await _eventStore.SaveAsync(realm, cancellationToken);

    return await _realmQuerier.GetAsync(realm.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The realm output (Id={realm.Id}) could not be found.");
  }
}
