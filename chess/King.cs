using System;
using System.Net.NetworkInformation;
using Tabletop;


namespace Chess_Game.chess
{
    class King : Piece
    {
        private ChessGame game;
        public King(Table table, PieceColor color, ChessGame game) : base(table, color)
        {
            this.game = game;
        }


        public override string ToString()
        {
            return "K";
        }

        private bool CanMove(Position position)
        {
            Piece piece = Table.ReturnPiece(position);
            return piece == null || piece.Color != Color ;
        }

        private bool ValidationRookToCastle(Position position)
        {
            Piece piece = Table.ReturnPiece(position);
            return piece != null && piece is Rook && piece.Color == Color && piece.QntMoviments == 0;
        }




        //public override bool[,] IsValidMovimentations()
        //{
        //    bool[,] matriz = new bool[Table.Line, Table.Column];

        //    int[] directions = { -1, 0, 1 };

        //    foreach (int i in directions)
        //    {
        //        foreach (int j in directions)
        //        {
        //            if (i == 0 && j == 0)
        //                continue; // Ignorar a posição atual

        //            Position pos = new Position(position.Line + i, position.Column + j);

        //            if (Table.IsValidPosition(pos) && CanMove(pos))
        //                matriz[pos.Line, pos.Column] = true;
        //        }
        //    }

        //    return matriz;
        //}

        public override bool[,] IsValidMovimentations()
        {
            bool[,] matriz = new bool[Table.line, Table.column];

            Position pos = new Position(0, 0);

            // ⬆
            pos.DefineValues(position.Line - 1, position.Column);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            // ⬈
            pos.DefineValues(position.Line - 1, position.Column + 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            //⮕
            pos.DefineValues(position.Line, position.Column + 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            //⬊
            pos.DefineValues(position.Line + 1, position.Column + 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            //⬇
            pos.DefineValues(position.Line + 1, position.Column);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            //⬋
            pos.DefineValues(position.Line + 1, position.Column - 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            //⬅
            pos.DefineValues(position.Line, position.Column - 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;

            //⬉
            pos.DefineValues(position.Line - 1, position.Column - 1);
            if (Table.IsValidPosition(pos) && CanMove(pos))
                matriz[pos.Line, pos.Column] = true;


            //#SpecialMove: Castle 
            if (QntMoviments == 0 && !game.IsGameInCheck)
            {
                //#SpecialMove: Castle __Litle Castle
                Position rookPosition_H1 = new Position(position.Line, position.Column + 3);
                if (ValidationRookToCastle(rookPosition_H1))
                {
                    Position position1 = new Position(position.Line, position.Column + 1);
                    Position position2 = new Position(position.Line, position.Column + 2);

                    if (Table.ReturnPiece(position1) == null && Table.ReturnPiece(position2) == null)
                        matriz[pos.Line, pos.Column + 2] = true;
                }
                //#SpecialMove: Castle __Large Castle
                Position rookPosition_A8 = new Position(position.Line, position.Column - 4);
                if (ValidationRookToCastle(rookPosition_A8))
                {
                    Position position1 = new Position(position.Line, position.Column - 1);
                    Position position2 = new Position(position.Line, position.Column - 2);
                    Position position3 = new Position(position.Line, position.Column - 2);

                    if (Table.ReturnPiece(position1) == null && Table.ReturnPiece(position2) == null && Table.ReturnPiece(position3) == null)
                        matriz[pos.Line, pos.Column - 2] = true;
                }
            }




            return matriz;
        }


    }
}
