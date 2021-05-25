using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ChessMultiplayer.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }

        public ICollection<Game> Games;

        public User()
        {
            Games = new ObservableCollection<Game>();
        }

        public bool IsNull() => Id == null;
    }
}
