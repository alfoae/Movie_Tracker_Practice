using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Movie_Tracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string SettingsFile = "settings.json";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadSettings();
        }

        public void ApplySettings(string lang, string theme)
        {
            this.Resources.MergedDictionaries.Clear();

            var langDict = new ResourceDictionary();
            langDict.Source = new Uri($"/Resources/Locales/Lang{lang}.xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(langDict);

            var themeDict = new ResourceDictionary();

            themeDict.Source = new Uri($"/Resources/{theme}Styles.xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(themeDict);

            var settings = new Models.AppSettings { Language = lang, Theme = theme };
            File.WriteAllText(SettingsFile, JsonSerializer.Serialize(settings));
        }

        private void LoadSettings()
        {
            string lang = "UA";
            string theme = "Light";

            if (File.Exists(SettingsFile))
            {
                var settings = JsonSerializer.Deserialize<Models.AppSettings>(File.ReadAllText(SettingsFile));
                lang = settings.Language;
                theme = settings.Theme;
            }

            ApplySettings(lang, theme);
        }
    }

}
