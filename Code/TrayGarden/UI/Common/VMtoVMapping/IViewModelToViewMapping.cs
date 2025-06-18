using System;
using System.Windows.Controls;

namespace TrayGarden.UI.Common.VMtoVMapping;

public interface IViewModelToViewMapping
{
  Type AcceptableViewModelType { get; }

  Control GetControl(object contextVM);
}