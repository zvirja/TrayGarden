using System;

using Serilog;

namespace TrayGarden.Diagnostics;

public static class Log
{
  public static ILogger For<T>(T owner)
  {
    return Serilog.Log.ForContext<T>();
  }

  public static ILogger For<T>()
  {
    return Serilog.Log.ForContext<T>();
  }

  public static ILogger For(Type type)
  {
    return Serilog.Log.ForContext(type ?? typeof(Log));
  }
}
