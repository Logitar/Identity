﻿using Logitar.Identity.Demo.Constants;

namespace Logitar.Identity.Demo.Models.Index;

public record ApiVersion
{
  public string Title { get; set; }
  public string Version { get; set; }

  public ApiVersion() : this(Api.Title, Api.Version)
  {
  }

  public ApiVersion(string title, Version version) : this(title, version.ToString())
  {
  }

  public ApiVersion(string title, string version)
  {
    Title = title;
    Version = version;
  }
}
