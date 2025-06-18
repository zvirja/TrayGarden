using System;
using System.Reflection;
using log4net;
using log4net.Config;

namespace TrayGarden.Diagnostics;

public static class Log
{
  static Log()
  {
    //var pp =  SecurityContextProvider.DefaultProvider.CreateSecurityContext(new object());

    XmlConfigurator.Configure();
  }

  public static void Debug(string message, Type type)
  {
    Assert.ArgumentNotNull(message, "message");
    ILog logger = LogManager.GetLogger(type ?? typeof(Log));
    if (logger != null)
    {
      logger.Debug(message);
    }
  }

  public static void Debug(string message, object contextObject)
  {
    Debug(message, contextObject != null ? contextObject.GetType() : null);
  }

  public static void Error(string message, Exception exception, Type ownerType)
  {
    Assert.ArgumentNotNull(message, "message");
    Assert.ArgumentNotNull(ownerType, "ownerType");
    if (string.IsNullOrEmpty(message))
    {
      message = "Exception with no description";
    }
    if (ownerType == null)
    {
      ownerType = MethodBase.GetCurrentMethod().DeclaringType;
    }
    ILog logger = LogManager.GetLogger(ownerType);
    if (logger != null)
    {
      if (exception != null)
      {
        logger.Error(message, exception);
      }
      else
      {
        logger.Error(message);
      }
    }
  }

  public static void Error(string message, Exception exception, object contextObject)
  {
    Error(message, exception, contextObject != null ? contextObject.GetType() : null);
  }

  public static void Info(string message, Type ownerType)
  {
    Assert.ArgumentNotNull(message, "message");
    Assert.ArgumentNotNull(ownerType, "ownerType");
    ILog logger = LogManager.GetLogger(ownerType);
    if (logger != null)
    {
      logger.Info(message);
    }
  }

  public static void Info(string message, object contextObject)
  {
    Info(message, contextObject != null ? contextObject.GetType() : null);
  }

  public static void Warn(string message, Type ownerType, Exception exception = null)
  {
    if (string.IsNullOrEmpty(message))
    {
      message = "Exception with no description";
    }
    if (ownerType == null)
    {
      ownerType = MethodBase.GetCurrentMethod().DeclaringType;
    }
    ILog logger = LogManager.GetLogger(ownerType);
    if (logger != null)
    {
      if (exception != null)
      {
        logger.Warn(message, exception);
      }
      else
      {
        logger.Warn(message);
      }
    }
  }

  public static void Warn(string message, object contextObject, Exception exception = null)
  {
    Warn(message, contextObject != null ? contextObject.GetType() : null, exception);
  }
}