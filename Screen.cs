using Chess_Game.chess;
using System;
using Tabletop;


namespace Chess_Game
{
    class Screen
    {

        public static void PrintGame(ChessGame game)
        {
            PrintTable(game.Table);
            PrintCapturedPieces(game);
            Console.WriteLine();
            Console.WriteLine("Turn: " + game.Turn);

            if(!game.IsGameFinished)
            {
                Console.WriteLine("Wating current player: " + game.curentPlayer);
                if (game.IsGameInCheck) { Console.WriteLine("CHECK ! "); }
            }
            else
            {
                Console.WriteLine("CHECK MATE!");
                Console.WriteLine("WINNER: " + game.curentPlayer);
            }
        }

        public static void PrintCapturedPieces(ChessGame game)
        {
            Console.WriteLine("Captured Pieces: ");
            Console.Write("White: ");
            PrintCapturedPiecesCollection(game.GetCapturedPiecesFromPlayer(PieceColor.Write));
            Console.WriteLine();
            Console.Write("Black: ");
            ConsoleColor auxColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintCapturedPiecesCollection(game.GetCapturedPiecesFromPlayer(PieceColor.Black));
            Console.ForegroundColor = auxColor;
            Console.WriteLine();
        }
        public static void PrintCapturedPiecesCollection(HashSet<Piece> pieces)
        {
            Console.Write("[");
            foreach (var piece in pieces)
            {
                Console.Write(piece + " ");
            }
            Console.Write("]");
        }



        public static void PrintTable(Table table)
        {
            for (int i = 0; i < table.line; i++)
            {
                Console.Write(8 - i + " ");

                for (int j = 0; j < table.column; j++)
                {
                    PrintPiece(table.ReturnPiece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
        }
        public static void PrintTable(Table table, bool[,] possibleMoviments)
        {
            ConsoleColor originalColor = Console.BackgroundColor;
            ConsoleColor canMoveColor = ConsoleColor.DarkGray;


            for (int i = 0; i < table.line; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < table.column; j++)
                {
                    if (possibleMoviments[i,j])
                    {
                        Console.BackgroundColor = canMoveColor;
                    }
                    else
                    {
                        Console.BackgroundColor = originalColor;
                    }
                    PrintPiece(table.ReturnPiece(i, j));
                    Console.BackgroundColor = originalColor;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  A B C D E F G H");
            Console.BackgroundColor = originalColor;
        }


        public static ChessPositionNotation ReadChesPosition()
        {
            string input = Console.ReadLine();
            var inputColumn = input[0].ToString().ToLower();
            char inputColumnProcessed = char.Parse(inputColumn);
            int inputLine = int.Parse(input[1] + "");
            return new ChessPositionNotation(inputColumnProcessed, inputLine);
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null) 
                Console.Write("- ");
            else
            {
                if (piece.Color == PieceColor.Write)
                    Console.Write(piece);
                else
                {
                    ConsoleColor colorAux = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(piece);
                    Console.ForegroundColor = colorAux;

                }
                Console.Write(" ");
            }
        }

    }
}
