#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep
{
  [UsedImplicitly]
  public class CreateStepInfo
  {
    #region Constructors and Destructors

    public CreateStepInfo()
    {
      this.ShortName = "user settings";
      this.Header = "User settings for plant";
      this.GlobalTitle = "Tray Garden -- User settings for #plantName plant";
    }

    #endregion

    #region Public Properties

    public string GlobalTitle { get; set; }

    public string Header { get; set; }

    public string ShortName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetUCStepPipelineArgs args)
    {
      Assert.IsNotNull(args.ConfigurationConstructInfo.ResultControlVM, "args.ConfigurationConstructInfo.ResultControlVM");
      WindowWithBackStateConstructInfo stateInfo = args.StateConstructInfo;
      stateInfo.ResultState = new WindowStepState(
        this.GetGlobalTitle(args),
        stateInfo.Header ?? this.Header,
        stateInfo.ShortName ?? this.ShortName,
        args.ConfigurationConstructInfo.ResultControlVM,
        stateInfo.SuperAction,
        stateInfo.StateSpecificHelpActions);
    }

    #endregion

    #region Methods

    protected virtual string GetGlobalTitle(GetUCStepPipelineArgs args)
    {
      var globalTitle = args.StateConstructInfo.GlobalTitle ?? this.GlobalTitle;
      return globalTitle.Replace("#plantName", args.UCServicePlantBox.RelatedPlantEx.Plant.HumanSupportingName);
    }

    #endregion
  }
}