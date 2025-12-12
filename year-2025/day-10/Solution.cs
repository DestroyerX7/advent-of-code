using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2025.Day10;

[PuzzleName("Factory")]
public partial class Solution : Solver
{
    private static readonly Regex _indicatorRegex = IndicatorRegex();
    private static readonly Regex _buttonRegex = ButtonRegex();

    public override object SolvePartOne(string[] input)
    {
        // List<int> nums = [1, 2, 3];
        // var combos = GetCombinations(nums, 2);

        // foreach (var combo in combos)
        // {
        //     foreach (int num in combo)
        //     {
        //         Console.Write(num + ", ");
        //     }

        //     System.Console.WriteLine();
        // }

        // return combos.Count;

        // int[][] yo =
        // {
        //     [0,0,0,0,1,1,0],
        //     [0,1,0,0,0,1,1],
        //     [0,0,1,1,1,0,1],
        //     [1,1,0,1,0,0,0],
        //     // [0, 1, 0, 1],
        //     // [1, 0, 1, 0],
        //     // [1, 0, 0, 1],
        //     // [0, 0, 1, 0],
        //     // [1, 1, 0, 1],
        //     // [0, 1, 1, 0],
        //     // [1, 1, 1, 1],
        // };

        // AugmentedMatrix augmentedMatrix = new(yo);
        // augmentedMatrix.Format();
        // System.Console.WriteLine(augmentedMatrix);
        // return augmentedMatrix.Solve();

        foreach (string line in input)
        {
            string indicator = _indicatorRegex.Match(line).ToString();
            MatchCollection matchCollection = _buttonRegex.Matches(line);

            string yo = "[";

            for (int i = 0; i < indicator.Length - 2; i++)
            {
                yo += ".";
            }

            yo += "]";

            if (yo == indicator)
            {
                continue;
            }

            List<string> buttons = [];

            foreach (Match match in matchCollection)
            {
                buttons.Add(match.ToString());
            }

            List<int> buttonNums = [];
            for (int i = 0; i < buttonNums.Count; i++)
            {
                buttonNums.Add(i);
            }

            for (int i = 1; i < buttons.Count; i++)
            {
                string hi = yo;
                var combos = GetCombinations(buttonNums, i);

                foreach (List<int> combo in combos)
                {
                    foreach (int num in combo)
                    {
                        List<int> nums = [.. buttons[num].ToString().Trim('(', ')').Split(',').Select(int.Parse)];

                        if (hi[num + 1] == '.')
                        {
                            hi = hi[..(num + 1)] + '#' + hi[(num + 2)..];
                        }
                        else
                        {
                            hi = hi[..(num + 1)] + '.' + hi[(num + 2)..];
                        }
                    }

                    System.Console.WriteLine(hi);

                    if (hi == indicator)
                    {
                        System.Console.WriteLine("mathc " + i);
                    }
                }
            }
        }

        return "Not Attempted Yet";
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Attempted Yet";
    }

    private static List<List<int>> GetCombinations(List<int> original, int size)
    {
        List<List<int>> combos = [];

        if (size == 0)
        {
            return combos;
        }
        if (size == 1)
        {
            foreach (int num in original)
            {
                combos.Add([num]);
            }

            return combos;
        }

        for (int i = original.Count - 1; i >= 0; i--)
        {
            List<int> copy = [.. original];
            copy.RemoveAt(i);

            var yo = GetCombinations(copy, size - 1);

            for (int j = 0; j < yo.Count; j++)
            {
                yo[j] = [.. yo[j], original[i]];
            }

            foreach (var hi in yo)
            {
                combos.Add(hi);
            }
        }

        return combos;
    }

    [GeneratedRegex(@"\[[\.#]+\]")]
    private static partial Regex IndicatorRegex();

    [GeneratedRegex(@"\([\d,]*\d\)")]
    private static partial Regex ButtonRegex();
}

public class AugmentedMatrix
{
    private readonly int[][] _augmentedMatrix;
    private readonly int _rows;
    private readonly int _cols;

    public AugmentedMatrix(int[][] augmentedMatrix)
    {
        if (augmentedMatrix[0].Length < 2)
        {
            throw new Exception("Invalid augmented matrix dimensions.");
        }

        _augmentedMatrix = augmentedMatrix;
        _rows = augmentedMatrix.Length;
        _cols = augmentedMatrix[0].Length;
    }

    public void Format()
    {
        for (int x = 0; x < _cols - 1; x++)
        {
            // System.Console.WriteLine(this);

            int pivotIndex = 4;

            for (int y = x; y < _rows; y++)
            {
                if (_augmentedMatrix[y][x] != 0)
                {
                    int div = _augmentedMatrix[y][x];

                    for (int i = 0; i < _cols; i++)
                    {
                        _augmentedMatrix[y][i] /= div;
                    }

                    SwapRows(x, y);
                    break;
                }
            }

            for (int y = 0; y < _rows; y++)
            {
                if (y == x || x >= _rows)
                {
                    continue;
                }

                int sub;
                // System.Console.WriteLine(x);
                if (_augmentedMatrix[x][x] == 0)
                {
                    sub = _augmentedMatrix[y][x] / _augmentedMatrix[x][pivotIndex];
                }
                else
                {
                    sub = _augmentedMatrix[y][x] / _augmentedMatrix[x][x];
                }

                for (int i = x; i < _cols; i++)
                {
                    _augmentedMatrix[y][i] -= sub * _augmentedMatrix[x][i];
                }
            }
        }
    }

    public int Solve()
    {
        int sum = 0;

        for (int y = 0; y < _rows; y++)
        {
            if (_augmentedMatrix[y][_cols - 1] != 0)
            {
                sum++;
            }
        }

        return sum;
    }

    public void SwapRows(int indexOne, int indexTwo)
    {
        (_augmentedMatrix[indexTwo], _augmentedMatrix[indexOne]) = (_augmentedMatrix[indexOne], _augmentedMatrix[indexTwo]);
    }

    public override string ToString()
    {
        string output = "";

        for (int y = 0; y < _rows; y++)
        {
            output += "| ";

            for (int x = 0; x < _cols; x++)
            {
                output += _augmentedMatrix[y][x] + " ";

                if (x == _cols - 2)
                {
                    output += "| ";
                }
            }

            output += "|\n";
        }

        return output;
    }
}