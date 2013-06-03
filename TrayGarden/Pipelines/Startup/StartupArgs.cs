using System.Windows;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Pipelines.Startup
{
    public class StartupArgs:PipelineArgs
    {
        public Application App
        {
            get { return Application.Current; }
        }
    }
}
