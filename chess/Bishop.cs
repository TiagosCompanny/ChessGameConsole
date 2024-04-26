using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabletop;

namespace Chess_Game.chess
{
    class Bishop : Piece
    {
        public Bishop(Table table, PieceColor color) : base(table, color)
        {}

        public override string ToString()
        {
            return "B";
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

            //⬉
            pos.DefineValues(position.Line - 1, position.Column - 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if(Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                    break;

                pos.DefineValues(pos.Line - 1, pos.Column - 1);
            }

            //⬈
            pos.DefineValues(position.Line - 1, position.Column + 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                    break;

                pos.DefineValues(pos.Line - 1, pos.Column + 1);
            }

            //⬊
            pos.DefineValues(position.Line + 1, position.Column + 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                    break;

                pos.DefineValues(pos.Line + 1, pos.Column + 1);
            }

            //⬋
            pos.DefineValues(position.Line + 1, position.Column - 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                    break;

                pos.DefineValues(pos.Line + 1, pos.Column - 1);
            }

            return matriz;
        }
    }
}
