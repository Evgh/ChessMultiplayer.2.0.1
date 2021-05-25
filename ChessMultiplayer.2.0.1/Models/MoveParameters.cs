using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMultiplayer.Models
{
    public class MoveParameters 
    {
        public int Id { get; set; }

        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int TargetRow { get; set; }
        public int TargetColumn { get; set; }
        public bool DidSomeoneDied { get; set; }

        public int ActionNum { get; set; }

        public int GameID { get; set; }
        public Game Game {get; set;}

        public override string ToString()
        {
            if (ActionNum == 0)
                return "Начальная позиция";
            else
                return $"{(char)(65+StartColumn)}{8 - StartRow} {(char)(65 + TargetColumn)}{8 - TargetRow}";
        }

        public bool IsSameMove(MoveParameters par)
        {
            return StartRow == par.StartRow &&
                   StartColumn == par.StartColumn &&
                   TargetRow == par.TargetRow &&
                   TargetColumn == par.TargetColumn &&
                   DidSomeoneDied == par.DidSomeoneDied &&
                   ActionNum == par.ActionNum; 
        }
    }
}
