
namespace Tabletop
{
    abstract class Piece
    {
        public Position position { get; set; }
        public PieceColor Color { get; protected set; }
        public int QntMoviments { get; protected set; }
        public Table Table { get; protected set; }

        public Piece(Table table, PieceColor color)
        {
            this.position = null;
            this.Table = table;
            this.Color = color;
            this.QntMoviments = 0;
        }

        public void IncrementQntMoviments()
        {
            QntMoviments++;
        }
        public void DecrementQntMoviments()
        {
            QntMoviments--;
        }
        public bool ExistPossibleMoviments()
        {
            bool[,] matriz = IsValidMovimentations();
            for (int i = 0; i < Table.line; i++)
            {
                for (int j = 0; j < Table.column; j++)
                {
                    if (matriz[i, j])
                        return true;
                }
            }
            return false;
        }


        public bool PossibleMovement(Position position)
        {
            return IsValidMovimentations()[position.Line, position.Column];
        }
        public abstract bool[,] IsValidMovimentations();
       
    }
}
