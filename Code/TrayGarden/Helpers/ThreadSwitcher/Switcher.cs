#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Helpers.ThreadSwitcher
{
  public class Switcher<TSwitchValue> : IDisposable
  {
    #region Constructors and Destructors

    public Switcher(TSwitchValue newValue)
    {
      StackRawSwitcher<TSwitchValue>.Enter(newValue);
    }

    #endregion

    #region Public Properties

    public static TSwitchValue CurrentValue
    {
      get
      {
        return StackRawSwitcher<TSwitchValue>.CurrentState;
      }
    }

    #endregion

    #region Public Methods and Operators

    public virtual void Dispose()
    {
      StackRawSwitcher<TSwitchValue>.Exit();
    }

    #endregion
  }
}