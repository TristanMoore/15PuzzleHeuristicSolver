﻿using System;

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

            puzz2.SolveWithBestFirst();
            puzz2.PrintSolution();

            puzz3.SolveWithBestFirst();
            puzz3.PrintSolution();

            Console.WriteLine("\n\n\nDone. Press 'Enter' to terminate.");
            Console.ReadLine();
        }
    }
}