#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling.PipelineModel;

using JetBrains.Annotations;

#endregion

namespace ClipboardChangerPlant.RequestHandling
{
  [UsedImplicitly]
  public class ProcessManager : INeedCongurationNode
  {
    #region Static Fields

    private static readonly Lazy<ProcessManager> _actualProcessManager =
      new Lazy<ProcessManager>(() => Factory.ActualFactory.GetRequestProcessManager());

    #endregion

    #region Fields

    protected XmlHelper ConfigurationHelper;

    protected List<Processor> Processors;

    #endregion

    #region Public Properties

    public static ProcessManager ActualManager
    {
      get
      {
        return _actualProcessManager.Value;
      }
    }

    public string Name { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual void ProcessRequest(
      bool onlyShorteningRequired,
      bool clipboardEvent,
      string predefinedClipboardValue,
      bool globalIconIsOriginator)
    {
      try
      {
        var processorsArgs = new ProcessorArgs(onlyShorteningRequired, clipboardEvent, predefinedClipboardValue, globalIconIsOriginator);
        foreach (var processor in this.Processors)
        {
          processor.Process(processorsArgs);
          if (processorsArgs.Aborted)
          {
            break;
          }
        }
      }
      catch
      {
        var notifyManager = Factory.ActualFactory.GetNotifyIconManager();
        notifyManager.SetNewIcon(notifyManager.ErrorTrayIcon, 250);
      }
    }

    public virtual void SetConfigurationNode(XmlNode configurationNode)
    {
      this.ConfigurationHelper = new XmlHelper(configurationNode);
      var processors =
        Factory.ActualFactory.RawFactory.GetObjectsCollectionFromConfigurationNode<Processor>(
          configurationNode.Name + "/pipeline/processor");
      this.Processors = processors;
    }

    #endregion
  }
}