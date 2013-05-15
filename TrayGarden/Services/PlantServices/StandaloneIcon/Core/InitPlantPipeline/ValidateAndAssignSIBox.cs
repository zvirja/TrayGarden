﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class ValidateAndAssignSIBox
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantSIArgs args)
        {
            if (!IsSIBoxValid(args.SIBox))
            {
                args.Abort();
                return;
            }
            args.SIBox.Plant = args.Plant;
            args.Plant.PutLuggage(args.LuggageName,args.SIBox);
        }

        protected virtual bool IsSIBoxValid(StandaloneIconPlantBox box)
        {
            if (box == null)
                return false;
            if (box.SettingsBox == null)
                return false;
            if (box.NotifyIcon == null)
                return false;
            return true;
        }
    }
}