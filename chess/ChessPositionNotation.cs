

using Tabletop;

namespace Chess_Game.chess
{
    class ChessPositionNotation
    {
        public char Column { get; private set; }
        public int Line { get; set; } 

        public ChessPositionNotation(char column, int line)
        {
            this.Column = column;
            this.Line = line;
        }

        public Position ToPosition()
        {
            return new Position(8 - Line, Column - 'a');
        }



        public override string ToString()
        {
            return "" + Column + Line;
        }
    }
}
