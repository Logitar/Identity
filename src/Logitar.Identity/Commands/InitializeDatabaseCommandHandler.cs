using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.Commands;

internal class InitializeDatabaseCommandHandler : INotificationHandler<InitializeDatabaseCommand>
{
  private readonly IConfiguration _configuration;
  private readonly EventContext _eventContext;
  private readonly IdentityContext _identityContext;

  public InitializeDatabaseCommandHandler(IConfiguration configuration, EventContext eventContext, IdentityContext identityContext)
  {
    _configuration = configuration;
    _eventContext = eventContext;
    _identityContext = identityContext;
  }

  public async Task Handle(InitializeDatabaseCommand _, CancellationToken cancellationToken)
  {
    if (_configuration.GetValue<bool>("EnableMigrations"))
    {
      await _eventContext.Database.MigrateAsync(cancellationToken);
      await _identityContext.Database.MigrateAsync(cancellationToken);
    }
  }
}
