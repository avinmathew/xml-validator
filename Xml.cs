using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace XmlValidator
{
    public class Xml
    {
        public static string Validate(string xsdPath, bool recurseXsdPath, string xmlPath, bool recurseXmlPath, bool includeWarnings)
        {
            if (!Directory.Exists(xsdPath))
                return "Invalid XSD path: " + xsdPath;
            if (!Directory.Exists(xmlPath))
                return "Invalid XML path: " + xmlPath;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            if (includeWarnings)
            {
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            }
            List<ValidationEventArgs> validationEvents = new List<ValidationEventArgs>();
            settings.ValidationEventHandler += ((s, e) => validationEvents.Add(e));

            StringBuilder sb = new StringBuilder();

            foreach (string file in Directory.EnumerateFiles(xsdPath, "*.xsd", recurseXsdPath ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                try
                {
                    settings.Schemas.Add(null, XmlReader.Create(file));
                }
                catch (XmlSchemaException ex)
                {
                    if (sb.Length > 0)
                        sb.AppendLine(); // Add empty line to separate validation errors from different files
                    sb.AppendLine("XML file: " + file);
                    sb.AppendLine("Line Number: " + ex.LineNumber);
                    sb.AppendLine("Line Position: " + ex.LinePosition);
                    sb.AppendLine("Message: " + ex.Message);
                }
            }

            foreach (string file in Directory.EnumerateFiles(xmlPath, "*.xml", recurseXmlPath ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                validationEvents.Clear();
                try
                {
                    using (XmlReader reader = XmlReader.Create(file, settings))
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(reader);
                    }
                }
                catch (XmlException ex)
                {
                    if (sb.Length > 0)
                        sb.AppendLine(); // Add empty line to separate validation errors from different files
                    sb.AppendLine("XML file: " + file);
                    sb.AppendLine("Line Number: " + ex.LineNumber);
                    sb.AppendLine("Line Position: " + ex.LinePosition);
                    sb.AppendLine("Message: " + ex.Message);
                }
                catch (XmlSchemaValidationException ex)
                {
                    if (sb.Length > 0)
                        sb.AppendLine(); // Add empty line to separate validation errors from different files
                    sb.AppendLine("XML file: " + file);
                    sb.AppendLine("Line Number: " + ex.LineNumber);
                    sb.AppendLine("Line Position: " + ex.LinePosition);
                    sb.AppendLine("Message: " + ex.Message);
                }
                if (validationEvents.Any())
                {
                    if (sb.Length > 0)
                        sb.AppendLine(); // Add empty line to separate validation errors from different files
                    sb.AppendLine("XML file: " + file);
                    foreach (ValidationEventArgs args in validationEvents)
                    {
                        sb.AppendLine("Severity: " + args.Severity);
                        sb.AppendLine("Line Number: " + args.Exception.LineNumber);
                        sb.AppendLine("Line Position: " + args.Exception.LinePosition);
                        sb.AppendLine("Message: " + args.Message);
                    }
                }
            }
            return sb.Length == 0 ? "All files valid." : sb.ToString();
        }
    }
}
