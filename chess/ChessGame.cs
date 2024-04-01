using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using Tabletop;
using System.Runtime.ConstrainedExecution;

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
        public Piece PieceWithPossibleEnPassantCapture { get; private set; }

        public ChessGame()
        {
            Table = new Table(8, 8);
            Turn = 1;
            curentPlayer = PieceColor.Write;
            IsGameFinished = false;
            IsGameInCheck = false;
            PossibleEnPassantCapture = null;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            InputPieces();

        }

        private Piece ExecuteMoviment(Position origin, Position destine)
        {
            Piece piece = Table.RemovePiece(origin);
            piece.IncrementQntMoviments();
            Piece Caputedpiece = Table.RemovePiece(destine);

            Table.InputPiece(piece, destine);

            if (Caputedpiece is not null)
            {
                capturedPieces.Add(Caputedpiece);
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

            return Caputedpiece;

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



        }



        public void DoMove(Position origin, Position destine)
        {
            Piece caputedPiece = ExecuteMoviment(origin, destine);

            if (IsKingInCheck(curentPlayer))
            {
                UndoMove(origin, destine, caputedPiece);
                throw new TableException("You can not check your King");
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
                if (curentPlayer == PieceColor.Write)
                {
                    curentPlayer = PieceColor.Black;
                }
                else
                {
                    curentPlayer = PieceColor.Write;
                }
            }

            //#SpecialMove: EnPassant

            Piece pieceTestEnPassant = Table.ReturnPiece(destine);
            if (pieceTestEnPassant is Pawn && (destine.Line == origin.Line - 2 || destine.Line == origin.Line + 2))
                PossibleEnPassantCapture = pieceTestEnPassant;
            else
                PossibleEnPassantCapture = null;

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
            if (cor == PieceColor.Write)
            {
                return PieceColor.Black;
            }
            else
            {
                return PieceColor.Write;
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
                            Piece capturedPiece = ExecuteMoviment(piece.position, new Position(i, j));
                            bool testIsInCheck = IsKingInCheck(color);
                            UndoMove(orign, destine, capturedPiece);
                            if (!testIsInCheck)
                                return false;
                        }

                    }
                }
            }
            return false;
        }

        public void inputNewPiece(char column, int line, Piece piece)
        {
            Table.InputPiece(piece, new ChessPositionNotation(column, line).ToPosition());
            pieces.Add(piece);

        }

        private void InputPieces()
        {


            inputNewPiece('a', 1, new Rook(Table, PieceColor.Write));
            inputNewPiece('b', 1, new Knight(Table, PieceColor.Write));
            inputNewPiece('c', 1, new Bishop(Table, PieceColor.Write));
            inputNewPiece('d', 1, new Queen(Table, PieceColor.Write));
            inputNewPiece('e', 1, new King(Table, PieceColor.Write, this));
            inputNewPiece('f', 1, new Bishop(Table, PieceColor.Write));
            inputNewPiece('g', 1, new Knight(Table, PieceColor.Write));
            inputNewPiece('h', 1, new Rook(Table, PieceColor.Write));
            inputNewPiece('a', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('b', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('c', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('d', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('e', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('f', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('g', 2, new Pawn(Table, PieceColor.Write, this));
            inputNewPiece('h', 2, new Pawn(Table, PieceColor.Write, this));

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




            //inputNewPiece('c', 1, new Rook(Table, PieceColor.Write));
            //inputNewPiece('c', 2, new Rook(Table, PieceColor.Write));
            //inputNewPiece('d', 2, new Rook(Table, PieceColor.Write));
            //inputNewPiece('e', 2, new Rook(Table, PieceColor.Write));
            //inputNewPiece('e', 1, new Rook(Table, PieceColor.Write));
            //inputNewPiece('d', 1, new King(Table, PieceColor.Write));

            //inputNewPiece('c', 7, new Rook(Table, PieceColor.Black));
            //inputNewPiece('c', 8, new Rook(Table, PieceColor.Black));
            //inputNewPiece('d', 7, new Rook(Table, PieceColor.Black));
            //inputNewPiece('e', 7, new Rook(Table, PieceColor.Black));
            //inputNewPiece('e', 8, new Rook(Table, PieceColor.Black));
            //inputNewPiece('d', 8, new King(Table, PieceColor.Black));

        }

    }
}
