using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling.PipelineModel;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling
{
  [UsedImplicitly]
  public class RequestHandlerChief
  {
    private static readonly Lazy<List<RequestHandler>> _handlers =
      new Lazy<List<RequestHandler>>(() => Factory.ActualFactory.GetRequestHandlers());

    public static List<RequestHandler> Handlers
    {
      get
      {
        return _handlers.Value;
      }
    }

    public static void PostInit()
    {
      foreach (RequestHandler requestHandler in Handlers)
      {
        requestHandler.PostInit();
      }
    }

    public static void PreInit()
    {
      foreach (RequestHandler requestHandler in Handlers)
      {
        requestHandler.PreInit();
      }
    }

    public static bool TryToResolveHandler(ProcessorArgs pipelineArgs, out RequestHandler handler)
    {
      foreach (var requestHandler in Handlers)
      {
        var handlerMatch = requestHandler.Match(pipelineArgs);
        if (handlerMatch == true)
        {
          handler = requestHandler;
          return true;
        }
        if (handlerMatch == null)
        {
          break;
        }
      }
      handler = null;
      return false;
    }
  }
}