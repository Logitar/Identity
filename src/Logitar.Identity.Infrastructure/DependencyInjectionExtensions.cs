using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain;
using Logitar.Identity.Infrastructure.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityInfrastructure(this IServiceCollection services)
  {
    services.AddLogitarEventSourcingInfrastructure();

    EventSerializer eventSerializer = new();
    eventSerializer.RegisterConverter(new TenantIdConverter());
    eventSerializer.RegisterConverter(new UniqueNameConverter());

    return services
      .AddLogitarIdentityDomain()
      .AddSingleton<IEventSerializer>(eventSerializer);
  }
}
