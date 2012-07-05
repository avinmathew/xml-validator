using System;
using System.IO;

namespace XmlValidator
{
    public class SettingsIO
    {
        public static Settings ReadSettings(string file)
        {
            Settings settings = new Settings();
            if (File.Exists(file))
            {
                try
                {
                    using (TextReader reader = File.OpenText(file))
                    {
                        string xsdPath = reader.ReadLine();
                        if (Directory.Exists(xsdPath))
                            settings.XsdPath = xsdPath;

                        settings.RecurseXsd = bool.Parse(reader.ReadLine());

                        string xmlPath = reader.ReadLine();
                        if (Directory.Exists(xmlPath))
                            settings.XmlPath = xmlPath;

                        settings.RecurseXml = bool.Parse(reader.ReadLine());

                        settings.IncludeWarnings = bool.Parse(reader.ReadLine());

                        settings.Height = double.Parse(reader.ReadLine());
                        settings.Width = double.Parse(reader.ReadLine());
                        settings.Left = double.Parse(reader.ReadLine());
                        settings.Top = double.Parse(reader.ReadLine());
                    }
                }
                catch (Exception) // An error at point in file terminates reading further settings
                {
                    settings.ReadError = true;
                }
                return settings;
            }
            return null;
        }

        public static void WriteSettings(string file, Settings settings)
        {
            using (TextWriter writer = File.CreateText(file))
            {
                writer.WriteLine(settings.XsdPath);
                writer.WriteLine(settings.RecurseXsd);
                writer.WriteLine(settings.XmlPath);
                writer.WriteLine(settings.RecurseXml);
                writer.WriteLine(settings.IncludeWarnings);
                writer.WriteLine(settings.Height);
                writer.WriteLine(settings.Width);
                writer.WriteLine(settings.Left);
                writer.WriteLine(settings.Top);
            }
        }
    }
}
