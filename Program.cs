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
            Console.BackgroundColor = ConsoleColor.Blue;
            Chess.Board b = new Chess.Board();

            //b.DisplayBoard();
            //b.DisplayPieces();

            Console.WriteLine($"The chess notation of 3, 3 is: {Coords.ToChessNotation(new Coords(3, 3))}");
            Console.WriteLine($"The coords of {Coords.ToChessNotation(new Coords(3, 3))} is: {Coords.ToCoords("d3")}");


            //Console.WriteLine($"The coords of tile 36 are {b.FindCoordsOfID(36)}");

            //Console.WriteLine($"The tile ID of 7,7 is: {b.FindTileOfCoords(new Coords(7, 7)).ID}");

            b.MovePiece(new Coords(0, 1), new Coords(0, 3));
            b.MovePiece(new Coords(7, 1), new Coords(7, 3));
            //b.DisplayPieces();
            b.MovePiece(Coords.ToCoords("c2"), Coords.ToCoords("c4"));
            b.DisplayTable();

            
        }
    }

    enum PieceType
    {
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
        Pawn,
        Empty
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
        A = 0,
        B, // 1
        C, // 2
        D, // 3
        E, // 4
        F, // 5
        G, // 6
        H  // 7
    }

    enum Owner
    {
        White,
        Black,
        Empty
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
        public static string ToChessNotation(Coords coords)
        {
            string res;
            switch (coords.X)
            {
                case (int)ChessNotation.A: // https://stackoverflow.com/questions/943398/get-int-value-from-enum-in-c-sharp
                    res = $"a{coords.Y}";
                    break;

                case (int)ChessNotation.B:
                    res = $"b{coords.Y}";
                    break;

                case (int)ChessNotation.C:
                    res = $"c{coords.Y}";
                    break;

                case (int)ChessNotation.D:
                    res = $"d{coords.Y}";
                    break;

                case (int)ChessNotation.E:
                    res = $"e{coords.Y}";
                    break;

                case (int)ChessNotation.F:
                    res = $"f{coords.Y}";
                    break;

                case (int)ChessNotation.G:
                    res = $"g{coords.Y}";
                    break;

                case (int)ChessNotation.H:
                    res = $"h{coords.Y}";
                    break;

                default:
                    return "err";
                    break;
            }

            return res;
        }
        
        public static Coords ToCoords(string notation)
        {
            Coords res = new Coords();
            int y = (int)Char.GetNumericValue(notation[1]) - 1; // -1 for the offset

            switch (notation[0])
            {
                case 'a':
                    res = new Coords(0, y);
                    break;

                case 'b':
                    res = new Coords(1, y);
                    break;

                case 'c':
                    res = new Coords(2, y);
                    break;

                case 'd':
                    res = new Coords(3, y);
                    break;

                case 'e':
                    res = new Coords(4, y);
                    break;

                case 'f':
                    res = new Coords(5, y);
                    break;

                case 'g':
                    res = new Coords(6, y);
                    break;

                case 'h':
                    res = new Coords(7, y);
                    break;

                default:
                    res = new Coords(-1, -1);
                    Console.WriteLine("error, please enter valid chess notation");
                    break;
            }

            return res;
        }
    }

    class Tile
    {
        public ConsoleColor TileColor { get; set; }
        public int ID { get; set; }
        public Piece Piece { get; set; }
        public Coords Coords { get; set; }

        public Tile(int ID, ConsoleColor TileColor, Coords Coords)
        {
            this.ID = ID;
            this.TileColor = TileColor;
            this.Coords = Coords;
            this.Piece = new Piece(new Coords(-1, -1), Owner.Empty);
        }
    }

    /// <summary>
    /// Remember! Board is 0-7, not 1-8, remember the offset.
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
            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x
                {
                    cnt++;
                    board[x, y] = new Tile(cnt, ((x + y) % 2 == 0) ? ConsoleColor.Black : ConsoleColor.White, new Coords(x, y));
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
                    Console.WriteLine($"{board[x, y].TileColor} ID: {board[x, y].ID} Coords: {x} {y} Piece: {board[x, y].Piece}");
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
                    board[x, y].Piece = new Piece(new Coords(x, y), Owner.Black, DeterminePieceType(x, y, Owner.Black));
                }
            }

            // white set
            for (int y = 6; y < 8; y++) // two iterations, one for each row
            {
                for (int x = 0; x < 8; x++) // 8 iterations, one for each column
                {
                    board[x, y].Piece = new Piece(new Coords(x, y), Owner.White, DeterminePieceType(x, y, Owner.White));
                }
            }

        }

        public void DisplayPieces()
        {
            for (int y = 0; y < 8; y++) // y
            {
                for (int x = 0; x < 8; x++) // x 
                {
                    Console.WriteLine($"{((board[x, y].Piece == null) ? "Empty" : board[x, y].Piece.Type)} with POS: {board[x, y].Coords}");
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
                    if (board[x, y].ID == ID)
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
                    if (board[x, y].Coords.X == coords.X && board[x, y].Coords.Y == coords.Y) // get around comparing objects, probs a better way but my brain hurts enough. https://stackoverflow.com/a/26349452 maybe?
                    {
                        res = board[x, y];
                        //Console.WriteLine($"success {x} and {y}");
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
            //Console.WriteLine(finalPoint.Coords);
            startingPoint.Piece = new Piece(new Coords(-1, -1), Owner.Empty); // empty pos
        }

        public void DisplayTable()
        {
            // TODO: need to add chess notation columns
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("| ||0||1||2||3||4||5||6||7|");
            for (int y = 7; y >= 0; y--) // going backwards because we're working top-down
            {
                Console.Write($"|{y}|");
                //Console.WriteLine(y);
                for (int x = 0; x < 8; x++)
                {
                    //Console.Write($"X: {x} Y: {y}");
                    Console.BackgroundColor = FindTileOfCoords(new Coords(x, y)).TileColor;
                    Console.Write($"|{GetPieceShorthand(new Coords(x, y))}|");
                    Console.BackgroundColor = ConsoleColor.Blue;
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Blue;
        }

        public string GetPieceShorthand(Coords coords)
        {
            string res;

            switch (board[coords.X, coords.Y].Piece.Type)
            {
                case PieceType.Rook:
                    res = "R";
                    break;

                case PieceType.Knight:
                    res = "K";
                    break;

                case PieceType.Bishop:
                    res = "B";
                    break;

                case PieceType.Queen:
                    res = "Q";
                    break;

                case PieceType.King:
                    res = "X";
                    break;

                case PieceType.Pawn:
                    res = "P";
                    break;

                case PieceType.Empty:
                    res = "O";
                    break;

                default:
                    res = "O";
                    break;

            }

            return res;
        }

    }

    class Piece
    {
        public Owner Owner { get; }
        public Coords Home { get; } // original location of piece

        private PieceType type = PieceType.Empty;
        public PieceType Type { get { return this.type; } set { this.type = value; } }

        public Piece(Coords Home, Owner Owner, PieceType Type = PieceType.Empty)
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

            //Console.WriteLine(json);

            Piece output = (Piece)JsonSerializer.Deserialize<Piece>(json);

            return output; // https://stackoverflow.com/a/58294305
        }
    }

}
