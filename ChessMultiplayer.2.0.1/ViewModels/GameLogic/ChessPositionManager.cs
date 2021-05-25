using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;
using ChessMultiplayer.Models;

namespace ChessMultiplayer.ViewModels.GameLogic
{
    public class ChessPositionsManager : ICanDoUndo
    {
        PositionVM[][] positions;
        PositionVM selectedPosition;

        PositionVM blackKingPosition;
        PositionVM whiteKingPosition;
        ObservableCollection<IFigure> deadWhites;
        ObservableCollection<IFigure> deadBlacks;

        Command pressCommand;

        public ChessPositionsManager(Command pressCommand)
        {
            this.pressCommand = pressCommand;

            DeadBlacks = new ObservableCollection<IFigure>();
            DeadWhites = new ObservableCollection<IFigure>();

            InitializePositions();
            InitializeFigures();
        }

        public PositionVM this[int i, int j]
        {
            get => positions[i][j];
        }

        #region Properties
        public PositionVM SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
            }
        }

        public ObservableCollection<IFigure> DeadWhites
        {
            get => deadWhites;
            set
            {
                deadWhites = value;
            }
        }

        public ObservableCollection<IFigure> DeadBlacks
        {
            get => deadBlacks;
            set
            {
                deadBlacks = value;
            }
        }

        #endregion

        #region ICanDoUndo Logic
        public void Do(MoveParameters moveParameters)
        {
            PositionVM startPosition = positions[moveParameters.StartRow][moveParameters.StartColumn];
            PositionVM targetPosition = positions[moveParameters.TargetRow][moveParameters.TargetColumn];

            if (moveParameters.DidSomeoneDied)
            {
                FigureColor color = targetPosition.Figure.FigureColor;
                bool isWhite = color == FigureColor.White;
                var stack = isWhite ? DeadWhites : DeadBlacks;

                stack.Add(targetPosition.Figure);
            }

            targetPosition.Figure = startPosition.Figure;
            startPosition.Figure = null;

            SelectedPosition = null;
        }

        public void Undo(MoveParameters parameters)
        {
            MoveParameters moveParameters = parameters as MoveParameters;

            PositionVM startPosition = positions[moveParameters.StartRow][moveParameters.StartColumn];
            PositionVM targetPosition = positions[moveParameters.TargetRow][moveParameters.TargetColumn];

            startPosition.Figure = targetPosition.Figure;

            if (moveParameters.DidSomeoneDied)
            {
                bool isWhite = startPosition.Figure.FigureColor == FigureColor.White;
                var deadsCollection = isWhite ? deadBlacks : deadWhites;

                var figure = deadsCollection[deadsCollection.Count - 1];
                deadsCollection.Remove(figure);
                targetPosition.Figure = figure;
            }
            else
            {
                targetPosition.Figure = null;
            }
        }
        #endregion

        #region Initialization
        public void StartNewGame()
        {
            RemoveSelection();
            RemoveFigures();
            DeadBlacks.Clear();
            DeadWhites.Clear();

            InitializeFigures();
        }

        void InitializePositions()
        {
            positions = new PositionVM[8][];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new PositionVM[8];
            }


            for (int i = 0; i < positions.Length; i++)
            {
                for (int j = 0; j < positions.Length; j++)
                {
                    var position = new PositionVM(pressCommand);
                    position.Row = i;
                    position.Column = j;
                    position.State = PositionVM.CheckState.Simple;

                    if ((i + j) % 2 == 0)
                    {
                        position.Type = PositionVM.CheckType.White;
                    }
                    else
                    {
                        position.Type = PositionVM.CheckType.Black;
                    }
                    positions[i][j] = position;
                }
            }
        }

        void InitializeFigures()
        {
            for (int i = 0; i < 8; i++)
            {
                positions[1][i].Figure = new ChessFigure(FigureType.Pawn, FigureColor.Black);
            }
            positions[0][0].Figure = new ChessFigure(FigureType.Rook, FigureColor.Black);
            positions[0][1].Figure = new ChessFigure(FigureType.Knight, FigureColor.Black);
            positions[0][2].Figure = new ChessFigure(FigureType.Bishop, FigureColor.Black);
            positions[0][3].Figure = new ChessFigure(FigureType.Queen, FigureColor.Black);
            positions[0][4].Figure = new ChessFigure(FigureType.King, FigureColor.Black);
            positions[0][5].Figure = new ChessFigure(FigureType.Bishop, FigureColor.Black);
            positions[0][6].Figure = new ChessFigure(FigureType.Knight, FigureColor.Black);
            positions[0][7].Figure = new ChessFigure(FigureType.Rook, FigureColor.Black);


            for (int i = 0; i < 8; i++)
            {
                positions[6][i].Figure = new ChessFigure(FigureType.Pawn, FigureColor.White);
            }
            positions[7][0].Figure = new ChessFigure(FigureType.Rook, FigureColor.White);
            positions[7][1].Figure = new ChessFigure(FigureType.Knight, FigureColor.White);
            positions[7][2].Figure = new ChessFigure(FigureType.Bishop, FigureColor.White);
            positions[7][3].Figure = new ChessFigure(FigureType.Queen, FigureColor.White);
            positions[7][4].Figure = new ChessFigure(FigureType.King, FigureColor.White);
            positions[7][5].Figure = new ChessFigure(FigureType.Bishop, FigureColor.White);
            positions[7][6].Figure = new ChessFigure(FigureType.Knight, FigureColor.White);
            positions[7][7].Figure = new ChessFigure(FigureType.Rook, FigureColor.White);

            whiteKingPosition = positions[7][4];
            blackKingPosition = positions[0][4];
        }

        void RemoveFigures()
        {
            for (var i = 0; i < positions.Length; i++)
            {
                for (var j = 0; j < positions[i].Length; j++)
                {
                    positions[i][j].Figure = null;
                }
            }
        }
        #endregion

        #region Selection Logic
        public void RemoveSelection()
        {
            foreach (var posRow in positions)
            {
                foreach (var pos in posRow)
                {
                    pos.State = PositionVM.CheckState.Simple;
                }
            }
            //if (IsKingReallySafe(FigureColor.White) == false)
            //{
            //    whiteKingPosition.State = PositionVM.CheckState.CanBeat;
            //}

            //if (IsKingReallySafe(FigureColor.Black) == false)
            //{
            //    blackKingPosition.State = PositionVM.CheckState.CanBeat;
            //}

        }

        public void DrawMoveOpportunities()
        {
            switch (SelectedPosition.Figure.FigureType)
            {
                case FigureType.Pawn: PawnMovement(); break;
                case FigureType.Knight: KnightMovement(); break;
                case FigureType.Bishop: BishopMovement(); break;
                case FigureType.Rook: RookMovement(); break;
                case FigureType.Queen: QueenMovement(); break;
                case FigureType.King: KingMovement(); break;
            }
            SelectedPosition.State = PositionVM.CheckState.Selected;
        }

        PositionVM PawnMovement()
        {
            int row = SelectedPosition.Row;
            int col = SelectedPosition.Column;
            // 4 cитуации: два удара по горизонтали, ход вперед, двойной ход вперед
            bool isWhite = SelectedPosition.Figure.FigureColor == FigureColor.White;
            int startRow = isWhite ? 6 : 1;
            int direction = isWhite ? -1 : 1;

            // удар вправо
            if (IsCoordinatesCorrect(row + direction, col + 1))
            {
                if (positions[row + direction][col + 1].Figure != null && IsKingSafe(row + direction, col + 1))
                {
                    if (positions[row + direction][col + 1].Figure.FigureColor != SelectedPosition.Figure.FigureColor)
                        positions[row + direction][col + 1].State = PositionVM.CheckState.CanBeat;
                }
            }

            // удар влево
            if (IsCoordinatesCorrect(row + direction, col - 1))
            {
                if (positions[row + direction][col - 1].Figure != null && IsKingSafe(row + direction, col - 1))
                {
                    if (positions[row + direction][col - 1].Figure.FigureColor != SelectedPosition.Figure.FigureColor)
                        positions[row + direction][col - 1].State = PositionVM.CheckState.CanBeat;
                }
            }

            // обычный ход
            if (IsCoordinatesCorrect(row + direction, col) && IsKingSafe(row + direction, col))
            {
                if (positions[row + direction][col].Figure == null && IsKingSafe(row + direction, col))
                {
                    positions[row + direction][col].State = PositionVM.CheckState.CanMove;
                }
            }
            // самый первый двойной ход
            if (row == startRow)
            {
                if (IsCoordinatesCorrect(row + direction * 2, col))
                {
                    if (positions[row + direction * 2][col].Figure == null && IsKingSafe(row + direction * 2, col))
                    {
                        positions[row + direction * 2][col].State = PositionVM.CheckState.CanMove;
                    }
                }
            }
            return null;
        }
        void KnightMovement()
        {
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 2, 1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 2, -1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -2, 1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -2, -1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 2);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 1, -2);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 2);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -1, -2);
        }

        void BishopMovement()
        {
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 1, -1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, -1, -1);
        }

        void RookMovement()
        {
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 0);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 0);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 0, 1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 0, -1);
        }

        void QueenMovement()
        {
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 0);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 0);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 0, 1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 0, -1);

            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, 1, -1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 1);
            LongMovement(SelectedPosition.Row, SelectedPosition.Column, -1, -1);
        }

        void KingMovement()
        {
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 0);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 0);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 0, 1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 0, -1);

            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 1, 1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, 1, -1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -1, 1);
            ShortMovement(SelectedPosition.Row, SelectedPosition.Column, -1, -1);
        }

        PositionVM LongMovement(int row, int col, int rowDelta, int colDelta)
        {
            PositionVM breakPosition;

            if (!IsCoordinatesCorrect(col, row))
            {
                return null;
            }
            if ((breakPosition = ShortMovement(row, col, rowDelta, colDelta)) != null)
            {
                return breakPosition;
            }
            // если не наткнулись на фигуру, то двигаемся дальше
            LongMovement(row + rowDelta, col + colDelta, rowDelta, colDelta);
            return null;

        }

        PositionVM ShortMovement(int row, int col, int rowDelta, int colDelta)
        {
            int nextRow = row + rowDelta;
            int nextCol = col + colDelta;

            if (!IsCoordinatesCorrect(nextRow, nextCol))
            {
                return null;
            }
            if (positions[nextRow][nextCol].Figure != null)
            {
                if (positions[nextRow][nextCol].Figure.FigureColor == SelectedPosition.Figure.FigureColor)
                {
                    return positions[nextRow][nextCol];
                }
            }

            // будет ли король в безопасности при перемещении на эту позицию?
            bool safeKingFlag = true;
            //safeKingFlag = IsKingSafe(whiteKingPosition.Row, whiteKingPosition.Row);

            if (positions[nextRow][nextCol].Figure != null)
            {
                if (safeKingFlag)
                    positions[nextRow][nextCol].State = PositionVM.CheckState.CanBeat;

                return positions[nextRow][nextCol];
            }

            if (safeKingFlag)
                positions[nextRow][nextCol].State = PositionVM.CheckState.CanMove;
            return null;
        }
        bool IsCoordinatesCorrect(int row, int col)
        {
            return (row < positions.Length && row >= 0) && (col < positions[row].Length && col >= 0);
        }
        #endregion

        // Проверки на безопасность короля, которые пока работают криво. 
        #region Validation is king safe
        enum Check { Empty, EndBoard, Ally, Enemy };
        bool IsKingSafe(int targetRow, int targetCol)
        {

            //PositionVM currentKing;
            //FigureColor kingColor = SelectedPosition.Figure.FigureColor;
            //currentKing = kingColor == FigureColor.White ? whiteKingPosition : blackKingPosition;

            //int kingRow = currentKing.Row;
            //int kingCol = currentKing.Column;

            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, 0, 1))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, 0, -1))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, 1, 0))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, -1, 0))
            //    return false;

            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, 1, 1))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, -1, 1))
            //    return false; 
            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, 1, -1))
            //    return false; 
            //if (!LongKingCheck(kingRow, kingCol, targetRow, targetCol, -1, -1))
            //    return false;

            return true;
        }

        bool LongKingCheck(int checkingRow, int checkingCol, int targetRow, int targetCol, int rowDelta, int colDelta)
        {

            //int row = checkingRow + rowDelta;
            //int col = checkingCol + colDelta;

            //while (IsCoordinatesCorrect(row, col))
            //{
            //    Check checkState = ShortKingCheck(row, col, targetRow, targetCol);
            //    switch (checkState)
            //    {
            //        case Check.EndBoard: return true;
            //        case Check.Ally: return true;
            //        case Check.Enemy: return false;
            //        case Check.Empty: 
            //            {
            //                row += rowDelta;
            //                col += colDelta;
            //                break;
            //            } 
            //    }
            //}
            return true;
        }

        Check ShortKingCheck(int rowCoord, int colCoord, int targetRow, int targetCol)
        {

            //if (!IsCoordinatesCorrect(rowCoord, colCoord))
            //{
            //    return Check.EndBoard;
            //}
            //else if (positions[rowCoord][colCoord] == SelectedPosition) // если эта позиция, с которой уходит выделенная фигура
            //{
            //    return Check.Empty;  
            //}

            //else if (rowCoord == targetRow && colCoord == targetCol) // если сюда встанет выделенная фигура в результате следующего хода
            //{
            //    return Check.Ally; 
            //}

            //if (positions[rowCoord][colCoord].Figure != null)
            //{
            //    IFigure anotherFigure = positions[rowCoord][colCoord].Figure;
            //    if (anotherFigure.FigureColor == SelectedPosition.Figure.FigureColor) 
            //    {
            //        return Check.Ally;
            //    }
            //    else 
            //    {
            //        return Check.Enemy;
            //    }
            //}
            return Check.Empty;
        }


        bool IsKingReallySafe(FigureColor kingColor)
        {
            //PositionVM currentKing;
            //currentKing = kingColor == FigureColor.White ? whiteKingPosition : blackKingPosition;

            //int kingRow = currentKing.Row;
            //int kingCol = currentKing.Column;

            //if (!LongKingCheck(kingRow, kingCol, 0, 1, kingColor))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, 0, -1, kingColor))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, 1, 0, kingColor))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, -1, 0, kingColor))
            //    return false;

            //if (!LongKingCheck(kingRow, kingCol, 1, 1, kingColor))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, -1, 1, kingColor))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, 1, -1, kingColor))
            //    return false;
            //if (!LongKingCheck(kingRow, kingCol, -1, -1, kingColor))
            //    return false;

            return true;
        }

        bool LongKingCheck(int checkingRow, int checkingCol, int rowDelta, int colDelta, FigureColor color)
        {

            //int row = checkingRow + rowDelta;
            //int col = checkingCol + colDelta;

            //while (IsCoordinatesCorrect(checkingCol, checkingRow))
            //{
            //    Check checkState = ShortKingCheck(row, col, color);
            //    switch (checkState)
            //    {
            //        case Check.EndBoard: return true;
            //        case Check.Ally: return true;
            //        case Check.Enemy: return false;
            //        case Check.Empty:
            //            {
            //                row += rowDelta;
            //                col += colDelta;
            //                break;
            //            }
            //    }
            //}
            return true;
        }

        Check ShortKingCheck(int rowCoord, int colCoord, FigureColor color)
        {
            //if (!IsCoordinatesCorrect(rowCoord, colCoord))
            //{
            //    return Check.EndBoard;
            //}

            //if (positions[rowCoord][colCoord].Figure != null)
            //{
            //    IFigure anotherFigure = positions[rowCoord][colCoord].Figure;
            //    if (anotherFigure.FigureColor == color)
            //    {
            //        return Check.Ally;
            //    }
            //    else
            //    {
            //        return Check.Enemy;
            //    }
            //}
            return Check.Empty;
        }

        #endregion
    }
}
