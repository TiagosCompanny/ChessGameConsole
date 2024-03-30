using System;
using System.Reflection.Emit;
using Tabletop;


namespace Chess_Game.chess
{
    class Rook : Piece
    {
        public Rook(Table table, PieceColor color) : base(table, color)
        {

        }
        public override string ToString()
        {
            return "R";
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

            // ⬆
            pos.DefineValues(position.Line - 1, position.Column);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null&& Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Line--;
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
                pos.Line++;
            }

            // ➡
            pos.DefineValues(position.Line, position.Column + 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column++;
            }

            // ⬅
            pos.DefineValues(position.Line, position.Column - 1);
            while (Table.IsValidPosition(pos) && CanMove(pos))
            {
                matriz[pos.Line, pos.Column] = true;
                if (Table.ReturnPiece(pos) != null && Table.ReturnPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column--;
            }

            return matriz;
        }

    }
}
