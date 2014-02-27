#region

//using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Helpers.ThreadSwitcher
{
  public static class StackRawSwitcher<TSwitchValue>
  {
    #region Static Fields

    private static string _itemKey;

    #endregion

    #region Public Properties

    public static TSwitchValue CurrentState
    {
      get
      {
        return GetCurrentSwitchState();
      }
    }

    #endregion

    #region Properties

    private static string ItemKey
    {
      get
      {
        if (_itemKey != null)
        {
          return _itemKey;
        }
        _itemKey = typeof(TSwitchValue).FullName + "_Swither";
        return _itemKey;
      }
    }

    #endregion

    #region Public Methods and Operators

    public static void Enter(TSwitchValue newValue)
    {
      GetStack(true).Push(newValue);
    }

    public static void Exit()
    {
      Stack<TSwitchValue> stack = GetStack(false);
      if (stack == null || stack.Count == 0)
      {
        throw new InvalidOperationException("The stack is empty");
      }
      stack.Pop();
    }

    #endregion

    #region Methods

    private static TSwitchValue GetCurrentSwitchState()
    {
      Stack<TSwitchValue> stack = GetStack(false);
      if (stack == null || stack.Count == 0)
      {
        return default(TSwitchValue);
      }
      return stack.Peek();
    }

    private static Stack<TSwitchValue> GetStack(bool createIfNotExist)
    {
      var currentStack = ThreadContext.ContextItems[ItemKey] as Stack<TSwitchValue>;
      if (currentStack != null || !createIfNotExist)
      {
        return currentStack;
      }
      currentStack = new Stack<TSwitchValue>();
      ThreadContext.ContextItems[ItemKey] = currentStack;
      return currentStack;
    }

    #endregion
  }
}