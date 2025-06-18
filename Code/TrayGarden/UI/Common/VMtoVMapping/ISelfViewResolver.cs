using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TrayGarden.UI.Common.VMtoVMapping;

public interface ISelfViewResolver
{
  Control GetViewToPresentMe();
}