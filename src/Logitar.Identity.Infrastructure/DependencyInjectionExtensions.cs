using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain;
using Logitar.Identity.Infrastructure.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarEventSourcingInfrastructure()
      .AddLogitarIdentityDomain()
      .AddSingleton<IEventSerializer>(_ => new EventSerializer(GetJsonConverters()));
  }

  private static JsonConverter[] GetJsonConverters() => new JsonConverter[]
  {
    new SessionIdConverter(),
    new TenantIdConverter(),
    new UniqueNameConverter(),
    new UserIdConverter()
  };
}
