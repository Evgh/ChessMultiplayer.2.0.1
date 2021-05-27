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
            
            GameEnds += OnGameEnds;

            InitializePositions();
            InitializeFigures();

            IsWhiteMove = true;
            IsGameEnded = false;
        }

        public PositionVM this[int i, int j]
        {
            get => positions[i][j];
        }

        bool IsWhiteMove { get; set; }
        bool IsGameEnded { get; set; }
        
        public event EventHandler GameEnds;
        protected void OnGameEnds(object sender, EventArgs e)
        {
            IsGameEnded = true;
        }

        public event EventHandler Check;

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

            if (whiteKingPosition == targetPosition || blackKingPosition == targetPosition)
                GameEnds.Invoke(this, null);


            whiteKingPosition = (startPosition == whiteKingPosition) ? targetPosition : whiteKingPosition;
            blackKingPosition = (startPosition == blackKingPosition) ? targetPosition : blackKingPosition;

            SelectedPosition = null;
            RemoveSelection();
            IsWhiteMove = !IsWhiteMove;
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


            if (IsGameEnded)
            {
                IsGameEnded = false;
            }
            else
            {
                whiteKingPosition = (targetPosition == whiteKingPosition) ? startPosition : whiteKingPosition;
                blackKingPosition = (targetPosition == blackKingPosition) ? startPosition : blackKingPosition;
            }

            IsWhiteMove = !IsWhiteMove;
            RemoveSelection();
            
        }
        #endregion

        #region Initialization
        public void StartNewGame()
        {
            RemoveFigures();
            DeadBlacks.Clear();
            DeadWhites.Clear();

            InitializeFigures();
            RemoveSelection();

            IsWhiteMove = true;
            IsGameEnded = false;
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

            IsCheck();
        }

        public void DrawMoveOpportunities()
        {
            if (CanMove())
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
            else
            {
                SelectedPosition = null;
            }
        }


        bool CanMove()
        {
            if (!IsGameEnded)
                return IsWhiteMove ? SelectedPosition.Figure.FigureColor == FigureColor.White : SelectedPosition.Figure.FigureColor == FigureColor.Black;
            else
                return false;
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
                if (positions[row + direction][col + 1].Figure != null)
                {
                    if (positions[row + direction][col + 1].Figure.FigureColor != SelectedPosition.Figure.FigureColor)
                        positions[row + direction][col + 1].State = PositionVM.CheckState.CanBeat;
                }
            }

            // удар влево
            if (IsCoordinatesCorrect(row + direction, col - 1))
            {
                if (positions[row + direction][col - 1].Figure != null)
                {
                    if (positions[row + direction][col - 1].Figure.FigureColor != SelectedPosition.Figure.FigureColor)
                        positions[row + direction][col - 1].State = PositionVM.CheckState.CanBeat;
                }
            }

            // обычный ход
            if (IsCoordinatesCorrect(row + direction, col))
            {
                if (positions[row + direction][col].Figure == null)
                {
                    positions[row + direction][col].State = PositionVM.CheckState.CanMove;
                }
            }
            // самый первый двойной ход
            if (row == startRow)
            {
                if (IsCoordinatesCorrect(row + direction * 2, col))
                {
                    if (positions[row + direction * 2][col].Figure == null)
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
        void IsCheck()
        {
            if (DiagonalCheck(whiteKingPosition) || StraightCheck(whiteKingPosition) || PawnCheck(whiteKingPosition) || KnightCheck(whiteKingPosition) || KingCheck(whiteKingPosition)) 
            {
                whiteKingPosition.State = PositionVM.CheckState.Check;
                Check?.Invoke(this, null);
            }

            if (DiagonalCheck(blackKingPosition) || StraightCheck(blackKingPosition) || PawnCheck(blackKingPosition) || KnightCheck(blackKingPosition) || KingCheck(blackKingPosition)) 
            {
                blackKingPosition.State = PositionVM.CheckState.CanBeat;
                Check?.Invoke(this, null);
            }
        }

        // returns true if finds enemy
        bool DiagonalCheck(PositionVM currentKing)
        {
            var kingColor = currentKing.Figure.FigureColor;
            PositionVM buff;

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, 1, 1, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Bishop || buff.Figure.FigureType == FigureType.Queen);
            }

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, -1, 1, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Bishop || buff.Figure.FigureType == FigureType.Queen);
            }

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, 1, -1, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Bishop || buff.Figure.FigureType == FigureType.Queen);
            }

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, -1, -1, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Bishop || buff.Figure.FigureType == FigureType.Queen);
            }

            return false;
        }

        bool StraightCheck(PositionVM currentKing)
        {
            var kingColor = currentKing.Figure.FigureColor;
            PositionVM buff;

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, 1, 0, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Rook || buff.Figure.FigureType == FigureType.Queen);
            }

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, -1, 0, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Rook || buff.Figure.FigureType == FigureType.Queen);
            }

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, 0, 1, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Rook || buff.Figure.FigureType == FigureType.Queen);
            }

            if ((buff = LongCheck(currentKing.Row, currentKing.Column, 0, -1, kingColor)) != null)
            {
                return (buff.Figure.FigureType == FigureType.Rook || buff.Figure.FigureType == FigureType.Queen);
            }

            return false;
        }

        bool PawnCheck(PositionVM currentKing)
        {
            var kingColor = currentKing.Figure.FigureColor;
            PositionVM buff;
            bool isWhite = kingColor == FigureColor.White;
            int direction = isWhite ? -1 : 1;
            int startRow = isWhite ? 6 : 1;

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, direction, 1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Pawn);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, direction, -1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Pawn);

            return false;
        }

        bool KnightCheck(PositionVM currentKing)
        {
            var kingColor = currentKing.Figure.FigureColor;
            PositionVM buff;

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 2, 1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 2, -1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -2, 1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -2, -1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 1, 2, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 1, -2, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -1, 2, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -1, -2, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.Knight);

            return false;
        }

        bool KingCheck(PositionVM currentKing)
        {
            var kingColor = currentKing.Figure.FigureColor;
            PositionVM buff;

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 1, 0, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -1, 0, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 0, 1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 0, -1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 1, 1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, 1, -1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -1, 1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            if ((buff = ShortCheck(currentKing.Row, currentKing.Column, -1, -1, kingColor)) != null)
                return (buff.Figure.FigureType == FigureType.King);

            return false;
        }


        PositionVM LongCheck(int row, int col, int rowDelta, int colDelta, FigureColor color)
        {
            PositionVM breakPosition;
            int bufCol = col;
            int bufRow = row;

            while(IsCoordinatesCorrect(bufRow, bufCol))
            {
                if ((breakPosition = OneCheck(bufRow, bufCol, rowDelta, colDelta, color)) != null)
                {
                    if (breakPosition.Figure.FigureColor != color)
                        return breakPosition;
                    else
                        return null;
                }
                bufRow += rowDelta;
                bufCol += colDelta;
            }

            return null;
        }

        PositionVM ShortCheck(int row, int col, int rowDelta, int colDelta, FigureColor color)
        {
            PositionVM breakPosition;
            int bufCol = col;
            int bufRow = row;

            if ((breakPosition = OneCheck(bufRow, bufCol, rowDelta, colDelta, color)) != null)
            {
                if (breakPosition.Figure.FigureColor != color)
                    return breakPosition;
                else
                    return null;
            }

            return null;
        }

        PositionVM OneCheck(int row, int col, int rowDelta, int colDelta, FigureColor color)
        {
            int nextRow = row + rowDelta;
            int nextCol = col + colDelta;

            if (!IsCoordinatesCorrect(nextRow, nextCol))
            {
                return null;
            }

            if (positions[nextRow][nextCol].Figure != null)
            {
                return positions[nextRow][nextCol];
            }
            return null;
        }

        #endregion
    }
}
