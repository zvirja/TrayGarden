using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClipboardChangerPlant.Configuration;
using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling
{
  [UsedImplicitly]
  public class ReqHandlerResolver
  {
    private static readonly Lazy<List<RequestHandler>> _handlers =
        new Lazy<List<RequestHandler>>(() => Factory.ActualFactory.GetRequestHandlers());

    public static List<RequestHandler> Handlers
    {
      get { return _handlers.Value; }
    }

    public static bool TryToResolveHandler(string valueToHandle, out RequestHandler handler)
    {
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
  }
}
