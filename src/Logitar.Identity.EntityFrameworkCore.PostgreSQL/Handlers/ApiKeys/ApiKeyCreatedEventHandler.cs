using Logitar.Identity.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.ApiKeys;

/// <summary>
/// The handler for <see cref="ApiKeyCreatedEvent"/> events.
/// </summary>
internal class ApiKeyCreatedEventHandler : INotificationHandler<ApiKeyCreatedEvent>
{
  /// <summary>
  /// The actor service.
  /// </summary>
  private readonly IActorService _actorService;
  /// <summary>
  /// The identity database context.
  /// </summary>
  private readonly IdentityContext _context;
  /// <summary>
  /// The logger instance.
  /// </summary>
  private readonly ILogger<ApiKeyCreatedEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyCreatedEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="actorService">The actor service.</param>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public ApiKeyCreatedEventHandler(IActorService actorService,
    IdentityContext context,
    ILogger<ApiKeyCreatedEventHandler> logger)
  {
    _actorService = actorService;
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="notification">The event to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public async Task Handle(ApiKeyCreatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      RealmEntity? realm = await _context.Realms
        .Include(x => x.Roles)
        .SingleOrDefaultAsync(x => x.AggregateId == notification.RealmId.Value, cancellationToken);
      if (realm == null)
      {
        _logger.LogError("The realm 'AggregateId={id}' could not be found.", notification.RealmId);
        return;
      }

      ActorEntity actor = await _actorService.GetActorAsync(notification.ActorId, cancellationToken);
      ApiKeyEntity apiKey = new(notification, realm, actor);
      if (notification.Roles.Any())
      {
        IEnumerable<string> roleIds = notification.Roles.Select(id => id.Value).Distinct();

        RoleEntity[] roles = await _context.Roles
          .Where(x => roleIds.Contains(x.AggregateId))
          .ToArrayAsync(cancellationToken);

        apiKey.Roles.AddRange(roles);
      }

      _context.ApiKeys.Add(apiKey);
      await _context.SaveChangesAsync(cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
