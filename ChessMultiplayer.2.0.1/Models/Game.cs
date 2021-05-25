using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMultiplayer.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime HappenedAt { get; set; }

        public string UserID { get; set; }
        public User User { get; set; }

        public ICollection<MoveParameters> Moves { get; set; }
        public Game()
        {
            Moves = new List<MoveParameters>();
        }

        public override string ToString()
        {
            return $"Игра от {HappenedAt}";
        }
    }
}
