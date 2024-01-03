namespace Logitar.Identity.Contracts;

public record ErrorData
{
  public string Key { get; set; }
  public string? Value { get; set; }

  public ErrorData() : this(string.Empty, value: null)
  {
  }
  public ErrorData(string key, string? value)
  {
    Key = key;
    Value = value;
  }
}
