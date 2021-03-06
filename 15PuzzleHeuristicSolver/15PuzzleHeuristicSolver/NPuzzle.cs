﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace _15PuzzleHeuristicSolver
{
    public class NPuzzle
    {
        public const int BLANKVAL = -1;
        public readonly Node goalState;
        public readonly Node initialState;
        public List<Node> solution;
        private Stopwatch calcStopwatch = new Stopwatch();
        private int openCount = 0;
        private int closeCount = 0;

        public enum Operator { Root, Up, Down, Left, Right };

        public NPuzzle(Node init)
        {
            initialState = init;
            int[] tmp = new int[init.state.Length];
            goalState = new Node(tmp, init.rows, init.columns, 0, Operator.Root);
            for (int i = 0; i < goalState.state.Length; i++)
            {
                goalState.state[i] = i;
            }
            goalState.state[goalState.state.Length - 1] = BLANKVAL;
        }

        public List<Node> SolveWithAStar(int stepCost)
        {
            Dictionary<Node, int> openHash = new Dictionary<Node, int>();
            HashSet<Node> closeHash = new HashSet<Node>();
            SortedList<int, Node> open = new SortedList<int, Node>(new DuplicateKeyComparer<int>());
            Node best;

            //0. start the timer
            calcStopwatch.Reset();
            calcStopwatch.Start();

            //1. put IS on open
            open.Add(initialState.HVal, initialState);
            openHash.Add(initialState, initialState.HVal);

            while (true)
            {
                //2. if open is empty, fail
                if (!open.Any())
                {
                    openCount = open.Count();
                    closeCount = closeHash.Count();
                    calcStopwatch.Stop();
                    return null;
                }

                //pick out best from open and put on close
                best = open.First().Value;
                openHash.Remove(best);
                open.RemoveAt(0);
                closeHash.Add(best);

                //3. expand n
                foreach (Operator op in GetValidOperators(best))
                {
                    Node successor = ApplyOperator(op, best, stepCost);
                    if (successor.Equals(goalState))
                    {
                        //4. if successor is goal, output solution
                        openCount = open.Count();
                        closeCount = closeHash.Count();
                        solution = GenerateSolutionPath(closeHash.ToList(), successor);
                        calcStopwatch.Stop();
                        return solution;
                    }

                    if (!closeHash.Contains(successor) && !openHash.ContainsKey(successor))
                    {
                        open.Add(successor.HVal, successor);
                        openHash.Add(successor, successor.HVal);
                    }
                }
            }
        }

        public void PrintSolution()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Calculation Time: {0}", calcStopwatch.Elapsed);
            Console.WriteLine("Open Count: {0}", openCount);
            Console.WriteLine("Close Count: {0}", closeCount);
            Console.WriteLine("Total Nodes Generated: {0}", openCount + closeCount);
            Console.WriteLine("Length of Solution Path: {0}", solution.Count());
            Console.WriteLine("Solution:");
            foreach (Node n in solution)
            {
                PrintNode(n);
            }
        }

        public static void PrintNode(Node n)
        {
            StringBuilder stb = new StringBuilder();

            stb.AppendLine(new string('-', 20));
            stb.AppendLine(string.Format("Operator: {0}", n.appliedOp.ToString()));
            for (int i = 0; i < n.state.Length; i++)
            {
                if (i % n.columns == 0 && i != 0)
                {
                    stb.AppendLine();
                }
                stb.AppendFormat("{0} ", (n.state[i] + 1).ToString().PadLeft(2));
            }
            stb.AppendLine();
            stb.AppendLine(new string('-', 20));

            Console.WriteLine(stb);
        }

        private static List<Node> GenerateSolutionPath(List<Node> close, Node finalState)
        {
            List<Node> solution = new List<Node>();

            //backtrack through closed to get a path
            solution.Add(finalState);
            Node tmp = finalState;
            while (tmp.appliedOp != Operator.Root)
            {
                tmp = ApplyReverseOperator(tmp.appliedOp, tmp, 0);
                solution.Add(tmp);
                tmp = close.First(n => n.Equals(tmp));
            }

            //reverse the backwards path
            solution.First().appliedOp = Operator.Root;
            foreach (Node n in solution)
            {
                n.appliedOp = ReverseOperator(n.appliedOp);
            }
            solution.Reverse();

            return solution;
        }

        private static Operator ReverseOperator(Operator op)
        {
            switch (op)
            {
                case Operator.Up:
                    return Operator.Down;

                case Operator.Down:
                    return Operator.Up;

                case Operator.Left:
                    return Operator.Right;

                case Operator.Right:
                    return Operator.Left;

                case Operator.Root:
                default:
                    return op;
            }
        }

        private static Node ApplyReverseOperator(Operator op, Node n, int stepCost)
        {
            return ApplyOperator(ReverseOperator(op), n, stepCost);
        }

        private static Node ApplyOperator(Operator op, Node n, int stepCost) //assumes operation is valid
        {
            int blankIndex = FindBlankIndex(n);
            int[] tmp = new int[n.state.Length];
            n.state.CopyTo(tmp, 0);
            switch (op)
            {
                case Operator.Up:
                    tmp[blankIndex] = tmp[blankIndex - n.columns];
                    tmp[blankIndex - n.columns] = BLANKVAL;
                    break;

                case Operator.Down:
                    tmp[blankIndex] = tmp[blankIndex + n.columns];
                    tmp[blankIndex + n.columns] = BLANKVAL;
                    break;

                case Operator.Left:
                    tmp[blankIndex] = tmp[blankIndex - 1];
                    tmp[blankIndex - 1] = BLANKVAL;
                    break;

                case Operator.Right:
                    tmp[blankIndex] = tmp[blankIndex + 1];
                    tmp[blankIndex + 1] = BLANKVAL;
                    break;

                case Operator.Root:
                default:
                    break;
            }

            return new Node(tmp, n.rows, n.columns, n.distance + stepCost, op);
        }

        private static List<Operator> GetValidOperators(Node n)
        {
            return Enum.GetValues(typeof(Operator)).Cast<Operator>().Where(op => OperatorIsValid(op, n)).ToList();
        }

        private static bool OperatorIsValid(Operator op, Node n)
        {
            int blankIndex = FindBlankIndex(n);

            switch (op)
            {
                case Operator.Up:
                    return blankIndex >= n.columns;

                case Operator.Down:
                    return blankIndex < n.state.Length - n.columns;

                case Operator.Left:
                    return blankIndex % n.columns != 0;

                case Operator.Right:
                    return blankIndex % n.columns != n.columns - 1;

                default:
                    return false;
            }
        }

        private static int FindBlankIndex(Node n)
        {
            for (int i = 0; i < n.rows * n.columns; i++)
            {
                if (n.state[i] == BLANKVAL)
                {
                    return i;
                }
            }
            throw new Exception("Blank not found.");
        }

        public static int FindHeuristic(Node n)
        {
            return FindManhattanDistance(n) + FindLinearConflict(n);
        }

        public static int FindManhattanDistance(Node n)
        {
            int sum = 0;
            for (int i = 0; i < n.rows * n.columns; i++)
            {
                if (n.state[i] != BLANKVAL)
                {
                    int currRow = i / n.rows;
                    int currCol = i % n.columns;
                    int destRow = n.state[i] / n.rows;
                    int destCol = n.state[i] % n.columns;
                    sum += Math.Abs(currRow - destRow);
                    sum += Math.Abs(currCol - destCol);
                }
            }
            return sum;
        }

        public static int FindLinearConflict(Node n)
        {
            int sum = 0;
            for (int j = 0; j < n.rows; j++)
            {
                for (int k = 0; k < n.rows; k++)
                {
                    int jCurrRow = n.state[j] / n.columns;
                    int kCurrRow = n.state[k] / n.columns;
                    int jGoalRow = j / n.columns;
                    int kGoalRow = k / n.columns;
                    bool allOnSameRow = jCurrRow == kCurrRow && kCurrRow == jGoalRow && jGoalRow == kGoalRow;
                    bool jIsRightOfK = n.state[k] < n.state[j];
                    bool jGoalIsLeftOfGoalK = j < k;
                    if (allOnSameRow && jIsRightOfK && jGoalIsLeftOfGoalK)
                    {
                        sum += 2;
                    }
                }
            }
            return sum;
        }
    }

    public class Node
    {
        public NPuzzle.Operator appliedOp;
        public int[] state;
        public readonly int rows;
        public readonly int columns;
        public readonly int heuristic;
        public readonly int distance;
        public int HVal { get { return heuristic + distance; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="boardArry">Make this 0 based indexed. -1 is the blank.</param>
        /// <param name="rows">1 indexed based</param>
        /// <param name="colums">1 indexed based</param>
        public Node(int[] boardArry, int rows, int colums, int currentDistance, NPuzzle.Operator previousOp)
        {
            this.rows = rows;
            this.columns = colums;
            state = boardArry;
            heuristic = NPuzzle.FindHeuristic(this);
            distance = currentDistance;
            appliedOp = previousOp;
        }

        public override bool Equals(object obj)
        {
            Node n = obj as Node;
            if (n == null)
            {
                return false;
            }
            bool b = Enumerable.SequenceEqual(this.state, n.state);
            return b;
        }

        public override int GetHashCode()
        {
            StringBuilder stb = new StringBuilder();
            foreach (int tile in state)
            {
                stb.Append(tile);
                stb.Append(' ');
            }
            return stb.ToString().GetHashCode();
        }
    }

    //allows duplicates. Used to pick the 'best' n in open.
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);
            if (result == 0)
            {
                return 1; //Handle equality as being greater
            }
            else
            {
                return result;
            }
        }
    }
}