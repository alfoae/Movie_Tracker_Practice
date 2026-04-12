using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public bool IsWatched { get; set; }

        public Movie() { }

        public Movie(string title, int year, string genre, bool isWatched = false)
        {
            Title = title;
            Year = year;
            Genre = genre;
            IsWatched = isWatched;
        }
    }
}