#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline
{
  public class InitializeFirstStep
  {
    #region Public Properties

    public string GlobalTitle { get; set; }

    public string Header { get; set; }

    public string ShortName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetMainVMPipelineArgs args)
    {
      Assert.IsNotNull(args.ResultVM, "Result VM can't be null");
      Assert.IsNotNull(args.PlantsConfigVM, "PlantsConfig VM can't be null");
      args.ResultVM.ReplaceInitialState(this.GetInitialStep(args));
    }

    #endregion

    #region Methods

    protected virtual WindowStepState GetInitialStep(GetMainVMPipelineArgs args)
    {
      var step = new WindowStepState(
        this.GlobalTitle.GetValueOrDefault("Tray Garden -- Plants configuration"),
        this.Header.GetValueOrDefault("Here you configure plants"),
        this.ShortName.GetValueOrDefault("plants config"),
        args.PlantsConfigVM,
        args.SuperAction,
        args.StateSpecificHelpActions);
      return step;
    }

    #endregion
  }
}