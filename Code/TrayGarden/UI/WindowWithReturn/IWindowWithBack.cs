using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.UI.Common.VMtoVMapping;

namespace TrayGarden.UI.WindowWithReturn;

public interface IWindowWithBack
{
  bool IsCurrentlyDisplayed { get; }

  void BringToFront();

  void Initialize([NotNull] List<IViewModelToViewMapping> mvtovmappings);

  void PrepareAndShow(WindowWithBackVM viewModel);
}