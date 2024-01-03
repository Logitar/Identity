using Logitar.Identity.Constants;

namespace Logitar.Identity.Models.Index;

public record ApiVersion
{
  public string Title { get; set; } = Api.Title;
  public string Version { get; set; } = Api.Version.ToString();
}
