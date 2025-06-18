using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Helpers.ThreadSwitcher;

public class Switcher<TSwitchValue> : IDisposable
{
  public Switcher(TSwitchValue newValue)
  {
    StackRawSwitcher<TSwitchValue>.Enter(newValue);
  }

  public static TSwitchValue CurrentValue
  {
    get
    {
      return StackRawSwitcher<TSwitchValue>.CurrentState;
    }
  }

  public virtual void Dispose()
  {
    StackRawSwitcher<TSwitchValue>.Exit();
  }
}