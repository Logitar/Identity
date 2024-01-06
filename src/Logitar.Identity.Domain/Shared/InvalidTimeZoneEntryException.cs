namespace Logitar.Identity.Domain.Shared;

public class InvalidTimeZoneEntryException : Exception
{
  private const string ErrorMessage = "The specified time zone identifier did not resolve to a tz entry.";

  public string Id
  {
    get => (string)Data[nameof(Id)]!;
    private set => Data[nameof(Id)] = value;
  }

  public InvalidTimeZoneEntryException(string id) : base(BuildMessage(id))
  {
    Id = id;
  }

  private static string BuildMessage(string id) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(Id), id)
    .Build();
}
