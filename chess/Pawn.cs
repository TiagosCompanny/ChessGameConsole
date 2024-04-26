using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabletop;

namespace Chess_Game.chess
{
    class Pawn : Piece
    {
        private ChessGame game;

        public Pawn(Table table, PieceColor color, ChessGame game) : base(table, color)
        {
            this.game = game;
        }
        public override string ToString()
        {
            return "P";
        }
        private bool IsThereEnemy(Position position)
        {
            Piece piece = Table.ReturnPiece(position);
            return piece != null && piece.Color != Color;
        }
        private bool IsFree(Position position)
        {
            return Table.ReturnPiece(position) == null;
        }
        public override bool[,] IsValidMovimentations()
        {
            bool[,] matriz = new bool[Table.line, Table.column];

            Position pos = new Position(0, 0);


            if (Color == PieceColor.White)
            {

                pos.DefineValues(position.Line - 1, position.Column);
                if (Table.IsValidPosition(pos) && IsFree(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }
                pos.DefineValues(position.Line - 2, position.Column);
                Position p2 = new Position(position.Line - 1, position.Column);
                if (Table.IsValidPosition(p2) && IsFree(p2) && Table.IsValidPosition(pos) && IsFree(pos) && QntMoviments == 0)
                {
                    matriz[pos.Line, pos.Column] = true;
                }
                pos.DefineValues(position.Line - 1, position.Column - 1);
                if (Table.IsValidPosition(pos) && IsThereEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }
                pos.DefineValues(position.Line - 1, position.Column + 1);
                if (Table.IsValidPosition(pos) && IsThereEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                //#SpecialMove: EnPassant
                if(position.Line == 3)
                {
                    Position leftPosition = new Position(position.Line, position.Column - 1);
                    if(Table.IsValidPosition(leftPosition) && IsThereEnemy(leftPosition) && Table.ReturnPiece(leftPosition) == game.PieceWithPossibleEnPassantCapture)
                    {
                        matriz[leftPosition.Line - 1, leftPosition.Column] = true;
                    }

                    Position rightPosition = new Position(position.Line, position.Column + 1);
                    if (Table.IsValidPosition(rightPosition) && IsThereEnemy(rightPosition) && Table.ReturnPiece(rightPosition) == game.PieceWithPossibleEnPassantCapture)
                    {
                        matriz[rightPosition.Line - 1, rightPosition.Column] = true;
                    }
                }
            }
            else
            {
                pos.DefineValues(position.Line + 1, position.Column);
                if (Table.IsValidPosition(pos) && IsFree(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }
                pos.DefineValues(position.Line + 2, position.Column);
                Position p2 = new Position(position.Line + 1, position.Column);
                if (Table.IsValidPosition(p2) && IsFree(p2) && Table.IsValidPosition(pos) && IsFree(pos) && QntMoviments == 0)
                {
                    matriz[pos.Line, pos.Column] = true;
                }
                pos.DefineValues(position.Line + 1, position.Column - 1);
                if (Table.IsValidPosition(pos) && IsThereEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }
                pos.DefineValues(position.Line + 1, position.Column + 1);
                if (Table.IsValidPosition(pos) && IsThereEnemy(pos))
                {
                    matriz[pos.Line, pos.Column] = true;
                }

                //#SpecialMove: EnPassant
                if (position.Line == 4)
                {
                    Position leftPosition = new Position(position.Line, position.Column - 1);
                    if (Table.IsValidPosition(leftPosition) && IsThereEnemy(leftPosition) && Table.ReturnPiece(leftPosition) == game.PieceWithPossibleEnPassantCapture)
                    {
                        matriz[leftPosition.Line + 1, leftPosition.Column] = true;
                    }

                    Position rightPosition = new Position(position.Line, position.Column + 1);
                    if (Table.IsValidPosition(rightPosition) && IsThereEnemy(rightPosition) && Table.ReturnPiece(rightPosition) == game.PieceWithPossibleEnPassantCapture)
                    {
                        matriz[rightPosition.Line + 1, rightPosition.Column] = true;
                    }
                }
            }

            return matriz;
        }
    }
}
