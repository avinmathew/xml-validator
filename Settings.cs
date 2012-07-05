
namespace XmlValidator
{
    public class Settings
    {
        public string XsdPath { get; set; }
        public bool? RecurseXsd { get; set; }
        public string XmlPath { get; set; }
        public bool? RecurseXml { get; set; }
        public bool? IncludeWarnings { get; set; }

        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

        public bool ReadError { get; set; }
    }
}
