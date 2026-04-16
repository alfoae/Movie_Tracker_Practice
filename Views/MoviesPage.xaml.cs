using Movie_Tracker.Models;
using MovieTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

namespace Movie_Tracker
{
    /// <summary>
    /// Логика взаимодействия для MoviesPage.xaml
    /// </summary>
    public partial class MoviesPage : Page
    {
        private ObservableCollection<Movie> movies;

        private readonly string dataFile = "movies.json";

        public MoviesPage()
        {
            InitializeComponent();
            LoadData();
            MoviesGrid.ItemsSource = movies;
        }

        private void LoadData()
        {
            if (File.Exists(dataFile))
            {
                string json = File.ReadAllText(dataFile);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    movies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(json);
                }
            }

            if (movies == null)
            {
                movies = new ObservableCollection<Movie>();
                new Movie("Ласкаво просимо", DateTime.Now.Year, "Інфо", false);
            }
        }

        private void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(movies, options);
            File.WriteAllText(dataFile, json);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
            MessageBox.Show("Дані успішно збережено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                MessageBox.Show("Будь ласка, введіть назву фільму!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtYear.Text, out int parsedYear) || parsedYear < 1890 || parsedYear > DateTime.Now.Year)
            {
                MessageBox.Show("Введіть коректний рік!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Movie newMovie = new Movie(TxtTitle.Text, parsedYear, TxtGenre.Text);
            movies.Add(newMovie);

            App.HistoryProvider.Add(new MovieHistory
            {
                Title = newMovie.Title,
                Year = parsedYear,
                Genre = TxtGenre.Text,
                AddedAt = DateTime.Now,
                IsWatched = false
            });

            App.SaveHistory();

            TxtTitle.Clear();
            TxtYear.Clear();
            TxtGenre.Clear();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesGrid.SelectedItem is Movie selectedMovie)
            {
                var hist = App.HistoryProvider.FirstOrDefault(h => h.Title == selectedMovie.Title && h.DeletedAt == null);
                if (hist != null)
                {
                    hist.DeletedAt = DateTime.Now;
                }

                movies.Remove(selectedMovie);
                App.SaveHistory();
            }
            else
            {
                MessageBox.Show("Оберіть фільм для видалення!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = TxtSearch.Text.ToLower();

            ICollectionView view = System.Windows.Data.CollectionViewSource.GetDefaultView(MoviesGrid.ItemsSource);

            if (string.IsNullOrWhiteSpace(filterText))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = item =>
                {
                    Movie movie = item as Movie;
                    if (movie == null) return false;

                    return movie.Title.ToLower().Contains(filterText) || movie.Genre.ToLower().Contains(filterText);

                };
            }
        }



    }
}
