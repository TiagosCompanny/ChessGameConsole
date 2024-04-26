using Tabletop;

namespace Chess_Game.chess
{
    class Knight : Piece
    {
        public Knight(Table table, PieceColor color) : base(table, color)
        {}

        public override string ToString()
        {
            return "N";
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

            pos.DefineValues(position.Line - 1, position.Column - 2);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line - 2, position.Column - 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line + 2, position.Column + 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line - 2, position.Column + 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line - 1, position.Column + 2);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line + 1, position.Column + 2);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line + 2, position.Column - 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            pos.DefineValues(position.Line + 1, position.Column - 2);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            return matriz;
        }
    }
}
