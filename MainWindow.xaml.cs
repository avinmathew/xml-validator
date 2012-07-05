using System.IO;
using System.Windows;
using System.Windows.Input;

namespace XmlValidator
{
    public partial class MainWindow : Window
    {
        private const string SETTINGS_FILE = "XmlValidator.ini";

        public MainWindow()
        {
            InitializeComponent();
            ReadSettings();
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            ValidateXML();
            Cursor = Cursors.Arrow;
        }

        private void ValidateXML()
        {
            txtOutput.Text = Xml.Validate(txtXsdPath.Text, chkRecurseXsd.IsChecked.Value, txtXmlPath.Text, chkRecurseXsd.IsChecked.Value, chkIncludeWarnings.IsChecked.Value);
        }

        private void ReadSettings()
        {
            Settings settings = SettingsIO.ReadSettings(SETTINGS_FILE);
            if (settings != null)
            {
                txtXsdPath.Text = settings.XsdPath;
                chkRecurseXsd.IsChecked = settings.RecurseXsd;
                txtXmlPath.Text = settings.XmlPath;
                chkRecurseXml.IsChecked = settings.RecurseXml;
                chkIncludeWarnings.IsChecked = settings.IncludeWarnings;
                if (settings.Height.HasValue) this.Height = settings.Height.Value;
                if (settings.Width.HasValue) this.Width = settings.Width.Value;
                if (settings.Left.HasValue) this.Left = settings.Left.Value;
                if (settings.Top.HasValue) this.Top = settings.Top.Value;
            }
        }

        private void WriteSettings()
        {
            Settings settings = new Settings
            {
                XsdPath = txtXsdPath.Text,
                RecurseXsd = chkRecurseXsd.IsChecked,
                XmlPath = txtXmlPath.Text,
                RecurseXml = chkRecurseXml.IsChecked,
                IncludeWarnings = chkIncludeWarnings.IsChecked,
                Height = this.Height,
                Width = this.Width,
                Left = this.Left,
                Top = this.Top
            };
            SettingsIO.WriteSettings(SETTINGS_FILE, settings);
        }

        private void Path_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btnValidate.IsEnabled = Directory.Exists(txtXsdPath.Text) && Directory.Exists(txtXmlPath.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WriteSettings();
        }
    }
}
