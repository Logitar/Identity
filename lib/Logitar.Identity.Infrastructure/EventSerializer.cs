using Logitar.Identity.Infrastructure.Converters;

namespace Logitar.Identity.Infrastructure;

public class EventSerializer : EventSourcing.Infrastructure.EventSerializer
{
  private readonly PasswordConverter _passwordConverter;

  public EventSerializer(PasswordConverter passwordConverter)
  {
    _passwordConverter = passwordConverter;
  }

  protected override void RegisterConverters()
  {
    base.RegisterConverters();

    SerializerOptions.Converters.Add(_passwordConverter);
    SerializerOptions.Converters.Add(new ApiKeyIdConverter());
    SerializerOptions.Converters.Add(new CustomIdentifierConverter());
    SerializerOptions.Converters.Add(new DescriptionConverter());
    SerializerOptions.Converters.Add(new DisplayNameConverter());
    SerializerOptions.Converters.Add(new EntityIdConverter());
    SerializerOptions.Converters.Add(new GenderConverter());
    SerializerOptions.Converters.Add(new IdentifierConverter());
    SerializerOptions.Converters.Add(new LocaleConverter());
    SerializerOptions.Converters.Add(new OneTimePasswordIdConverter());
    SerializerOptions.Converters.Add(new PersonNameConverter());
    SerializerOptions.Converters.Add(new RoleIdConverter());
    SerializerOptions.Converters.Add(new SessionIdConverter());
    SerializerOptions.Converters.Add(new TenantIdConverter());
    SerializerOptions.Converters.Add(new TimeZoneConverter());
    SerializerOptions.Converters.Add(new UniqueNameConverter());
    SerializerOptions.Converters.Add(new UrlConverter());
    SerializerOptions.Converters.Add(new UserIdConverter());
  }
}
