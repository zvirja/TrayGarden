﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
    public class ParcerResolver
    {
        protected Dictionary<Type, IParcer> Parcers { get; set; }
        protected ModernFactory OwningFactory { get; set; }

        public ParcerResolver(ModernFactory factory)
        {
            Parcers = new Dictionary<Type, IParcer>();
            OwningFactory = factory;
        }

        public virtual IParcer GetParcer(Type type)
        {
            if (Parcers.ContainsKey(type))
                return Parcers[type];
            var parcer = ResolveParcer(type);
            Parcers[type] = parcer;
            return parcer;
        }

        protected virtual IParcer ResolveParcer(Type type)
        {
            if (type == typeof(string))
                return new StringParcer();
            if (type == typeof(bool))
                return new BoolParcer();
            if (type == typeof(int))
                return new IntParcer();
            return new ObjectParcer(OwningFactory);
        }

    }
}