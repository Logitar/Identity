namespace Logitar.Identity.Contracts;

public record Error
{
  public string Code { get; set; }
  public string? Message { get; set; }
  public List<ErrorData> Data { get; set; } = [];

  public Error() : this(string.Empty)
  {
  }
  public Error(string code, string? message = null, IEnumerable<ErrorData>? data = null)
  {
    Code = code;
    Message = message;
    Data = data?.ToList() ?? [];
  }
}
