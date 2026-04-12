using Movie_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Movie_Tracker.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private bool _isLoaded = false;

        public SettingsPage()
        {
            InitializeComponent();
            LoadCurrentSettingsToUI();
            _isLoaded = true; // Тепер ми готові відстежувати зміни
        }

        private void LoadCurrentSettingsToUI()
        {
            if (File.Exists("settings.json"))
            {
                var json = File.ReadAllText("settings.json");
                var settings = JsonSerializer.Deserialize<AppSettings>(json);

                // Виставляємо мову в ComboBox по Tag
                foreach (ComboBoxItem item in CmbLanguage.Items)
                {
                    if (item.Tag.ToString() == settings.Language)
                    {
                        CmbLanguage.SelectedItem = item;
                        break;
                    }
                }

                // Виставляємо тему в ComboBox по Tag
                foreach (ComboBoxItem item in CmbTheme.Items)
                {
                    if (item.Tag.ToString() == settings.Theme)
                    {
                        CmbTheme.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // ОСЬ ЦЕЙ МЕТОД, ЯКОГО ТОБІ НЕ ВИСТАЧАЛО:
        private void SettingChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;

            // Отримуємо вибрані об'єкти (ComboBoxItem)
            var selectedLang = CmbLanguage.SelectedItem as ComboBoxItem;
            var selectedTheme = CmbTheme.SelectedItem as ComboBoxItem;

            if (selectedLang != null && selectedTheme != null)
            {
                string lang = selectedLang.Tag.ToString();   // Буде "UA" або "EN"
                string theme = selectedTheme.Tag.ToString(); // Буде "Light" або "Dark"

                // Застосовуємо налаштування через App.xaml.cs
                ((App)Application.Current).ApplySettings(lang, theme);
            }
        }
    }
}
