using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling.PipelineModel;

namespace ClipboardChangerPlant.RequestHandling
{
  public class ProcessManager : INeedCongurationNode
  {
    private static readonly Lazy<ProcessManager> _actualProcessManager = new Lazy<ProcessManager>(() => Factory.ActualFactory.GetRequestProcessManager());
    public static ProcessManager ActualManager
    {
      get { return _actualProcessManager.Value; }
    }

    protected XmlHelper ConfigurationHelper;
    protected List<Processor> Processors;

    public virtual void ProcessRequest()
    {
      try
      {
        var processorsArgs = new ProcessorArgs();
        foreach (var processor in Processors)
        {
          processor.Process(processorsArgs);
          if (processorsArgs.Abort)
            break;
        }
      }
      catch
      {
        var notifyManager = Factory.ActualFactory.GetNotifyIconManager();
        notifyManager.SetNewIcon(notifyManager.DefaultTrayIcon, 1);
      }
    }

    public virtual void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
      var processors = Factory.ActualFactory.RawFactory.GetObjectsCollectionFromConfigurationNode<Processor>(
         configurationNode.Name + "/pipeline/processor");
      Processors = processors;
    }

    public string Name { get; set; }
  }
}
