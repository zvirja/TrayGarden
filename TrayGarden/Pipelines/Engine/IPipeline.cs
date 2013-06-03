using System;

namespace TrayGarden.Pipelines.Engine
{
    public interface IPipeline
    {
        Type ArgumentType { get; }
        string Name { get; }
        void Invoke<TArgumentType>(TArgumentType argument, bool maskExceptions) where TArgumentType : PipelineArgs;
    }
}