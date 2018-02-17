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
            //int[] test = { 0, 2, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, -1 };
            //Node init = new Node(test, 4, 4);

            //int lc = NPuzzle.FindLinearConflict(init); //should be 2
            //int md = NPuzzle.FindManhattanDistance(init); //should be 2

            int[] board1 = { 0, 1, -1, 2, 5, 6, 10, 3, 4, 8, 11, 7, 12, 9, 13, 14 };
            int[] board2 = { 0, 1, 2, 3, 11, 12, 13, 4, 10, -1, 14, 5, 9, 8, 7, 6 }; //spiral
            int[] board3 = { -1, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }; //backward
            Node init1 = new Node(board1, 4, 4, 0, NPuzzle.Operator.Root);
            Node init2 = new Node(board2, 4, 4, 0, NPuzzle.Operator.Root);
            Node init3 = new Node(board3, 4, 4, 0, NPuzzle.Operator.Root);
            NPuzzle puzz1 = new NPuzzle(init1);
            NPuzzle puzz2 = new NPuzzle(init2);
            NPuzzle puzz3 = new NPuzzle(init3);

            var n1 = NPuzzle.FindHeuristic(init1);
            var n2 = NPuzzle.FindHeuristic(init2);
            var n3 = NPuzzle.FindHeuristic(init3);

            var solution1 = puzz1.SolveWithBestFirst();
            foreach (Node n in solution1)
            {
                NPuzzle.PrintNode(n);
            }

            var solution2 = puzz2.SolveWithBestFirst();
            foreach (Node n in solution2)
            {
                NPuzzle.PrintNode(n);
            }

            var solution3 = puzz3.SolveWithBestFirst();
            foreach (Node n in solution3)
            {
                NPuzzle.PrintNode(n);
            }
        }
    }

    //public class Puzzle
    //{
    //    public const int BOARD_SIZE = 16;
    //    public const int BOARD_WIDTH = 4;
    //    public const int BLANKVAL = -1;
    //    private const int CHANGED_FLAG = -42;
    //    private static readonly int[] GOAL_STATE = new int[BOARD_SIZE] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, BLANKVAL };
    //    private int[] currTileSet;

    //    public enum Operator { Up, Down, Left, Right };

    //    public Puzzle(int[] board)
    //    {
    //        if (!BoardIsValidAndSolvable(board))
    //        {
    //            currTileSet = GOAL_STATE;
    //            throw new Exception("ERROR, invalid board passed.");
    //        }
    //        currTileSet = board;
    //    }

    //    public static List<Operator> FindValidOperators(Node n)
    //    {
    //        return Enum.GetValues(typeof(Operator)).Cast<Operator>().Where(op => OperatorIsValid(op, n)).ToList();
    //    }

    //    private static bool OperatorIsValid(Operator op, Node n)
    //    {
    //        int blankIndex = FindBlankIndex(n.state);

    //        switch (op)
    //        {
    //            case Operator.Up:
    //                return blankIndex > 3;

    //            case Operator.Down:
    //                return blankIndex < 12;

    //            case Operator.Left:
    //                return blankIndex % BOARD_WIDTH != 0;

    //            case Operator.Right:
    //                return blankIndex % BOARD_WIDTH != 3;

    //            default:
    //                return false;
    //        }
    //    }

    //    public Node DoOperator(Operator op, Node n)
    //    {
    //        Node nCopy = n.Copy();
    //        if (!OperatorIsValid(op, nCopy))
    //        {
    //            throw new Exception("ERROR, Attempted to perform invalid operator.");
    //        }

    //        int tmpIndex = FindBlankIndex(nCopy.state);
    //        switch (op)
    //        {
    //            case Operator.Up:
    //                nCopy.state[tmpIndex] = nCopy.state[tmpIndex - 4];
    //                nCopy.state[tmpIndex - 4] = BLANKVAL;
    //                break;

    //            case Operator.Down:
    //                nCopy.state[tmpIndex] = nCopy.state[tmpIndex + 4];
    //                nCopy.state[tmpIndex + 4] = BLANKVAL;
    //                break;

    //            case Operator.Left:
    //                nCopy.state[tmpIndex] = nCopy.state[tmpIndex - 1];
    //                nCopy.state[tmpIndex - 1] = BLANKVAL;
    //                break;

    //            case Operator.Right:
    //                nCopy.state[tmpIndex] = nCopy.state[tmpIndex + 1];
    //                nCopy.state[tmpIndex + 1] = BLANKVAL;
    //                break;
    //        }

    //        return nCopy;
    //    }

    //    public Node DoReverseOperator(Operator op, Node n)
    //    {
    //        switch (op)
    //        {
    //            case Operator.Up:
    //                return DoOperator(Operator.Down, n);

    //            case Operator.Down:
    //                return DoOperator(Operator.Up, n);

    //            case Operator.Left:
    //                return DoOperator(Operator.Left, n);

    //            case Operator.Right:
    //                return DoOperator(Operator.Right, n);
    //        }

    //        return n;
    //    }

    //    private static bool BoardIsValidAndSolvable(int[] boardArr)
    //    {
    //        if (boardArr.Length != BOARD_SIZE)
    //        {
    //            return false;
    //        }
    //        if (boardArr.Union(GOAL_STATE).Count() != boardArr.Intersect(GOAL_STATE).Count())
    //        {
    //            return false;
    //        }

    //        //If the blank is on an even row counting from the bottom(second-last, fourth - last etc), then the number of inversions in a solvable situation is odd.
    //        //If the blank is on an odd row counting from the bottom(last, third-last, fifth - last etc) then the number of inversions in a solvable situation is even.
    //        bool blankIsOnEvenRow = (FindBlankIndex(boardArr) / BOARD_WIDTH) % 2 == 0; //1 because its counting from the bottom
    //        bool inversionCountIsEven = FindInversionCountInArray(boardArr) % 2 == 0;
    //        return (blankIsOnEvenRow && !inversionCountIsEven) || (!blankIsOnEvenRow && inversionCountIsEven);
    //    }

    //    private static int FindBlankIndex(int[] boardArr)
    //    {
    //        return boardArr.Select((v, i) => new { Index = i, Value = v }).First(p => p.Value == BLANKVAL).Index;
    //    }

    //    private static int FindInversionCountInArray(int[] boardArr)
    //    {
    //        int invSum = 0;
    //        for (int i = 0; i < BOARD_SIZE - 1; i++)
    //        {
    //            for (int j = i + 1; j < BOARD_SIZE; j++)
    //            {
    //                if (boardArr[i] != BLANKVAL && boardArr[j] != BLANKVAL)
    //                {
    //                    if (boardArr[i] > boardArr[j])
    //                    {
    //                        invSum++;
    //                    }
    //                }
    //            }
    //        }
    //        return invSum;
    //    }

    //    public List<Operator> SolveWithBestFirst()
    //    {
    //        List<Operator> solution = new List<Operator>();
    //        List<Node> open = new List<Node>();
    //        List<Node> close = new List<Node>();
    //        Node goal = new Node(GOAL_STATE, 0);
    //        Node initialState = new Node(currTileSet, FindInversionCountInArray(currTileSet));

    //        //put IS on open
    //        open.Add(initialState);

    //        while (true)
    //        {
    //            //if open is empty, fail
    //            if (!open.Any())
    //            {
    //                return null;
    //            }

    //            //take best from open and put on close
    //            open = open.OrderBy(n => n.hVal).ToList();
    //            var best = open[0];
    //            open.RemoveAt(0);
    //            close.Add(best);

    //            //expand best and add successors to open
    //            var validOps = FindValidOperators(best);
    //            foreach (Operator op in validOps)
    //            {
    //                var n = DoOperator(op, best);
    //                if (!close.Any(tmp => tmp.Equals(n)) && !open.Any(tmp => tmp.Equals(n)))
    //                {
    //                    n.backPtr = op;
    //                    n.hVal = FindInversionCountInArray(n.state);
    //                    open.Add(n);

    //                    if (n == goal)
    //                    {
    //                        return TraverseSolution(initialState, n, close);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public List<Operator> TraverseSolution(Node initialState, Node goalState, List<Node> closed)
    //    {
    //        List<Operator> solution = new List<Operator>();
    //        Node curr = goalState;

    //        while (curr != initialState)
    //        {
    //            solution.Add(curr.backPtr);
    //            curr = DoReverseOperator(curr.backPtr, curr);
    //            curr = closed.First(n => n.Equals(curr));
    //        }

    //        solution.Reverse();
    //        return solution;
    //    }
    //}

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