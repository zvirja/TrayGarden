#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Common.VMtoVMapping;

#endregion

namespace TrayGarden.UI.WindowWithReturn
{
  public interface IWindowWithBack
  {
    #region Public Properties

    bool IsCurrentlyDisplayed { get; }

    #endregion

    #region Public Methods and Operators

    void BringToFront();

    void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings);

    void PrepareAndShow(WindowWithBackVM viewModel);

    #endregion
  }
}