using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

using TrayGarden.Configuration.ModernFactoryStuff.Parcers;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners;

public class MethodAssigner : IContentAssigner
{
  public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType, Func<Type, IParcer> valueParcerResolver)
  {
    var methodInfo = ResolveMethodInfo(contentNode, instance, instanceType);
    if (methodInfo == null)
    {
      return;
    }
    Type[] methodArgTypes = methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();
    object[] methodArgs = GetArgValues(contentNode, methodArgTypes, valueParcerResolver);
    InvokeMethod(methodInfo, methodArgs, instance);
  }

  protected virtual Type[] GetArgTypes(XmlNode contentNode)
  {
    string argTypes = XmlHelper.GetAttributeValue(contentNode, "argTypes");
    if (argTypes.IsNullOrEmpty())
    {
      return null;
    }
    string[] typesStrArray = argTypes.Split(new string[] { "|" }, StringSplitOptions.None);
    var types = new List<Type>();
    foreach (string typeStr in typesStrArray)
    {
      var resolvedType = ReflectionHelper.ResolveType(typeStr);
      if (resolvedType == null)
      {
        return null;
      }
      types.Add(resolvedType);
    }
    return types.ToArray();
  }

  protected virtual object[] GetArgValues(XmlNode contentNode, Type[] argTypes, Func<Type, IParcer> valueParcerResolver)
  {
    if (contentNode.ChildNodes.Count < argTypes.Length)
    {
      return null;
    }
    XmlNodeList xmlChildNodes = contentNode.ChildNodes;
    var result = new object[argTypes.Length];
    for (int i = 0; i < argTypes.Length; i++)
    {
      Type currentType = argTypes[i];
      IParcer parcer = valueParcerResolver(currentType);
      result[i] = parcer.ParceNodeValue(xmlChildNodes[i]);
    }
    return result;
  }

  protected virtual void InvokeMethod(MethodInfo methodInfo, object[] args, object instance)
  {
    try
    {
      methodInfo.Invoke(instance, args);
    }
    catch (Exception ex)
    {
      Log.Error(
        "Can't properly assign content by method call. Instance: {0}, Method: {1}, Params: {2}".FormatWith(
          instance,
          methodInfo.Name,
          string.Join(",", args.Select(x => x ?? "null"))),
        ex,
        this);
    }
  }

  protected virtual MethodInfo ResolveMethodInfo(XmlNode contentNode, object instance, Type instanceType)
  {
    string nodeName = contentNode.Name;
    var methodParamsTypes = GetArgTypes(contentNode);
    MethodInfo resolvedMI = null;
    if (methodParamsTypes != null)
    {
      if (contentNode.ChildNodes.Count >= methodParamsTypes.Length)
      {
        resolvedMI = instanceType.GetMethod(nodeName, methodParamsTypes);
      }
    }
    else
    {
      var paramsCount = contentNode.ChildNodes.Count;
      var allMethodInfos = instanceType.GetMethods();
      foreach (MethodInfo methodInfo in allMethodInfos)
      {
        if (methodInfo.Name.Equals(nodeName, StringComparison.OrdinalIgnoreCase) && methodInfo.GetParameters().Length == paramsCount)
        {
          resolvedMI = methodInfo;
          break;
        }
      }
    }
    return resolvedMI;
  }
}