#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
  public class ContentAssignersResolver
  {
    #region Constructors and Destructors

    public ContentAssignersResolver()
    {
      this.Assigners = new Dictionary<string, IContentAssigner>();
    }

    #endregion

    #region Properties

    protected Dictionary<string, IContentAssigner> Assigners { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual IContentAssigner GetDirectAssigner(string hintValue)
    {
      var uppercased = hintValue.ToUpperInvariant();
      var key = "D:" + uppercased;
      if (this.Assigners.ContainsKey(key))
      {
        return this.Assigners[key];
      }
      IContentAssigner assigner = this.ResolveDirectAssigner(uppercased);
      this.Assigners[key] = assigner;
      return assigner;
    }

    public virtual IContentAssigner GetPropertyAssigner(string hintValue)
    {
      var uppercased = hintValue.ToUpperInvariant();
      var key = "P:" + uppercased;
      if (this.Assigners.ContainsKey(key))
      {
        return this.Assigners[key];
      }
      IContentAssigner assigner = this.ResolvePropertyAssigner(uppercased);
      this.Assigners[key] = assigner;
      return assigner;
    }

    #endregion

    #region Methods

    protected virtual IContentAssigner ResolveDirectAssigner(string hintValue)
    {
      switch (hintValue)
      {
        case "NEWLIST":
          return new NewListDirectAssigner();
        case "OBJECTFACTORY":
          return new ObjectFactoryAssigner();
        default:
          return null;
      }
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

    #endregion
  }
}