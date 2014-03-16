#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

#endregion

namespace WindowsLangHotKeysSetter
{
  public class ParamsConfigurator : TrayGarden.Reception.Services.IUserConfiguration
  {
    #region Static Fields

    public static ParamsConfigurator Instance = new ParamsConfigurator();

    #endregion

    #region Properties

    protected IStringUserSetting ConfiguredArgsSets { get; set; }

    protected IPersonalUserSettingsSteward PersonalSteward { get; set; }

    #endregion

    #region Public Methods and Operators

    public List<Tuple<UInt32, UInt32, UInt32, IntPtr>> GetArgsTuples()
    {
      string rawStringValue = this.ConfiguredArgsSets.Value;
      if (rawStringValue.IsNullOrEmpty())
      {
        return null;
      }
      string[] rawSets = rawStringValue.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
      if (rawSets.Length == 0)
      {
        return null;
      }
      var result = new List<Tuple<UInt32, UInt32, UInt32, IntPtr>>();
      foreach (string rawSet in rawSets)
      {
        Tuple<UInt32, UInt32, UInt32, IntPtr> parsedSet = this.ParseSet(rawSet);
        if (parsedSet != null)
        {
          result.Add(parsedSet);
        }
      }
      return result.Count != 0 ? result : null;
    }

    public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
    {
      this.PersonalSteward = personalSettingsSteward;
      this.DeclareSettings();
    }

    #endregion

    #region Methods

    protected virtual void DeclareSettings()
    {
      this.ConfiguredArgsSets = this.PersonalSteward.DeclareStringSetting(
        "ArgsSets",
        "Parameters sets",
        "00000100,0000c005,00000031,04090409|00000101,0000c005,00000032,04190419|00000102,0000c005,00000033,f0a80422",
        "Here you should specify parameters sets for the CliImmSetHotKey method calls. The format of string is following: dwID-UINT,uModifiers-UINT,uVirtualKey-UINT,hkl-UINT|... Parameter values should be specified in HEX without prefix.");
    }

    protected Tuple<uint, uint, uint, IntPtr> ParseSet(string rawSet)
    {
      string[] rawArgs = rawSet.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      if (rawArgs.Length != 4)
      {
        return null;
      }
      try
      {
        NumberStyles parseStyle = NumberStyles.HexNumber;
        uint dwID = uint.Parse(rawArgs[0], parseStyle);
        uint uModifiers = uint.Parse(rawArgs[1], parseStyle);
        uint uVirtualKey = uint.Parse(rawArgs[2], parseStyle);
        IntPtr hkl;
        if (IntPtr.Size == 4)
        {
          hkl = new IntPtr(uint.Parse(rawArgs[3], parseStyle));
        }
        else
        {
          hkl = new IntPtr(long.Parse(rawArgs[3], parseStyle));
        }
        return new Tuple<uint, uint, uint, IntPtr>(dwID, uModifiers, uVirtualKey, hkl);
      }
      catch (Exception ex)
      {
        Log.Error("WindowsLangHotKeysSetter. Unable to parse the args set: '{0}'".FormatWith(rawSet), ex, this);
      }
      return null;
    }

    #endregion
  }
}