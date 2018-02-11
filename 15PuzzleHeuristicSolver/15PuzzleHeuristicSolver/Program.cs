using System;
using System.Collections.Generic;
using System.Linq;

namespace _15PuzzleHeuristicSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int[] spiralArr = new int[Puzzle.BOARD_SIZE] { 1, 2, Puzzle.BLANKVAL, 3, 6, 7, 11, 4, 5, 9, 12, 8, 13, 10, 14, 15 };
            var spiralPuzz = new Puzzle(spiralArr);
            //puzz.
        }
    }

    public class Puzzle
    {
        public const int BOARD_SIZE = 16;
        public const int BOARD_WIDTH = 4;
        public const int BLANKVAL = -1;
        private const int CHANGED_FLAG = -42;
        private static readonly int[] GOAL_STATE = new int[BOARD_SIZE] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, BLANKVAL };
        private int[] tileSet;

        public enum Operators { Up, Down, Left, Right };

        public Puzzle(int[] board)
        {
            if (!BoardIsValidAndSolvable(board))
            {
                tileSet = GOAL_STATE;
                throw new Exception("ERROR, invalid board passed.");
            }
            tileSet = board;
        }

        public List<Operators> ValidOperators
        {
            get
            {
                return Enum.GetValues(typeof(Operators)).Cast<Operators>().Where(op => OperatorIsValid(op)).ToList();
            }
        }

        private bool OperatorIsValid(Operators op)
        {
            switch (op)
            {
                case Operators.Up:
                    return BlankIndex > 3;

                case Operators.Down:
                    return BlankIndex < 12;

                case Operators.Left:
                    return BlankIndex % BOARD_WIDTH != 0;

                case Operators.Right:
                    return BlankIndex % BOARD_WIDTH != 3;

                default:
                    return false;
            }
        }

        public void DoOperator(Operators op)
        {
            if (!OperatorIsValid(op))
            {
                throw new Exception("ERROR, Attempted to perform invalid operator.");
            }

            int tmpIndex = BlankIndex;
            switch (op)
            {
                case Operators.Up:
                    tileSet[tmpIndex] = tileSet[tmpIndex - 4];
                    tileSet[tmpIndex - 4] = BLANKVAL;
                    break;

                case Operators.Down:
                    tileSet[tmpIndex] = tileSet[tmpIndex + 4];
                    tileSet[tmpIndex + 4] = BLANKVAL;
                    break;

                case Operators.Left:
                    tileSet[tmpIndex] = tileSet[tmpIndex - 1];
                    tileSet[tmpIndex - 1] = BLANKVAL;
                    break;

                case Operators.Right:
                    tileSet[tmpIndex] = tileSet[tmpIndex + 1];
                    tileSet[tmpIndex + 1] = BLANKVAL;
                    break;
            }

            buffBlankIndex = CHANGED_FLAG;
            buffInversionCount = CHANGED_FLAG;
        }

        private int buffBlankIndex = CHANGED_FLAG;

        public int BlankIndex
        {
            get
            {
                if (buffBlankIndex == CHANGED_FLAG)
                {
                    buffBlankIndex = FindBlankIndex(tileSet);
                }
                return buffBlankIndex;
            }
        }

        private int buffInversionCount = CHANGED_FLAG;

        public int InversionCount
        {
            get
            {
                if (buffInversionCount == CHANGED_FLAG)
                {
                    buffInversionCount = FindInversionCountInArray(tileSet);
                }
                return buffInversionCount;
            }
        }

        private static bool BoardIsValidAndSolvable(int[] boardArr)
        {
            if (boardArr.Length != BOARD_SIZE)
            {
                return false;
            }
            if (boardArr.Union(GOAL_STATE).Count() != boardArr.Intersect(GOAL_STATE).Count())
            {
                return false;
            }

            //If the blank is on an even row counting from the bottom(second-last, fourth - last etc), then the number of inversions in a solvable situation is odd.
            //If the blank is on an odd row counting from the bottom(last, third-last, fifth - last etc) then the number of inversions in a solvable situation is even.
            bool blankIsOnEvenRow = (FindBlankIndex(boardArr) / BOARD_WIDTH) % 2 == 0; //1 because its counting from the bottom
            bool inversionCountIsEven = FindInversionCountInArray(boardArr) % 2 == 0;
            return (blankIsOnEvenRow && !inversionCountIsEven) || (!blankIsOnEvenRow && inversionCountIsEven);
        }

        private static int FindBlankIndex(int[] boardArr)
        {
            return boardArr.Select((v, i) => new { Index = i, Value = v }).First(p => p.Value == BLANKVAL).Index;
        }

        private static int FindInversionCountInArray(int[] boardArr)
        {
            int invSum = 0;
            for (int i = 0; i < BOARD_SIZE - 1; i++)
            {
                for (int j = i + 1; j < BOARD_SIZE; j++)
                {
                    if (boardArr[i] != BLANKVAL && boardArr[j] != BLANKVAL)
                    {
                        if (boardArr[i] > boardArr[j])
                        {
                            invSum++;
                        }
                    }
                }
            }
            return invSum;
        }
    }
}