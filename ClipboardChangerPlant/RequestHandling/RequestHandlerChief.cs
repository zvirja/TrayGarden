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
      get { return _handlers.Value; }
    }

    public static bool TryToResolveHandler(ProcessorArgs pipelineArgs, out RequestHandler handler)
    {
      string valueToHandle = pipelineArgs.ResultUrl;
      foreach (var requestHandler in Handlers)
      {
        if (requestHandler.Match(valueToHandle))
        {
          handler = requestHandler;
          return true;
        }
      }
      handler = null;
      return false;
    }

    public static void PreInit()
    {
      foreach (RequestHandler requestHandler in Handlers)
        requestHandler.PreInit();
    }

    public static void PostInit()
    {
      foreach (RequestHandler requestHandler in Handlers)
        requestHandler.PostInit();
    }
  }
}
