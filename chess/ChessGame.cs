using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using Tabletop;
using System.Runtime.ConstrainedExecution;
using System.Drawing;

namespace Chess_Game.chess
{
    class ChessGame
    {
        public Table Table { get; private set; }
        public int Turn { get; private set; }
        public PieceColor curentPlayer { get; private set; }
        public bool IsGameFinished { get; private set; }
        public HashSet<Piece> pieces;
        public HashSet<Piece> capturedPieces;
        public bool IsGameInCheck;
        public Piece? PieceWithPossibleEnPassantCapture { get; private set; }

        public ChessGame()
        {
            Table = new Table(8, 8);
            Turn = 1;
            curentPlayer = PieceColor.White;
            IsGameFinished = false;
            IsGameInCheck = false;
            PieceWithPossibleEnPassantCapture = null;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            InputPieces();

        }

        private Piece ExecuteMoviment(Position origin, Position destine)
        {
            Piece piece = Table.RemovePiece(origin);
            piece.IncrementQntMoviments();
            Piece Capturedpiece = Table.RemovePiece(destine);

            Table.InputPiece(piece, destine);

            if (Capturedpiece is not null)
            {
                capturedPieces.Add(Capturedpiece);
            }

            //#SpecialMove: Litle Castle 
            if (piece is King && destine.Column == origin.Column + 2)
            {
                Position orignRook = new Position(origin.Line, origin.Column + 3);
                Position destineRook = new Position(origin.Line, origin.Column + 1);
                Piece rookPiece = Table.RemovePiece(orignRook);
                rookPiece.IncrementQntMoviments();
                Table.InputPiece(rookPiece, destineRook);
            }

            //#SpecialMove: Large Castle 
            if (piece is King && destine.Column == origin.Column - 2)
            {
                Position orignRook = new Position(origin.Line, origin.Column - 4);
                Position destineRook = new Position(origin.Line, origin.Column - 1);
                Piece rookPiece = Table.RemovePiece(orignRook);
                rookPiece.IncrementQntMoviments();
                Table.InputPiece(rookPiece, destineRook);
            }

            //#SpecialMove: En Passant 
            if (piece is Pawn)
            {
                if (origin.Column != destine.Column && Capturedpiece == null)
                {
                    Position pawnPosition;

                    if (piece.Color == PieceColor.White)
                        pawnPosition = new Position(destine.Line + 1, destine.Column);
                    else
                        pawnPosition = new Position(destine.Line - 1, destine.Column);

                    Capturedpiece = Table.RemovePiece(pawnPosition);
                    capturedPieces.Add(Capturedpiece);

                }
            }

            return Capturedpiece;

        }

        public void UndoMove(Position origin, Position destine, Piece caputedPiece)
        {
            Piece piece = Table.RemovePiece(destine);
            piece.DecrementQntMoviments();


            if (caputedPiece != null)
            {
                Table.InputPiece(caputedPiece, destine);
                capturedPieces.Remove(caputedPiece);
            }

            Table.InputPiece(piece, origin);

            //#SpecialMove: Litle Castle 
            if (piece is King && destine.Column == origin.Column + 2)
            {
                Position orignRook = new Position(origin.Line, origin.Column + 3);
                Position destineRook = new Position(origin.Line, origin.Column + 1);
                Piece rookPiece = Table.RemovePiece(destineRook);
                rookPiece.IncrementQntMoviments();
                Table.InputPiece(rookPiece, orignRook);
            }

            //#SpecialMove: Large Castle 
            if (piece is King && destine.Column == origin.Column - 2)
            {
                Position orignRook = new Position(origin.Line, origin.Column - 4);
                Position destineRook = new Position(origin.Line, origin.Column - 1);
                Piece rookPiece = Table.RemovePiece(destineRook);
                rookPiece.IncrementQntMoviments();
                Table.InputPiece(rookPiece, orignRook);
            }

            //#SpecialMove: En Passant 
            if (piece is Pawn)
            {
                if (origin.Column != destine.Column && caputedPiece == PieceWithPossibleEnPassantCapture)
                {
                    Piece piecePawn = Table.RemovePiece(destine);


                    Position pawnPosition;

                    if (piece.Color == PieceColor.White)
                        pawnPosition = new Position(3, destine.Column);
                    else
                        pawnPosition = new Position(4, destine.Column);

                    Table.InputPiece(piecePawn, pawnPosition);

                }
            }
        }



