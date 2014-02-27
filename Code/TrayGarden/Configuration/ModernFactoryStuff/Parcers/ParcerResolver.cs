#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
  public class ParcerResolver
  {
    #region Constructors and Destructors

    public ParcerResolver(ModernFactory factory)
    {
      this.Parcers = new Dictionary<Type, IParcer>();
      this.OwningFactory = factory;
    }

    #endregion

    #region Properties

    protected ModernFactory OwningFactory { get; set; }

    protected Dictionary<Type, IParcer> Parcers { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual IParcer GetParcer(Type type)
    {
      if (this.Parcers.ContainsKey(type))
      {
        return this.Parcers[type];
      }
      var parcer = this.ResolveParcer(type);
      this.Parcers[type] = parcer;
      return parcer;
    }

    #endregion

    #region Methods

    protected virtual IParcer ResolveParcer(Type type)
    {
      if (type == typeof(string))
      {
        return new StringParcer();
      }
      if (type == typeof(bool))
      {
        return new BoolParcer();
      }
      if (type == typeof(int))
      {
        return new IntParcer();
      }
      return new ObjectParcer(this.OwningFactory);
    }

    #endregion
  }
}