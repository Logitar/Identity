using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Sessions.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.Sessions;

/// <summary>
/// The handler for <see cref="SessionRefreshedEvent"/> events.
/// </summary>
internal class SessionRefreshedEventHandler : INotificationHandler<SessionRefreshedEvent>
{
  /// <summary>
  /// The identity database context.
  /// </summary>
  private readonly IdentityContext _context;
  /// <summary>
  /// The logger instance.
  /// </summary>
  private readonly ILogger<SessionRefreshedEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionRefreshedEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public SessionRefreshedEventHandler(IdentityContext context, ILogger<SessionRefreshedEventHandler> logger)
  {
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="notification">The event to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public async Task Handle(SessionRefreshedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      SessionEntity? session = await _context.Sessions
        .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
      if (session == null)
      {
        _logger.LogError("The session 'AggregateId={id}' could not be found.", notification.AggregateId);
        return;
      }

      session.Refresh(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