        public void DoMove(Position origin, Position destine)
        {
            Piece caputedPiece = ExecuteMoviment(origin, destine);

            if (IsKingInCheck(curentPlayer))
            {
                UndoMove(origin, destine, caputedPiece);
                throw new TableException("You can not check your King");
            }

            Piece pieceMoved = Table.ReturnPiece(destine);
            //#SpecialMove: Promotion
            if (pieceMoved is Pawn)
            {
                if ((pieceMoved.Color == PieceColor.White && destine.Line == 0) || (pieceMoved.Color == PieceColor.Black && destine.Line == 7))
                {
                    PromoveNewPiece(pieceMoved, destine);
                }
            }

            if (IsKingInCheck(GetAdversaryColor(curentPlayer)))
                IsGameInCheck = true;

            else
                IsGameInCheck = false;

            if (IsKingInCheckMATE(GetAdversaryColor(curentPlayer)))
                IsGameFinished = true;
            else
            {
                Turn++;
                //ChangeColorPlayer
                if (curentPlayer == PieceColor.White)
                {
                    curentPlayer = PieceColor.Black;
                }
                else
                {
                    curentPlayer = PieceColor.White;
                }
            }

            //#SpecialMove: EnPassant

            if (pieceMoved is Pawn && (destine.Line == origin.Line - 2 || destine.Line == origin.Line + 2))
                PieceWithPossibleEnPassantCapture = pieceMoved;
            else
                PieceWithPossibleEnPassantCapture = null;

        }

        public void ValidateOrignPosition(Position origin)
        {
            if (Table.ReturnPiece(origin) is null)
            {
                throw new TableException("There is no piece in this possition");
            }
            if (curentPlayer != Table.ReturnPiece(origin).Color)
            {
                throw new TableException("This piece own your adversary");
            }
            if (!Table.ReturnPiece(origin).ExistPossibleMoviments())
            {
                throw new TableException("There is no possible moves to this piece");
            }
        }

        public void ValidateDestinePosition(Position origin, Position destine)
        {
            if (!Table.ReturnPiece(origin).PossibleMovement(destine))
            {
                throw new TableException("Invalid Destine Position!");
            }
        }

        public HashSet<Piece> GetCapturedPiecesFromPlayer(PieceColor color)
        {
            HashSet<Piece> piecesCollection = new HashSet<Piece>();
            foreach (var piece in capturedPieces)
            {
                if (piece.Color == color)
                    piecesCollection.Add(piece);
            }
            return piecesCollection;
        }
        public HashSet<Piece> GetPiecesInGame(PieceColor color)
        {
            HashSet<Piece> piecesCollection = new HashSet<Piece>();
            foreach (var piece in pieces)
            {
                if (piece.Color == color)
                    piecesCollection.Add(piece);
            }
            piecesCollection.ExceptWith(GetCapturedPiecesFromPlayer(color));
            return piecesCollection;
        }

        private PieceColor GetAdversaryColor(PieceColor cor)
        {
            if (cor == PieceColor.White)
            {
                return PieceColor.Black;
            }
            else
            {
                return PieceColor.White;
            }
        }
        private Piece GetKing(PieceColor color)
        {
            foreach (var piece in GetPiecesInGame(color))
            {
                if (piece is King)
                    return piece;
            }
            return null;
        }
        public bool IsKingInCheck(PieceColor color)
        {
            Piece king = GetKing(color);

            if (king == null)
                throw new TableException("There is no " + color + "king on the table");

            foreach (var piece in GetPiecesInGame(GetAdversaryColor(color)))
            {
                bool[,] mat = piece.IsValidMovimentations();

                if (mat[king.position.Line, king.position.Column])
                    return true;
            }
            return false;
        }


