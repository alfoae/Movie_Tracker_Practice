using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Movie_Tracker.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void CmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbLanguage.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag.ToString();

                ResourceDictionary dict = new ResourceDictionary();

                switch (tag)
                {
                    case "EN":
                        dict.Source = new Uri("Resources/Locales/LangEN.xaml", UriKind.Relative);
                        break;
                    case "UA":
                    default:
                        dict.Source = new Uri("Resources/Locales/LangUA.xaml", UriKind.Relative);
                        break;
                }

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
