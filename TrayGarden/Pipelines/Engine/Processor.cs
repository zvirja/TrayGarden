using System;
using System.Reflection;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    [UsedImplicitly]
    public class Processor
    {
        protected Delegate Invoker { get; set; }
        protected bool Initialized { get; set; }

        [UsedImplicitly]
        public virtual bool Initialize([NotNull] object processorObject, [NotNull] string argumentTypeStr)
        {
            Assert.ArgumentNotNull(processorObject, "processorObject");
            Assert.ArgumentNotNullOrEmpty(argumentTypeStr, "argumentTypeStr");
            var argumentType = ReflectionHelper.ResolveType(argumentTypeStr);
            Assert.IsNotNull(argumentType, "Processor initialization: Invalid type {0}".FormatWith(argumentTypeStr));
            Invoker = ResolveInvoker(processorObject, argumentType);
            if (Invoker == null)
            {
                Log.Warn("Can't initialize processor {0}".FormatWith(processorObject.GetType().FullName), this);
                return false;
            }
            Initialized = true;
            return true;
        }

        public virtual void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs
        {
            if (!Initialized)
                throw new NonInitializedException();
            if (Invoker == null)
                return;
            var castedInvoker = (Action<TArgumentType>)Invoker;
            castedInvoker(argument);
        }

        protected virtual Delegate ResolveInvoker(object processorObject, Type argumentType)
        {
            Type processorObjType = processorObject.GetType();
            MethodInfo processMethod = processorObjType.GetMethod("Process");
            if (!ValidateProcessorObj(processMethod, argumentType))
            {
                Log.Warn(
                    "The processor object {0} doesn't contain valid process method{{Process({1}) expected }}".FormatWith
                        (processMethod.GetType().FullName, argumentType.FullName), this);
                return null;
            }
            Type generalProcessInvokerType = typeof(Action<>);
            Type specificProcessInvokerType = generalProcessInvokerType.MakeGenericType(new[] { argumentType });
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

        public override string ToString()
        {
            return !Initialized ? base.ToString() : "Processor. Executable type: {0}".FormatWith(Invoker.Target.GetType().FullName);
        }
    }
}