        public bool IsKingInCheckMATE(PieceColor color)
        {
            if (!IsKingInCheck(color))
                return false;

            foreach (var piece in GetPiecesInGame(color))
            {
                bool[,] mat = piece.IsValidMovimentations();

                for (int i = 0; i < Table.line; i++)
                {
                    for (int j = 0; j < Table.column; j++)
                    {
                        if (mat[i, j])
                        {
                            Position orign = piece.position;
                            Position destine = new Position(i, j);
                            Piece capturedPiece = ExecuteMoviment(orign, destine);
                            bool testIsInCheck = IsKingInCheck(color);
                            UndoMove(orign, destine, capturedPiece);
                            if (!testIsInCheck)
                                return false;
                        }

                    }
                }
            }
            return true;
        }

        public void inputNewPiece(char column, int line, Piece piece)
        {
            Table.InputPiece(piece, new ChessPositionNotation(column, line).ToPosition());
            pieces.Add(piece);

        }
        private void PromoveNewPiece(Piece pieceMoved, Position destine)
        {
            pieceMoved = Table.RemovePiece(destine);
            pieces.Remove(pieceMoved);

            Piece newPiece;
            Console.WriteLine("Select which piece will you promove:\n");
            Console.WriteLine("1 - QUEEN");
            Console.WriteLine("2 - ROOK");
            Console.WriteLine("3 - KNIGHT");
            Console.WriteLine("4 - BISHOP\n");

            string valueSelected = Console.ReadLine();

            int i = 0;
            if (int.TryParse(valueSelected, out i))
            {
                switch (i)
                {
                    case 1:
                        newPiece = new Queen(Table, pieceMoved.Color);
                        break;
                    case 2:
                        newPiece = new Rook(Table, pieceMoved.Color);
                        break;
                    case 3:
                        newPiece = new Knight(Table, pieceMoved.Color);
                        break;
                    case 4:
                        newPiece = new Bishop(Table, pieceMoved.Color);
                        break;
                    default:
                        newPiece = new Queen(Table, pieceMoved.Color);
                        break;
                }
                Table.InputPiece(newPiece, destine);
                pieces.Add(newPiece);
            }
            else
            {
                Piece newQueen = new Queen(Table, pieceMoved.Color);
                Table.InputPiece(newQueen, destine);
                pieces.Add(newQueen);

            }

        }


        private void InputPieces()
        {


            inputNewPiece('a', 1, new Rook(Table, PieceColor.White));
            inputNewPiece('b', 1, new Knight(Table, PieceColor.White));
            inputNewPiece('c', 1, new Bishop(Table, PieceColor.White));
            inputNewPiece('d', 1, new Queen(Table, PieceColor.White));
            inputNewPiece('e', 1, new King(Table, PieceColor.White, this));
            inputNewPiece('f', 1, new Bishop(Table, PieceColor.White));
            inputNewPiece('g', 1, new Knight(Table, PieceColor.White));
            inputNewPiece('h', 1, new Rook(Table, PieceColor.White));
            inputNewPiece('a', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('b', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('c', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('d', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('e', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('f', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('g', 2, new Pawn(Table, PieceColor.White, this));
            inputNewPiece('h', 2, new Pawn(Table, PieceColor.White, this));

            inputNewPiece('a', 8, new Rook(Table, PieceColor.Black));
            inputNewPiece('b', 8, new Knight(Table, PieceColor.Black));
            inputNewPiece('c', 8, new Bishop(Table, PieceColor.Black));
            inputNewPiece('d', 8, new Queen(Table, PieceColor.Black));
            inputNewPiece('e', 8, new King(Table, PieceColor.Black, this));
            inputNewPiece('f', 8, new Bishop(Table, PieceColor.Black));
            inputNewPiece('g', 8, new Knight(Table, PieceColor.Black));
            inputNewPiece('h', 8, new Rook(Table, PieceColor.Black));
            inputNewPiece('a', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('b', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('c', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('d', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('e', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('f', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('g', 7, new Pawn(Table, PieceColor.Black, this));
            inputNewPiece('h', 7, new Pawn(Table, PieceColor.Black, this));
        }

    }
}
