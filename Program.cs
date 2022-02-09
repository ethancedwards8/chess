using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {

            Chess.Board b = new Chess.Board();

            b.DisplayBoard();
            b.DisplayPieces();

            Console.WriteLine(Coords.ToChessNotation(3, 3));

            Console.WriteLine($"The coords of tile 36 are {b.FindCoordsOfID(36)}");

            Console.WriteLine(b.FindTileOfCoords(new Coords(7, 7)).ID);

            b.MovePiece(new Coords(6, 1), new Coords(5, 2));
            b.DisplayPieces();
        }
    }

    enum PieceType
    {
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
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
        public Coords Coords { get; set; }

        public Tile(int ID, Color TileColor, Coords Coords)
        {
            this.ID = ID;
            this.TileColor = TileColor;
            this.Coords = Coords;
        }
    }

    /// <summary>
    /// Remember! Board is 0-7, not 1-8, account for the offset.
    /// </summary>
    class Board
    {
        private Tile[,] board = new Tile[8, 8];

        public Board()
        {
            CreateBoard();
            PlacePieces();
        }

        public void CreateBoard() // also serves as a board "reset"
        {
            int cnt = 0;
            bool ticker = false;

            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x
                {
                    cnt++;
                    board[y, x] = new Tile(cnt, ticker ? Color.White : Color.Black, new Coords(x, y));
                    ticker = !ticker;
                }
            }
        }

        public void DisplayBoard()
        {
            int num = 0;

            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x 
                {
                    Console.WriteLine($"{board[y, x].TileColor} {board[y, x].ID} {y + 1} {x + 1} {board[y, x].Piece}");
                    num++;
                }
            }
            Console.WriteLine($"{num} tiles created.");

        }

        public void PlacePieces()
        {
            // this could probably be put into one loop


            // black set
            for (int y = 0; y < 2; y++) // two iterations, one for each row
            {
                for (int x = 0; x < 8; x++) // 8 iterations, one for each column
                {
                    board[y, x].Piece = new Piece(new Coords(x, y), Owner.Black, DeterminePieceType(x, y, Owner.Black));
                }
            }

            // white set
            for (int y = 6; y < 8; y++) // two iterations, one for each row
            {
                for (int x = 0; x < 8; x++) // 8 iterations, one for each column
                {
                    board[y, x].Piece = new Piece(new Coords(x, y), Owner.White, DeterminePieceType(x, y, Owner.White));
                }
            }

        }

        public void DisplayPieces()
        {
            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x 
                {
                    Console.WriteLine($"{((board[y, x].Piece == null) ? "Empty" : board[y, x].Piece.Type)} with POS: {board[y, x].Coords}");
                }
            }
        }

        public PieceType DeterminePieceType(int x, int y, Owner owner) // this is horrible, but I think it works, I couldn't think of an algorithmically decent way to do this
        {
            PieceType res;

            switch (owner)
            {
                case Owner.Black:
                    switch (y)
                    {
                        case 0:
                            if (x == 0 || (x % 7 == 0))
                                res = PieceType.Rook;
                            else if (x == 1 || (x % 6 == 0))
                                res = PieceType.Knight;
                            else if (x == 2 || (x % 5 == 0))
                                res = PieceType.Bishop;
                            else if (x == 3)
                                res = PieceType.Queen;
                            else
                                res = PieceType.King;
                            break;

                        case 1:
                            res = PieceType.Pawn;
                            break;

                        default:
                            res = PieceType.Pawn;
                            break;

                    }

                    break;

                case Owner.White:
                    switch (y)
                    {
                        case 7:
                            if (x == 0 || (x % 7 == 0))
                                res = PieceType.Rook;
                            else if (x == 1 || (x % 6 == 0))
                                res = PieceType.Knight;
                            else if (x == 2 || (x % 5 == 0))
                                res = PieceType.Bishop;
                            else if (x == 3)
                                res = PieceType.Queen;
                            else
                                res = PieceType.King;
                            break;

                        case 6:
                            res = PieceType.Pawn;
                            break;

                        default:
                            res = PieceType.Pawn;
                            break;

                    }
                    break;

                default:
                    res = PieceType.Pawn;
                    break;
            }

            return res;
        }

        public Coords FindCoordsOfID(int ID) // probably won't be very useful
        {
            Coords pos = new Coords();

            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x
                {
                    if (board[y, x].ID == ID)
                    {
                        pos = new Coords(x, y); 
                    }
                }
            }

            return pos;
        }

        public Tile FindTileOfCoords(Coords coords)
        {
            Tile res = board[0, 0]; // fixes error, unnecessary 

            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x
                {
                    if (board[y, x].Coords.X == coords.X && board[y, x].Coords.Y == coords.Y) // get around comparing objects, probs a better way but my brain hurts enough. https://stackoverflow.com/a/26349452 maybe?
                    {
                        res = board[y, x];
                        Console.WriteLine($"success {x} and {y}");
                        return res;
                    }
                    else
                    {
                        res = board[0, 0]; // return 0, 0 if cannot find coords
                        //Console.WriteLine($"hit default {x} and {y}");
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Method solely moves piece, does not do checking, should be handled elsewhere...
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        public void MovePiece(Coords start, Coords destination)
        {
            Tile startingPoint = FindTileOfCoords(start);
            Tile finalPoint = FindTileOfCoords(destination);

            Piece test = startingPoint.Piece.DeepCopy(startingPoint.Piece);
            

            finalPoint.Piece = test; // re-assign pos
            Console.WriteLine(finalPoint.Coords);
            startingPoint.Piece = null; // empty pos
        }
    }

    class Piece
    {
        public Owner Owner { get; }
        public Coords Home { get; } // original location of piece

        public PieceType Type { get; set; }

        public Piece(Coords Home, Owner Owner, PieceType Type)
        {
            this.Home = Home;
            this.Owner = Owner;
            this.Type = Type;
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone?redirectedfrom=MSDN&view=net-6.0#System_Object_MemberwiseClone  DeepCopy!!!
        /// <summary>
        /// Perform deep copy of object
        /// </summary>
        /// <returns>The copied object</returns>
        public Piece DeepCopy(Piece source) // https://stackoverflow.com/a/58294305
        {
            var json = JsonSerializer.Serialize(source);

            Console.WriteLine(json);

            Piece output = (Piece)JsonSerializer.Deserialize<Piece>(json);

            return output; // https://stackoverflow.com/a/58294305
        }
    }

}
