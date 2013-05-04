using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
    public class ContentAssignersResolver
    {
        protected Dictionary<string, IContentAssigner> Assigners { get; set; }

        public ContentAssignersResolver()
        {
            Assigners = new Dictionary<string, IContentAssigner>();
        }

        public virtual IContentAssigner GetPropertyAssigner(string hintValue)
        {
            var uppercased = hintValue.ToUpperInvariant();
            var key = "P:" + uppercased;
            if (Assigners.ContainsKey(key))
                return Assigners[key];
            IContentAssigner assigner = ResolvePropertyAssigner(uppercased);
            Assigners[key] = assigner;
            return assigner;
        }

        public virtual IContentAssigner GetDirectAssigner(string hintValue)
        {
            var uppercased = hintValue.ToUpperInvariant();
            var key = "D:" + uppercased;
            if (Assigners.ContainsKey(key))
                return Assigners[key];
            IContentAssigner assigner = ResolveDirectAssigner(uppercased);
            Assigners[key] = assigner;
            return assigner;
        }

        protected virtual IContentAssigner ResolvePropertyAssigner(string hintValue)
        {
            switch (hintValue)
            {
                case "INVOKE":
                    return new MethodAssigner();
                case "ADDLIST":
                    return new AddListAssigner();
                default:
                    return new SimpleAssigner();
            }
        }

        protected virtual IContentAssigner ResolveDirectAssigner(string hintValue)
        {
            switch (hintValue)
            {
                case "NEWLIST":
                    return new NewListDirectAssigner();
                default:
                    return null;
            }
        }


    }
}
