using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Tabletop;

namespace Chess_Game.chess
{
    class Queen : Piece
    {
        public Queen(Table table, PieceColor color) : base(table, color)
        {}

        public override string ToString()
        {
            return "Q";
        }

        private bool CanMove(Position position)
        {
            Piece piece = Table.ReturnPiece(position);
            return piece == null || piece.Color != Color;
        }

        public override bool[,] IsValidMovimentations()
        {
            bool[,] matriz = new bool[Table.line, Table.column];

            Position pos = new Position(0, 0);

            // ⬅
            pos.DefineValues(position.Line, position.Column - 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line, pos.Column - 1);
            }

            // ⮕
            pos.DefineValues(position.Line, position.Column + 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line, pos.Column + 1);
            }

            // ⬆
            pos.DefineValues(position.Line - 1, position.Column);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line - 1, pos.Column);
            }

            // ⬇
            pos.DefineValues(position.Line + 1, position.Column);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line + 1, pos.Column);
            }

            // ⬉
            pos.DefineValues(position.Line - 1, position.Column - 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line - 1, pos.Column - 1);
            }

            // ⬈
            pos.DefineValues(position.Line - 1, position.Column + 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line - 1, pos.Column + 1);
            }

            // ⬊
            pos.DefineValues(position.Line + 1, position.Column + 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line + 1, pos.Column + 1);
            }

            // ⬋
            pos.DefineValues(position.Line + 1, position.Column - 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.DefineValues(pos.Line + 1, pos.Column - 1);
            }

            return matriz;
        }
    }
}
