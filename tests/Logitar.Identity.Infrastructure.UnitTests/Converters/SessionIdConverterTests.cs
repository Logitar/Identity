using Logitar.Identity.Core;
using Logitar.Identity.Core.Sessions;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class SessionIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public SessionIdConverterTests()
  {
    _serializerOptions.Converters.Add(new SessionIdConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    SessionId sessionId = SessionId.NewId(TenantId.NewId());
    SessionId deserialized = JsonSerializer.Deserialize<SessionId>(string.Concat('"', sessionId, '"'), _serializerOptions);
    Assert.Equal(sessionId, deserialized);
  }

  [Fact(DisplayName = "It should deserialize the default value from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_DefaultValue()
  {
    SessionId sessionId = JsonSerializer.Deserialize<SessionId>("null", _serializerOptions);
    Assert.Equal(default, sessionId);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    SessionId sessionId = SessionId.NewId(TenantId.NewId());
    string json = JsonSerializer.Serialize(sessionId, _serializerOptions);
    Assert.Equal(string.Concat('"', sessionId, '"'), json);
  }
}
