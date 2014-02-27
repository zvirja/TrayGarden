#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.Engine
{
  public interface IServicesSteward
  {
    #region Public Properties

    List<IService> Services { get; set; }

    #endregion

    #region Public Methods and Operators

    void InformClosingStage();

    void InformDisplayStage();

    void InformInitializeStage();

    #endregion
  }
}