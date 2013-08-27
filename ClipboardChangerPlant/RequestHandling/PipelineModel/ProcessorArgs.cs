using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class ProcessorArgs
  {
    public bool Abort { get; set; }
    public string ResultUrl { get; set; }
    public bool ShouldBeShorted { get; set; }
    public RequestHandler ResolvedHandler { get; set; }

    public ProcessorArgs() { }

    public ProcessorArgs(string resultUrl)
    {
      ResultUrl = resultUrl;
    }
  }
}
