using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Users.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.Users;

/// <summary>
/// The handler for <see cref="PhoneUpdatedEvent"/> events.
/// </summary>
internal class PhoneUpdatedEventHandler : INotificationHandler<PhoneUpdatedEvent>
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
  private readonly ILogger<PhoneUpdatedEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="PhoneUpdatedEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="actorService">The actor service.</param>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public PhoneUpdatedEventHandler(IActorService actorService,
    IdentityContext context,
    ILogger<PhoneUpdatedEventHandler> logger)
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
  public async Task Handle(PhoneUpdatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      UserEntity? user = await _context.Users
        .Include(x => x.ExternalIdentifiers)
        .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
      if (user == null)
      {
        _logger.LogError("The user 'AggregateId={id}' could not be found.", notification.AggregateId);
        return;
      }

      ActorEntity actor = await _actorService.GetActorAsync(notification.ActorId, cancellationToken);
      user.SetPhone(notification, actor);
      await _context.SaveChangesAsync(cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
