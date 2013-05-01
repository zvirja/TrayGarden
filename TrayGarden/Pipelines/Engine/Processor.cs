using System;
using System.Reflection;
using System.Xml;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    public class Processor
    {
        protected Delegate Invoker { get; set; }

        public Processor(object processorObject, Type argumentType)
        {
            Invoker = ResolveInvoker(processorObject, argumentType);
        }

        protected virtual Delegate ResolveInvoker(object processorObject, Type argumentType)
        {
            Type processorObjType = processorObject.GetType();
            MethodInfo processMethod = processorObjType.GetMethod("Process");
            Type generalProcessInvokerType = typeof (Action<>);
            Type specificProcessInvokerType = generalProcessInvokerType.MakeGenericType(new[] {argumentType});
            Delegate invoker = Delegate.CreateDelegate(specificProcessInvokerType, processorObject, processMethod);
            return invoker;
        }

        public virtual void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs
        {
            var castedInvoker = (Action<TArgumentType>) Invoker;
            castedInvoker(argument);
        }
    }
}