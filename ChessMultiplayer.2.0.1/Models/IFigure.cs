using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMultiplayer.Models
{
    public enum FigureColor { White, Black };
    public enum FigureType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }
    public interface IFigure
    {
        FigureColor FigureColor { get; }
        FigureType FigureType { get; }        
        string ImagePath { get; }
    }
}
