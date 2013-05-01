using System;
using System.Reflection;
using System.Xml;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    public class Processor
    {
        protected Delegate Invoker { get; set; }

        public virtual bool Initialize(object processorObject, Type argumentType)
        {
            Invoker = ResolveInvoker(processorObject, argumentType);
            if (Invoker == null)
                return false;
            return true;
        }

        protected virtual Delegate ResolveInvoker(object processorObject, Type argumentType)
        {
            Type processorObjType = processorObject.GetType();
            MethodInfo processMethod = processorObjType.GetMethod("Process");
            if (!ValidateProcessorObj(processMethod, argumentType))
                return null;
            Type generalProcessInvokerType = typeof (Action<>);
            Type specificProcessInvokerType = generalProcessInvokerType.MakeGenericType(new[] {argumentType});
            Delegate invoker = Delegate.CreateDelegate(specificProcessInvokerType, processorObject, processMethod);
            return invoker;
        }

        protected virtual bool ValidateProcessorObj(MethodInfo processMI, Type argumentType)
        {
            ParameterInfo[] processParams = processMI.GetParameters();
            if (processParams.Length != 1)
                return false;
            Type firstParamType = processParams[0].ParameterType;
            return firstParamType == argumentType;
        }

        public virtual void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs
        {
            if (Invoker == null)
                return;
            var castedInvoker = (Action<TArgumentType>) Invoker;
            castedInvoker(argument);
        }
    }
}