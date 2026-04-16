using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Tracker.Models
{
    public class MovieHistory : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        public DateTime AddedAt { get; set; }
        public bool IsWatched { get; set; }

        // А ось поле DeletedAt ми робимо "розумним"
        private DateTime? _deletedAt;
        public DateTime? DeletedAt
        {
            get { return _deletedAt; }
            set
            {
                _deletedAt = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
