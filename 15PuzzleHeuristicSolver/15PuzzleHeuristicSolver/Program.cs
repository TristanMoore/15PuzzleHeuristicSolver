using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _15PuzzleHeuristicSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int[] board1 = { 0, 1, -1, 2, 5, 6, 10, 3, 4, 8, 11, 7, 12, 9, 13, 14 };
            int[] board2 = { 0, 1, 2, 3, 11, 12, 13, 4, 10, -1, 14, 5, 9, 8, 7, 6 }; //spiral
            int[] board3 = { -1, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }; //backward
            Node init1 = new Node(board1, 4, 4, 0, NPuzzle.Operator.Root);
            Node init2 = new Node(board2, 4, 4, 0, NPuzzle.Operator.Root);
            Node init3 = new Node(board3, 4, 4, 0, NPuzzle.Operator.Root);
            NPuzzle puzz1 = new NPuzzle(init1);
            NPuzzle puzz2 = new NPuzzle(init2);
            NPuzzle puzz3 = new NPuzzle(init3);

            puzz1.SolveWithBestFirst();
            puzz1.PrintSolution();
            Console.WriteLine("Finding 'Best' Time: {0}", puzz1.debugStopwatch.Elapsed);

            puzz2.SolveWithBestFirst();
            puzz2.PrintSolution();
            Console.WriteLine("Finding 'Best' Time: {0}", puzz2.debugStopwatch.Elapsed);

            puzz3.SolveWithBestFirst();
            puzz3.PrintSolution();

            Console.WriteLine("\n\n\nDone. Press 'Enter' to terminate.");
            Console.ReadLine();
        }
    }

    //public class Node
    //{
    //    public int[] state;
    //    public Puzzle.Operator backPtr;
    //    public int hVal;

    //    public Node(int[] boardArr, int inversionCt)
    //    {
    //        state = boardArr;
    //        hVal = inversionCt;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        Node n = obj as Node;
    //        if (n == null)
    //        {
    //            return false;
    //        }
    //        bool b = Enumerable.SequenceEqual(this.state, n.state);
    //        return b;
    //    }

    //    public override string ToString()
    //    {
    //        StringBuilder stb = new StringBuilder();
    //        foreach (int tile in state)
    //        {
    //            stb.Append(tile);
    //            stb.Append(' ');
    //        }
    //        return stb.ToString();
    //    }

    //    public override int GetHashCode()
    //    {
    //        return this.ToString().GetHashCode();
    //    }

    //    public Node Copy()
    //    {
    //        return new Node((int[])state.Clone(), hVal);
    //    }
    //}
}