using System.Xml;

namespace TrayGarden.Configuration
{
    public class SectionHandler : System.Configuration.ConfigurationSection
    {
        public XmlDocument XmlRepresentation { get; set; }

        protected override void DeserializeSection(XmlReader reader)
        {
            var document = new XmlDocument();
            document.Load(reader);
            XmlRepresentation = document;
        }
    }
}