

namespace Tabletop
{
    class Table
    {
        public int line { get; set; }
        public int column { get; set; }

        private Piece[,] Pieces;

        public Table(int line, int column)
        {
            this.line = line;
            this.column = column;
            Pieces = new Piece[line, column];

        }

        public Piece ReturnPiece(int line, int column)
        {
            return Pieces[line, column];
        }
        public Piece ReturnPiece(Position position)
        {
            return Pieces[position.Line, position.Column];
        }

        public bool HasPiece(Position position)
        {
            ValidatePosition(position);
            return ReturnPiece(position) != null;
        }


        public void InputPiece(Piece piece, Position position)
        {
            if (HasPiece(position))
            {
                throw new TableException("There is already a piece in this position");
            }

            Pieces[position.Line, position.Column] = piece;
            piece.position = position;
        }

        public Piece RemovePiece(Position position)
        {
            if (ReturnPiece(position) == null)
                return null;

            Piece auxPiece = ReturnPiece(position);
            auxPiece.position = null;
            Pieces[position.Line, position.Column] = null;
            return auxPiece;

        }
        public bool IsValidPosition(Position position)
        {
            if (position.Line < 0 || position.Line >= line || position.Column < 0 || position.Column >= column)
            {
                return false;
            }
            return true;
        }
        public void ValidatePosition(Position position)
        {
            if (!IsValidPosition(position))
            {
                throw new TableException("Invalid Position");
            }
        }
    }
}

