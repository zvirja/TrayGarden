using System;

namespace TrayGarden;

public class NonInitializedException : Exception
{
  public NonInitializedException()
    : base("Object isn't properly initialized. Should be initialized before usage.")
  {
  }
}