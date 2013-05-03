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

        public virtual IContentAssigner GetAssigner(string hintValue)
        {
            var uppercased = hintValue.ToUpperInvariant();
            if (Assigners.ContainsKey(uppercased))
                return Assigners[uppercased];
            IContentAssigner assigner = ResolveAssigner(uppercased);
            Assigners[uppercased] = assigner;
            return assigner;
        }

        protected virtual IContentAssigner ResolveAssigner(string hintValue)
        {
            switch (hintValue)
            {
                case "INVOKE":
                    return new MethodAssigner();
                case "ADDLIST":
                    return new AddListAssigner();
                case "NEWLIST":
                    return new NewListAssigner();
                default:
                    return new SimpleAssigner();
            }
        }


    }
}
