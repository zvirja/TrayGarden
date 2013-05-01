namespace TrayGarden.Pipelines.Engine
{
    public interface IPipelineManager
    {
        void InvokePipeline<TArgumentType>(string pipelineName, TArgumentType argument)
            where TArgumentType : PipelineArgs;
    }
}