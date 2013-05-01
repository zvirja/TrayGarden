namespace TrayGarden.Pipelines.Engine
{
    public class PipelineArgs
    {
        public bool Aborted { get; protected set; }
        public object Result { get; set; }


        public virtual void Abort()
        {
            Aborted = true;
        }
    }
}