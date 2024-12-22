using TimeZone = Logitar.Identity.Core.TimeZone;

namespace Logitar.Identity.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class TimeZoneConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  public TimeZoneConverterTests()
  {
    _serializerOptions.Converters.Add(new TimeZoneConverter());
  }

  [Fact(DisplayName = "It should deserialize the correct value from a non-null value.")]
  public void Given_Value_When_Deserialize_Then_CorrectValue()
  {
    TimeZone timezone = new("America/Toronto");
    TimeZone? deserialized = JsonSerializer.Deserialize<TimeZone>(string.Concat('"', timezone, '"'), _serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(timezone, deserialized);
  }

  [Fact(DisplayName = "It should deserialize null from a null value.")]
  public void Given_NullValue_When_Deserialize_Then_NullValue()
  {
    TimeZone? timezone = JsonSerializer.Deserialize<TimeZone>("null", _serializerOptions);
    Assert.Null(timezone);
  }

  [Fact(DisplayName = "It should serialize correctly the value.")]
  public void Given_Value_When_Serialize_Then_SerializedCorrectly()
  {
    TimeZone timezone = new("Asia/Singapore");
    string json = JsonSerializer.Serialize(timezone, _serializerOptions);
    Assert.Equal(string.Concat('"', timezone, '"'), json);
  }
}
