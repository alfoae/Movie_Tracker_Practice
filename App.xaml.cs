using Movie_Tracker.Models;
using System.Collections.ObjectModel;
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
        public static ObservableCollection<MovieHistory> HistoryProvider { get; set; } = new ObservableCollection<MovieHistory>();

        private const string SettingsFile = "settings.json";
        private const string HistoryFile = "history.json";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadSettings();
            LoadHistory();
        }

        public static void SaveHistory()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(HistoryProvider, options);
                File.WriteAllText(HistoryFile, json);
            }
            catch { }
        }

        private void LoadHistory()
        {
            if (File.Exists(HistoryFile))
            {
                try
                {
                    string json = File.ReadAllText(HistoryFile);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var data = JsonSerializer.Deserialize<ObservableCollection<MovieHistory>>(json);
                        if (data != null) HistoryProvider = data;
                    }
                }
                catch { HistoryProvider = new ObservableCollection<MovieHistory>(); }
            }
        }

        public void ApplySettings(string lang, string theme)
        {
            this.Resources.MergedDictionaries.Clear();
            var langDict = new ResourceDictionary { Source = new Uri($"/Resources/Locales/Lang{lang}.xaml", UriKind.Relative) };
            this.Resources.MergedDictionaries.Add(langDict);
            var themeDict = new ResourceDictionary { Source = new Uri($"/Resources/{theme}Styles.xaml", UriKind.Relative) };
            this.Resources.MergedDictionaries.Add(themeDict);

            var settings = new Models.AppSettings { Language = lang, Theme = theme };
            File.WriteAllText(SettingsFile, JsonSerializer.Serialize(settings));
        }

        private void LoadSettings()
        {
            string lang = "UA", theme = "Light";
            if (File.Exists(SettingsFile))
            {
                var settings = JsonSerializer.Deserialize<Models.AppSettings>(File.ReadAllText(SettingsFile));
                lang = settings?.Language ?? "UA";
                theme = settings?.Theme ?? "Light";
            }
            ApplySettings(lang, theme);
        }
    }

}
