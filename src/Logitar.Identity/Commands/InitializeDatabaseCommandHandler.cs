using MediatR;

namespace Logitar.Identity.Commands;

internal class InitializeDatabaseCommandHandler : INotificationHandler<InitializeDatabaseCommand>
{
  private readonly IConfiguration _configuration;

  public InitializeDatabaseCommandHandler(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public async Task Handle(InitializeDatabaseCommand _, CancellationToken cancellationToken)
  {
    if (_configuration.GetValue<bool>("EnableMigrations"))
    {
      // TODO(fpion): Migrate Database
    }
  }
}
