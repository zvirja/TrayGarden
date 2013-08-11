using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
    [UsedImplicitly]
    public class AssignVisibleText
    {

        public string GlobalTitle { get; set; }
        public string Header { get; set; }
        public string ShortName { get; set; }
        public string ConfigurationDescription { get; set; }

        public AssignVisibleText()
        {
            GlobalTitle = "Tray Garden -- Application configuration";
            Header = "Application configuration";
            ShortName = "app config";
            ConfigurationDescription =
                "Here you may configure global application settings. Some settings may require reboot to be applied";
        }


        [UsedImplicitly]
        public virtual void Process(GetApplicationConfigStepArgs args)
        {
            args.ConfigurationConstructInfo.ConfigurationDescription = ConfigurationDescription;
            var stepInfo = args.StepConstructInfo;
            stepInfo.GlobalTitle = GlobalTitle;
            stepInfo.Header = Header;
            stepInfo.ShortName = ShortName;
        }
    }
}
