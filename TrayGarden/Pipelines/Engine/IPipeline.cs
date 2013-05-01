using System;

namespace TrayGarden.Pipelines.Engine
{
    public interface IPipeline
    {
        Type ArgumentType { get; set; }
        string Name { get; set; }
        void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs;
    }
}