namespace TrayGarden.Configuration
{
    public class Factory
    {
        public static ConfigurationBasedFactory ActualFactory
        {
            get { return ConfigurationBasedFactory.ActualFactory; }
        }

        public static ConfigurationBasedFactoryRaw RawFactory
        {
            get { return ConfigurationBasedFactoryRaw.ActualFactory; }
        }
    }
}
