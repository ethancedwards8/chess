using System;
using System.Collections.Generic;
using System.Drawing;


namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {

            Chess.Board b = new Chess.Board();

            b.DisplayBoard();

            Console.WriteLine(Coords.ToChessNotation(3, 3));

            Console.WriteLine($"The coords of tile 36 are {b.FindCoordsOfID(36)}");
        }
    }

    enum PieceType
    {
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }

    enum MovementType
    {
        Linear,
        Diagonal,
        L,
        Forward,
        Free,
    }

    enum ChessNotation
    {
        A = 1,
        B, // 2
        C, // 3
        D, // 4
        E, // 5
        F, // 6
        G, // 7
        H  // 8
    }

    enum Owner
    {
        White,
        Black
    }

    public class Coords
    {
        public int X { get; }
        public int Y { get; }

        public Coords(int X = 0, int Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }

        public override string ToString() => $"{X}, {Y}"; // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
        public static string ToChessNotation(int x, int y)
        {
            string res;

            switch (x)
            {
                case (int)ChessNotation.A: // https://stackoverflow.com/questions/943398/get-int-value-from-enum-in-c-sharp
                    res = $"a{y}";
                    break;

                case (int)ChessNotation.B:
                    res = $"b{y}";
                    break;

                case (int)ChessNotation.C:
                    res = $"c{y}";
                    break;

                case (int)ChessNotation.D:
                    res = $"d{y}";
                    break;

                case (int)ChessNotation.E:
                    res = $"e{y}";
                    break;

                case (int)ChessNotation.F:
                    res = $"f{y}";
                    break;

                case (int)ChessNotation.G:
                    res = $"g{y}";
                    break;

                case (int)ChessNotation.H:
                    res = $"h{y}";
                    break;

                default:
                    return "err";
                    break;
            }

            return res;
        }
    }

    class Tile
    {
        public Color TileColor { get; set; }
        public int ID { get; set; }
        public Piece Piece { get; set; }

        public Tile(int ID, Color tileColor)
        {
            this.ID = ID;
            this.TileColor = tileColor;
        }
    }

    class Board
    {
        private Tile[,] board = new Tile[8, 8];

        public Board()
        {
            CreateBoard();
        }

        public void CreateBoard() // also serves as a board "reset"
        {
            int cnt = 0;
            bool ticker = false;

            for (int i = 0; i < 8; i++) // y
            {
                for (int j = 0; j < 8; j++) // x
                {
                    cnt++;
                    board[i, j] = new Tile(cnt, ticker ? Color.White : Color.Black);
                    ticker = !ticker;
                }
            }
        }

        public void DisplayBoard()
        {
            int num = 0;

            for (int i = 0; i < 8; i++) // y
            {
                for (int j = 0; j < 8; j++) // x 
                {
                    Console.WriteLine(board[i, j].TileColor + " " + (i + 1) + " " + (j + 1));
                    num++;
                }
            }
            Console.WriteLine($"{num} tiles created.");

        }

        public Coords FindCoordsOfID(int ID)
        {
            Coords pos = new Coords();

            for (int i = 0; i < 8; i++) // y
            {
                for (int j = 0; j < 8; j++) // x
                {
                    if (board[i, j].ID == ID)
                    {
                        pos = new Coords(j + 1, i + 1); // one offset because arrays start at 0
                    }
                }
            }

            return pos;
        }
    }

    class Piece
    {
        private bool initialMove = false;
        public Owner Owner { get; }

        public PieceType Type { get; set; }

        public Piece(Owner Owner, PieceType Type)
        {
            this.Owner = Owner;
            this.Type = Type;
        }
    }

}
