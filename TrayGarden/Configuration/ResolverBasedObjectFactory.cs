using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration
{
    public class ResolverBasedObjectFactory : IObjectFactory
    {
        public Func<object> GetInstanceResolver { get; protected set; }
        public Func<object> GetPurelyNewInstanceResolver { get; protected set; }

        public ResolverBasedObjectFactory(Func<object> getInstanceResolver, Func<object> getPurelyNewInstanceResolver)
        {
            GetInstanceResolver = getInstanceResolver;
            GetPurelyNewInstanceResolver = getPurelyNewInstanceResolver;
        }

        public virtual object GetObject()
        {
            if (GetInstanceResolver != null)
                return GetInstanceResolver();
            return null;
        }

        public virtual object GetPurelyNewObject()
        {
            if (GetPurelyNewInstanceResolver != null)
                return GetPurelyNewInstanceResolver();
            return null;
        }
    }
}
