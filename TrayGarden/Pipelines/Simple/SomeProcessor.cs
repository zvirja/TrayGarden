using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Pipelines.Simple
{
    public class SomeProcessor
    {
        public void Process(SomeArgs args)
        {
            var res = args.Name + "22";
            args.Result = res;
        }
    }
}