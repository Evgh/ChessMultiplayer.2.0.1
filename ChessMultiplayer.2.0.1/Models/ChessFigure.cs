using System;
using System.Collections.Generic;
using System.Text;

namespace ChessMultiplayer.Models
{
    class ChessFigure : IFigure
    {
        public ChessFigure()
        {

        }
        public ChessFigure(FigureType type, FigureColor color)
        {
            this.color = color;
            this.type = type;
        }

        FigureColor color;
        public virtual FigureColor FigureColor => color;

        FigureType type;
        public virtual FigureType FigureType => type;

        public virtual string ImagePath => FigureType.ToString().ToLower() + FigureColor.ToString()[0] + ".png";

        public virtual bool CanMove(int startRow, int startCol, int targetRow, int targetCol)
        {
            int rowDelta = startRow - targetRow;
            int colDelta = startCol - targetCol;

            if (rowDelta <= 2 && rowDelta >= -2 && colDelta <= 2 && colDelta >= -2)
                return true;
            else
                return false;
        }
    }
}
