using Chess_Game.chess;
using Tabletop;

namespace Chess_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChessGame game = new ChessGame();
                Console.WriteLine();

                while (!game.IsGameFinished)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintGame(game);


                        Console.WriteLine();
                        Console.WriteLine("Origem: ");
                        Position origin = Screen.ReadChesPosition().ToPosition();
                        game.ValidateOrignPosition(origin);

                        bool[,] possiblePossitions = game.Table.ReturnPiece(origin).IsValidMovimentations();
                        Console.Clear();

                        Screen.PrintTable(game.Table, possiblePossitions);
                        Console.WriteLine();
                        Console.WriteLine("Destino: ");
                        Position destine = Screen.ReadChesPosition().ToPosition();
                        game.ValidateDestinePosition(origin, destine);

                        
                        game.DoMove(origin, destine);
                    }
                    catch (TableException exception)
                    {
                        Console.WriteLine(exception.Message);
                        Console.Read();
                    }              
                }
                //Show Winner
                Console.Clear();
                Screen.PrintGame(game);
            }
            catch (TableException exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
