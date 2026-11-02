using System;
using System.Collections.Generic;

using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace DevDashboardAgentPlant;

public class PlantConfiguration : IUserConfiguration
{
  public static PlantConfiguration Instance { get; } = new();

  public IIntUserSetting Port { get; private set; }

  public IStringUserSetting BindAddress { get; private set; }

  public IStringUserSetting DisplayName { get; private set; }

  public IStringUserSetting AllowedRoots { get; private set; }

  public IStringUserSetting GitPath { get; private set; }

  public IIntUserSetting DeadlineSeconds { get; private set; }

  public IEnumerable<IUserSettingBase> RestartingSettings => [Port, BindAddress, AllowedRoots, GitPath, DeadlineSeconds];

  public string[] ResolveAllowedRoots()
  {
    return (AllowedRoots?.Value ?? string.Empty)
      .Split([';', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
  }

  public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
  {
    Port = personalSettingsSteward.DeclareIntSetting(
      "Port",
      "Listening port",
      7443,
      "TCP port the agent listens on. Changing it restarts a running agent.");
    BindAddress = personalSettingsSteward.DeclareStringSetting(
      "BindAddress",
      "Bind address",
      "127.0.0.1",
      "IP address the agent listens on. Use the address the dashboard reaches (e.g. the tailnet IP) rather than 0.0.0.0 where possible.");
    AllowedRoots = personalSettingsSteward.DeclareStringSetting(
      "AllowedRoots",
      "Allowed folders",
      string.Empty,
      "Existing absolute folders whose repositories are served, separated by semicolons. Changing them invalidates all paired credentials.");
    GitPath = personalSettingsSteward.DeclareStringSetting(
      "GitPath",
      "Git executable",
      string.Empty,
      "Optional absolute path to git.exe (Git 2.32 or newer). Leave empty to find Git on PATH or in the standard installation folder.");
    DeadlineSeconds = personalSettingsSteward.DeclareIntSetting(
      "DeadlineSeconds",
      "Request deadline (seconds)",
      20,
      "Maximum time per request, from 1 to 120 seconds. Increase this for folders with many or large repositories, or for fetches over a slow link.");
    DisplayName = personalSettingsSteward.DeclareStringSetting(
      "DisplayName",
      "Display name",
      Environment.MachineName,
      "Name of the agent's certificate. Applies only when the certificate is generated.");
  }
}
