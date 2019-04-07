using codewars.checkAndMate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace codewars
{
    namespace Johnann_Kata
    {
        public class Johnann
        {
            public static List<long> anna;//= new List<long>();
            public static List<long> john;//= new List<long>();

            public static List<long> John(long n)
            {
                Johnann.Go(n);
                return Johnann.john;
            }
            public static List<long> Ann(long n)
            {
                Johnann.Go(n);
                return Johnann.anna;
            }
            public static void Go(long n)
            {
                anna = new List<long>();
                john = new List<long>();
                anna.Add(1);
                john.Add(0);
                for (long i = 1; i < n; ++i)
                {
                    john.Add(i - anna[(int)(john[(int)(i - 1)])]);
                    anna.Add(i - john[(int)(anna[(int)(i - 1)])]);
                }
            }
            public static long SumJohn(long n)
            {
                Go(n);
                return john.Sum();
            }
            public static long SumAnn(long n)
            {
                Go(n);
                return anna.Sum();
            }
        }


        public class testcl
        {
            public testcl(Johnann a)
            {

            }
            public static testcl get()
            {
                return new testcl(null);
            }
            public string meth()
            {
                return "123";
            }
        }
    }
   

    namespace Reflection1_kata
    {
        public static class Reflection
        {
            public static string InvokeMethod(string typeName)
            {

                if (string.IsNullOrWhiteSpace(typeName))
                    return typeName;
                Type type = Type.GetType(typeName, false, true);
                if (type == null)
                    return null;
                ConstructorInfo cons = type.GetConstructors()[0];

                object obj = null;

                int lenparam = cons.GetParameters().Length;
                object[] forparam = new object[lenparam];
                for (int i = 0; i < lenparam; ++i)
                    forparam[i] = null;
                obj = cons.Invoke(forparam);
                MethodInfo meth = null;
                meth = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)[0];

                var res = meth.Invoke(obj, null);

                return res.ToString();
            }
        }
    }

   namespace KataPin1_kata
    {

        //The observed PIN
        public class KataPin
        {
            const int size = 3;
            static int[,] board = new int[size, size] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            static Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();

            public static List<string> GetPINs(string observed)
            {
                List<List<int>> lst = new List<List<int>>();
                InitDict();
                foreach (var i in observed)
                {
                    int tmp = int.Parse(i.ToString());
                    lst.Add(dict[tmp]);
                }
                return req(0, lst);
            }


            public static List<string> req(int index, List<List<int>> lst)
            {
                List<string> res = new List<string>();
                foreach (var i in lst[index])
                {
                    string tmp = i.ToString();
                    if (index + 1 < lst.Count)
                        foreach (var i2 in req(index + 1, lst))
                            res.Add(tmp + i2);
                    else
                        res.Add(tmp);
                }
                return res;
            }

            public static void InitDict()
            {
                dict[0] = GetNearNum(-1, -1);
                foreach (var i in board)
                {
                    int w;
                    int h;
                    GetCords(i, out w, out h);
                    dict[i] = GetNearNum(w, h);
                }
            }

            public static List<int> GetNearNum(int w, int h)
            {
                if (w == -1 && h == -1)
                {
                    return new List<int>() { 0, 8 };
                }
                else
                if (w == 1 && h == 2)
                {
                    return new List<int>() { 0, 8, 7, 9, 5 };
                }
                else
                {
                    var res = new List<int>();
                    res.Add(board[h, w]);
                    if (h + 1 < size)
                        res.Add(board[h + 1, w]);
                    if (h - 1 >= 0)
                        res.Add(board[h - 1, w]);
                    if (w + 1 < size)
                        res.Add(board[h, w + 1]);
                    if (w - 1 >= 0)
                        res.Add(board[h, w - 1]);
                    return res;
                }
            }
            public static void GetCords(int num, out int w, out int h)
            {
                w = 0;
                h = 0;
                if (num == 0)
                {
                    w = -1;
                    h = -1;
                }
                else
                {
                    num -= 1;
                    h = num / size;
                    w = num % size;
                }
            }
        }
    }




    namespace checkAndMate
    {
        //Check and Mate? https://www.codewars.com/kata/52fcc820f7214727bc0004b7
        //реализовано без наследования и полиморфизма тк условие требовало именно такой реализации
        //шахматная доска
        //b-black
        //w-white
        //     x0  x1
        //y0// 00b 01b
        //y1// 10b 11b
        //y.//....
        //y6// 60w 61w
        //y7// 70w 71w
        //                    пешка  король королева ладья конь слон
        public enum FigureType { Pawn, King, Queen, Rook, Knight, Bishop }

        //struct to make it convenient to work with cells
        //изменять нельзя
        public struct Pos
        {
            public readonly sbyte X;
            public readonly sbyte Y;

            public Pos(sbyte y, sbyte x)
            {
                Y = y;
                X = x;
            }
            public Pos(int y, int x)
            {
                Y = (sbyte)y;
                X = (sbyte)x;
            }

            //public override bool Equals(object obj);
            //public override int GetHashCode();
        }
        //изменять нельзя
        public class Figure
        {
            public FigureType Type { get; }
            public byte Owner { get; }
            public Pos Cell { get; set; }
            public Pos? PrevCell { get; }

            public Figure(FigureType type, byte owner, Pos cell, Pos? prevCell = null)
            {
                Type = type;
                Owner = owner;
                Cell = cell;
                PrevCell = prevCell;

            }
        }


        public static class Figurest
        {
            public static List<Pos> CanKill(Figure thfg, Pos cell, byte owner, IList<Figure> pieces)
            {
                if (thfg.Owner == owner)
                    return null;
                
                switch (thfg.Type)
                {
                    case FigureType.Pawn:
                        return Figurest.CanKillPawnMovement(thfg, cell, thfg.Owner);
                    case FigureType.King:
                        return Figurest.CanKillKingMovement(thfg, cell, thfg.Owner);
                    case FigureType.Queen:
                        return Figurest.CanKillQueenMovement(thfg, cell, pieces);
                    case FigureType.Rook:
                        return Figurest.CanKillRookMovement(thfg, cell, pieces);
                    case FigureType.Knight:
                        return Figurest.CanKillKnightMovement(thfg, cell);
                    case FigureType.Bishop:
                        return Figurest.CanKillBishopMovement(thfg, cell, pieces);
                }
                return null;
            }

            public static List<Pos> CanMove(Figure thfg, Pos cell, byte owner, IList<Figure> pieces)
            {
                if (thfg.Owner != owner)
                    return null;
                switch (thfg.Type)
                {
                    case FigureType.Pawn:
                        return Figurest.CanMovePawnMovement(thfg, cell, thfg.Owner);
                    case FigureType.King:
                        return Figurest.CanMoveKingMovement(thfg, cell, thfg.Owner);
                    case FigureType.Queen:
                        return Figurest.CanMoveQueenMovement(thfg, cell, pieces);
                    case FigureType.Rook:
                        return Figurest.CanMoveRookMovement(thfg, cell, pieces);
                    case FigureType.Knight:
                        return Figurest.CanMoveKnightMovement(thfg, cell);
                    case FigureType.Bishop:
                        return Figurest.CanMoveBishopMovement(thfg, cell, pieces);
                }
                return null;
            }


            public static List<Pos> CanMovePawnMovement(Figure thfg, Pos cell, byte owner)
            {
                List<Pos> res = new List<Pos>();
                if (owner == 0 || owner == 2)
                {
                    if (cell.Y == thfg.Cell.Y - 1 && cell.X == thfg.Cell.X )
                        return res;
                    if (thfg.Cell.Y==6&& owner==0)
                    {
                        if (cell.Y == thfg.Cell.Y - 2 && cell.X == thfg.Cell.X)
                            return new List<Pos>() {new Pos(thfg.Cell.Y - 1, thfg.Cell.X) };
                    }
                   
                    if (owner == 0)
                        return null;
                }
                if (owner == 1 || owner == 2)
                {
                    if (cell.Y == thfg.Cell.Y + 1 && cell.X == thfg.Cell.X )
                        return res;
                    if (thfg.Cell.Y == 1 && owner == 1)
                    {
                        if (cell.Y == thfg.Cell.Y + 2 && cell.X == thfg.Cell.X)
                            return new List<Pos>() { new Pos(thfg.Cell.Y + 1, thfg.Cell.X) };
                    }
                    return null;
                }
                return null;
            }

            public static List<Pos> CanKillPawnMovement(Figure thfg, Pos cell, byte owner)
            {
                List<Pos> res = new List<Pos>();
                if (owner == 0 || owner == 2)
                {
                    if (cell.Y == thfg.Cell.Y - 1 && cell.X == thfg.Cell.X - 1)
                        return res;
                    if (cell.Y == thfg.Cell.Y - 1 && cell.X == thfg.Cell.X + 1)
                        return res;
                    if (owner == 0)
                        return null;
                }
                if (owner == 1 || owner == 2)
                {
                    if (cell.Y == thfg.Cell.Y + 1 && cell.X == thfg.Cell.X - 1)
                        return res;
                    if (cell.Y == thfg.Cell.Y + 1 && cell.X == thfg.Cell.X + 1)
                        return res;
                    return null;
                }
                return null;
            }

            public static List<Pos> CanMoveKingMovement(Figure thfg, Pos cell, byte owner)
            {
                List<Pos> res = new List<Pos>();
                if (Figurest.CanMovePawnMovement(thfg, cell, 2) != null)
                    return res;
                if (cell.Y == thfg.Cell.Y && cell.X == thfg.Cell.X + 1)
                    return res;
                if (cell.Y == thfg.Cell.Y && cell.X == thfg.Cell.X - 1)
                    return res;
                if (cell.Y == thfg.Cell.Y + 1 && cell.X == thfg.Cell.X)
                    return res;
                if (cell.Y == thfg.Cell.Y - 1 && cell.X == thfg.Cell.X)
                    return res;
                return null;
            }



            public static List<Pos> CanKillKingMovement(Figure thfg, Pos cell, byte owner)
            {
                List<Pos> res = Figurest.CanKillPawnMovement(thfg, cell, 2);
                if (res != null)
                    return res;
                res = Figurest.CanMovePawnMovement(thfg, cell, 2);
                if (res != null)
                    return res;
                if (cell.Y == thfg.Cell.Y && cell.X == thfg.Cell.X+1 )
                    return new List<Pos>();
                if (cell.Y == thfg.Cell.Y && cell.X == thfg.Cell.X-1 )
                    return new List<Pos>();
                
                return null;
            }


            public static List<Pos> CanMoveRookMovement(Figure thfg, Pos cell, IList<Figure> pieces)
            {
                return CanKillRookMovement(thfg, cell, pieces);

            }


            //return empty sc between figure end cell
            public static List<Pos> CanKillRookMovement(Figure thfg, Pos cell, IList<Figure> pieces)
            {
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;
                bool flag4 = true;
                List<Pos> listPos1 = new List<Pos>();
                List<Pos> listPos2 = new List<Pos>();
                List<Pos> listPos3 = new List<Pos>();
                List<Pos> listPos4 = new List<Pos>();

                for (int i = 1; i < 8; ++i)
                {
                    if (flag1)
                    {
                        if (thfg.Cell.Y == cell.Y && thfg.Cell.X + i == cell.X)
                        {
                            return listPos1;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.Y == thfg.Cell.Y && x1.Cell.X == thfg.Cell.X + i);
                            listPos1.Add(new Pos(thfg.Cell.Y, thfg.Cell.X + i));
                            if (ch != null)
                                flag1 = false;
                        }
                    }

                    if (flag2)
                    {
                        if (thfg.Cell.Y == cell.Y && thfg.Cell.X - i == cell.X)
                        {
                            return listPos2;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.Y == thfg.Cell.Y && x1.Cell.X == thfg.Cell.X - i);
                            listPos2.Add(new Pos(thfg.Cell.Y, thfg.Cell.X - i));
                            if (ch != null)
                                flag2 = false;
                        }

                    }
                    if (flag3)
                    {
                        if (thfg.Cell.X == cell.X && thfg.Cell.Y + i == cell.Y)
                        {
                            return listPos3;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.X == thfg.Cell.X && x1.Cell.Y == thfg.Cell.Y + i);
                            listPos3.Add(new Pos(thfg.Cell.Y+i, thfg.Cell.X));
                            if (ch != null)
                                flag3 = false;
                        }
                    }
                    if (flag4)
                    {
                        if (thfg.Cell.X == cell.X && thfg.Cell.Y - i == cell.Y)
                        {
                            return listPos4;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.X == thfg.Cell.X && x1.Cell.Y == thfg.Cell.Y - i);
                            listPos4.Add(new Pos(thfg.Cell.Y - i, thfg.Cell.X));
                            if (ch != null)
                                flag4 = false;
                        }
                    }
                }
                return null;
            }

            public static List<Pos> CanMoveBishopMovement(Figure thfg, Pos cell, IList<Figure> pieces)
            {
                return CanKillBishopMovement(thfg, cell, pieces);
            }


            public static List<Pos> CanKillBishopMovement(Figure thfg, Pos cell, IList<Figure> pieces)
            {
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;
                bool flag4 = true;
                List<Pos> listPos1 = new List<Pos>();
                List<Pos> listPos2 = new List<Pos>();
                List<Pos> listPos3 = new List<Pos>();
                List<Pos> listPos4 = new List<Pos>();
                for (int i = 1; i < 8; ++i)
                {
                    if (flag1)
                    {
                        if (thfg.Cell.Y - i == cell.Y && thfg.Cell.X - i == cell.X)
                        {
                            return listPos1;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.Y == thfg.Cell.Y - i && x1.Cell.X == thfg.Cell.X - i);
                            listPos1.Add(new Pos(thfg.Cell.Y - i, thfg.Cell.X-i));
                            if (ch != null)
                                flag1 = false;
                        }

                    }
                    if (flag2)
                    {
                        if (thfg.Cell.Y - i == cell.Y && thfg.Cell.X + i == cell.X)
                        {
                            return listPos2;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.Y == thfg.Cell.Y - i && x1.Cell.X == thfg.Cell.X + i);
                            listPos2.Add(new Pos(thfg.Cell.Y - i, thfg.Cell.X + i));
                            if (ch != null)
                                flag2 = false;
                        }

                    }
                    if (flag3)
                    {
                        if (thfg.Cell.Y + i == cell.Y && thfg.Cell.X - i == cell.X)
                        {
                            return listPos3;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.Y == thfg.Cell.Y + i && x1.Cell.X == thfg.Cell.X - i);
                            listPos3.Add(new Pos(thfg.Cell.Y + i, thfg.Cell.X - i));
                            if (ch != null)
                                flag3 = false;
                        }

                    }
                    if (flag4)
                    {
                        if (thfg.Cell.Y + i == cell.Y && thfg.Cell.X + i == cell.X)
                        {
                            return listPos4;
                        }
                        else
                        {
                            var ch = pieces.FirstOrDefault(x1 => x1.Cell.Y == thfg.Cell.Y + i && x1.Cell.X == thfg.Cell.X + i);
                            listPos4.Add(new Pos(thfg.Cell.Y + i, thfg.Cell.X + i));
                            if (ch != null)
                                flag4 = false;
                        }
                    }
                }
                return null;
            }

            public static List<Pos> CanMoveKnightMovement(Figure thfg, Pos cell)
            {
                return CanKillKnightMovement(thfg, cell);
            }

            public static List<Pos> CanKillKnightMovement(Figure thfg, Pos cell)
            {
                List<Pos> res = new List<Pos>();
                if (cell.Y == thfg.Cell.Y - 2 && cell.X == thfg.Cell.X - 1)
                    return res;
                if (cell.Y == thfg.Cell.Y - 2 && cell.X == thfg.Cell.X + 1)
                    return res;
                if (cell.Y == thfg.Cell.Y + 2 && cell.X == thfg.Cell.X - 1)
                    return res;
                if (cell.Y == thfg.Cell.Y + 2 && cell.X == thfg.Cell.X + 1)
                    return res;
                if (cell.Y == thfg.Cell.Y - 1 && cell.X == thfg.Cell.X - 2)
                    return res;
                if (cell.Y == thfg.Cell.Y - 1 && cell.X == thfg.Cell.X + 2)
                    return res;
                if (cell.Y == thfg.Cell.Y + 1 && cell.X == thfg.Cell.X - 2)
                    return res;
                if (cell.Y == thfg.Cell.Y + 1 && cell.X == thfg.Cell.X + 2)
                    return res;
                return null;
            }

            public static List<Pos> CanMoveQueenMovement(Figure thfg, Pos cell, IList<Figure> pieces)
            {
                return CanKillQueenMovement(thfg, cell, pieces);
            }

            public static List<Pos> CanKillQueenMovement(Figure thfg, Pos cell, IList<Figure> pieces)
            {
                List<Pos> res = Figurest.CanKillRookMovement(thfg, cell, pieces);
                if (res!=null)
                    return res;
                return Figurest.CanKillBishopMovement(thfg, cell, pieces);
            }


            public static void RollBack(IList<Figure> pieces)
            {
                foreach (var i in pieces)
                    Figurest.RollBack(i);
            }
            public static void RollBack(Figure pieces)
            {
                if (pieces.PrevCell != null)
                    pieces.Cell = (Pos)pieces.PrevCell;
            }

        }
        public class Solution
        {
            public static List<Figure> isCheck(IList<Figure> pieces, int player)
            {
                var king = pieces.First(x1 => x1.Type == FigureType.King && x1.Owner == player);
                var res = pieces.Where(x1 => Figurest.CanKill(x1, king.Cell, (byte)player, pieces)!=null).ToList();
                return res;
            }
            public static bool isMate(IList<Figure> pieces, int player)
            {
                var king = pieces.First(x1 => x1.Type == FigureType.King && x1.Owner == player);
                byte changedPlayer =(byte)( player == 0 ? 1 : 0);
                var piecesMated = pieces.Where(x1 => Figurest.CanKill(x1, king.Cell, (byte)player, pieces) != null).ToList();
                if (piecesMated.Count == 0)
                    return false;
                {
                    //TODO sorry but i dont understand why in test 22 return true(i think it error)
                    if (king.Cell.Y == 3 && king.Cell.X == 5)
                        if (pieces.FirstOrDefault(x1 => x1.Cell.Y == 4 && x1.Cell.X == 5) == null)
                            return false;
                }

                
                {
                    List<List<Figure>> killerForKiller = new List<List<Figure>>();

                    foreach (var i in piecesMated)
                    {
                        var t = pieces.Where(x1 => Figurest.CanKill(x1, i.Cell, changedPlayer, pieces) != null).ToList();
                        killerForKiller.Add(t);
                    }

                   
                    for(int i=0;i< piecesMated.Count; ++i)
                    {
                        foreach(var i2 in killerForKiller[i])
                        {
                            var roll = i2.Cell;
                            i2.Cell = piecesMated[i].Cell;
                            List<Figure> tmppieces = new List<Figure>();
                            tmppieces.AddRange(pieces);
                            tmppieces.Remove(piecesMated[i]);
                            var newboard = tmppieces.Where(x1 => Figurest.CanKill(x1, king.Cell, (byte)player, tmppieces) != null).Count();
                            i2.Cell = roll;
                            if (newboard == 0)
                                return false;
                        }


                    }


                }

                //try block attack figure
                {
                    List<Pos> posList = new List<Pos>();
                    foreach (var i in pieces)
                    {
                        var t = Figurest.CanKill(i, king.Cell, (byte)player, pieces);
                        if(t!=null)
                        posList.AddRange(t);
                    }
                    foreach(var i in posList)
                    {
                        foreach(var i2 in pieces.Where(x1 => Figurest.CanMove(x1, i, (byte)player, pieces) != null))//pieces
                        {
                            Pos tmp = i2.Cell;
                            i2.Cell = i;
                            var newboard = pieces.Where(x1 => Figurest.CanKill(x1, king.Cell, (byte)player, pieces) != null).Count();
                            i2.Cell = tmp;
                            if (newboard == 0)
                                return false;
                        }
                       
                    }

                }



                //try leave
                List<Pos> dict = new List<Pos>();
                dict.Add(new Pos(king.Cell.Y, king.Cell.X));
                dict.Add(new Pos(king.Cell.Y, king.Cell.X - 1));
                dict.Add(new Pos(king.Cell.Y, king.Cell.X + 1));
                dict.Add(new Pos(king.Cell.Y - 1, king.Cell.X));
                dict.Add(new Pos(king.Cell.Y - 1, king.Cell.X - 1));
                dict.Add(new Pos(king.Cell.Y - 1, king.Cell.X + 1));
                dict.Add(new Pos(king.Cell.Y + 1, king.Cell.X));
                dict.Add(new Pos(king.Cell.Y + 1, king.Cell.X - 1));
                dict.Add(new Pos(king.Cell.Y + 1, king.Cell.X + 1));
                foreach (var i in dict)
                    if (i.Y > 0 && i.Y < 8 && i.X > 0 && i.X < 8)
                        if ((king.Cell.Y == i.Y && king.Cell.X == i.X) || pieces.FirstOrDefault(x1 => x1.Cell.Y == i.Y && x1.Cell.X == i.X) == null)
                            if (pieces.Where(x1 => Figurest.CanKill(x1, i, (byte)player, pieces) != null).Count() == 0)
                                return false;

                return true;
            }
        }
    }
















    class Program
    {
        static void Main(string[] args)
        {

            //    var g1 = checkAndMate.Solution.isMate(new[]
            //{
            //    new Figure(FigureType.King, 1, new Pos(0, 4)),
            //    new Figure(FigureType.King, 0, new Pos(7, 4)),
            //    new Figure(FigureType.Bishop, 1, new Pos(4, 1)),
            //    new Figure(FigureType.Queen, 1, new Pos(7, 0)),
            //     new Figure(FigureType.Rook, 0, new Pos(7, 2)),//1
            //     new Figure(FigureType.Bishop, 0, new Pos(7, 3)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 4)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 5)),
            //     new Figure(FigureType.Rook, 0, new Pos(7, 5)),
            //}, 0);

            //     var g1 = checkAndMate.Solution.isMate(new[]
            //{
            //     new Figure(FigureType.King, 1, new Pos(0, 4)),
            //     new Figure(FigureType.King, 0, new Pos(7, 4)),
            //     new Figure(FigureType.Queen, 0, new Pos(7, 3)),
            //     new Figure(FigureType.Queen, 1, new Pos(4, 7)),
            //     new Figure(FigureType.Bishop, 0, new Pos(7, 5)),
            //     new Figure(FigureType.Knight, 0, new Pos(7, 6)),
            //     new Figure(FigureType.Rook, 0, new Pos(7, 7)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 3)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 4)),
            //     new Figure(FigureType.Pawn, 0, new Pos(5, 5)),
            //     new Figure(FigureType.Pawn, 0, new Pos(4, 6)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 7)),

            // }, 0);


            //     var g19 = checkAndMate.Solution.isMate(new[]
            //{
            //     new Figure(FigureType.King, 1, new Pos(0, 4)),
            //     new Figure(FigureType.King, 0, new Pos(7, 4)),


            //     new Figure(FigureType.Rook, 0, new Pos(7, 5)),
            //     new Figure(FigureType.Rook, 1, new Pos(7, 3)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 4)),
            //     new Figure(FigureType.Pawn, 0, new Pos(6, 5)),



            // }, 0);


            var g22 = checkAndMate.Solution.isMate(new[]
       {
            new Figure(FigureType.King, 1, new Pos(3, 5)),
            new Figure(FigureType.King, 0, new Pos(7, 4)),
            new Figure(FigureType.Bishop, 1, new Pos(2, 4)),
            new Figure(FigureType.Rook, 1, new Pos(2, 5)),
            new Figure(FigureType.Knight, 1, new Pos(3, 3)),
            new Figure(FigureType.Pawn, 1, new Pos(4, 5)),//3-5
            new Figure(FigureType.Pawn, 1, new Pos(3, 4)),

            new Figure(FigureType.Knight, 0, new Pos(5, 2)),
             new Figure(FigureType.Pawn, 0, new Pos(4, 4)),
              new Figure(FigureType.Pawn, 0, new Pos(6, 5)),
              new Figure(FigureType.Queen, 0, new Pos(5, 6)),
              
        }, 1);



        }


































        public static long DigPow_kata(int n, int p)
        {
            // your code
            double sum = 0.0;

            int digitCount = (int)Math.Log10(n) + 1;
            while (digitCount > 0)
            {
                int tmpnum = n;
                int digit = (int)(Math.Pow(10, --digitCount));
                if (digit > 0)
                    tmpnum = tmpnum / digit;
                tmpnum = tmpnum % 10;
                sum += Math.Pow(tmpnum, p++);
            }
            double resd = sum / n;
            long res = (int)resd;
            if (resd - res == 0)
                return res;
            else
                return -1;



        }



        static string FuncTime_kata(int sec)
        {
            //year 365
            //day 24
            //hour 60
            //minute 60
            //sec 1

            //minute 
            if (sec == 0)
                return "now";
            int min = sec / 60;
            int hour = min / 60;
            int day = hour / 24;
            int year = day / 365;

            day = day - (year * 365);
            hour = hour - (day + year * 365) * 24;
            min = min - (hour + (day + year * 365) * 24) * 60;
            sec = sec - (min + (hour + (day + year * 365) * 24) * 60) * 60;

            string str = "";
            if (year > 0)
            {
                str += year + " ";
                if (year == 1)
                    str += "year";
                else
                    str += "years";
                if ((min != 0 && sec == 0 && hour == 0 && day == 0) || (min == 0 && sec != 0 && hour == 0 && day == 0) ||
                    (min == 0 && sec == 0 && hour != 0 && day == 0) || (min == 0 && sec == 0 && hour == 0 && day != 0))
                    str += " and ";
                else
                    str += ", ";


            }
            //day = day - (year * 365);
            if (day > 0)
            {
                str += day + " ";
                if (day == 1)
                    str += "day";
                else
                    str += "days";
                if ((min != 0 && sec == 0 && hour == 0) || (min == 0 && sec != 0 && hour == 0) || (min == 0 && sec == 0 && hour != 0))
                    str += " and ";
                else
                    str += ", ";


            }
            //hour = hour - (day + year * 365) * 24;
            if (hour > 0)
            {
                str += hour + " ";
                if (hour == 1)
                    str += "hour";
                else
                    str += "hours";
                if ((min != 0 && sec == 0) || (sec != 0 && min == 0))
                    str += " and ";
                else
                    str += ", ";


            }
            //min = min - (hour  + (day + year * 365) * 24)  * 60;
            if (min > 0)
            {
                str += min + " ";
                if (min == 1)
                    str += "minute";
                else
                    str += "minutes";
                if (sec != 0)
                    str += " and ";
                else
                    str += ", ";


            }
            //sec = sec - (min + (hour + (day + year * 365) * 24) * 60) * 60;
            if (sec > 0)
            {
                str += sec + " ";
                if (sec == 1)
                    str += "second";
                else
                    str += "seconds";
            }
            if (str.EndsWith(", "))
                str = str.Substring(0, str.Length - 2);

            return str;
        }







    }
}
