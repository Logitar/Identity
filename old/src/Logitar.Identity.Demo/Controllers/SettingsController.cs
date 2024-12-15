using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Identity.Demo.Controllers;

[ApiController]
[Route("settings")]
public class SettingsController : ControllerBase
{
  private readonly IRoleSettingsResolver _roleSettings;
  private readonly IUserSettingsResolver _userSettings;

  public SettingsController(IRoleSettingsResolver roleSettings, IUserSettingsResolver userSettings)
  {
    _roleSettings = roleSettings;
    _userSettings = userSettings;
  }

  [HttpGet("role")]
  public ActionResult<IRoleSettings> GetRoleSettings() => Ok(_roleSettings.Resolve());

  [HttpGet("user")]
  public ActionResult<IUserSettings> GetUserSettings() => Ok(_userSettings.Resolve());
}
