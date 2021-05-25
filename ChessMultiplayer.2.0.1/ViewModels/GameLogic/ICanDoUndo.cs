using System;
using System.Collections.Generic;
using System.Text;
using ChessMultiplayer.Models;

namespace ChessMultiplayer.ViewModels.GameLogic
{
    public interface ICanDoUndo
    {
        void Do(MoveParameters parameters);
        void Undo(MoveParameters parameters);
    }
}
