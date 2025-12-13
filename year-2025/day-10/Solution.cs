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
    private static readonly Regex _joltageRegex = JoltageRegex();

    public override object SolvePartOne(string[] input)
    {
        int totalPresses = 0;

        foreach (string line in input)
        {
            string indicator = _indicatorRegex.Match(line).ToString().Trim('[', ']');
            MatchCollection matchCollection = _buttonRegex.Matches(line);

            List<string> buttons = [];
            foreach (Match match in matchCollection)
            {
                buttons.Add(match.ToString().Trim('(', ')'));
            }

            for (int i = 1; i < buttons.Count; i++)
            {
                bool found = false;
                List<List<string>> buttonCombos = GetCombinations(buttons, i);

                foreach (List<string> buttonCombo in buttonCombos)
                {
                    string current = new('.', indicator.Length);

                    foreach (string button in buttonCombo)
                    {
                        int[] nums = [.. button.Split(',').Select(int.Parse)];

                        foreach (int num in nums)
                        {
                            if (current[num] == '.')
                            {
                                current = current[..num] + "#" + current[(num + 1)..];
                            }
                            else
                            {
                                current = current[..num] + "." + current[(num + 1)..];
                            }
                        }
                    }

                    if (current == indicator)
                    {
                        totalPresses += i;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }
        }

        return totalPresses;
    }

    public override object SolvePartTwo(string[] input)
    {
        long totalPresses = 0;

        foreach (string line in input)
        {
            MatchCollection matchCollection = _buttonRegex.Matches(line);

            List<string> buttons = [];
            foreach (Match match in matchCollection)
            {
                buttons.Add(match.ToString().Trim('(', ')'));
            }

            int[] joltageNums = [.. _joltageRegex.Match(line).ToString().Trim('{', '}').Split(',').Select(int.Parse)];

            int[][] matrix = new int[joltageNums.Length][];

            for (int y = 0; y < joltageNums.Length; y++)
            {
                matrix[y] = new int[buttons.Count + 1];
                matrix[y][buttons.Count] = joltageNums[y];
            }

            for (int x = 0; x < buttons.Count; x++)
            {
                int[] nums = [.. buttons[x].Split(',').Select(int.Parse)];

                foreach (int num in nums)
                {
                    matrix[num][x] = 1;
                }
            }

            AugmentedMatrix augmentedMatrix = new(matrix);
            augmentedMatrix.RowReduce();

            List<string> equations = augmentedMatrix.GetEquation();

            var yo = FindMin(equations);

            totalPresses += yo.Item1;
        }

        return totalPresses;

        // int[][] matrix =
        // {
        //     // [1, 0, 1, 1],
        //     // [1, 1, 0, 0],
        //     // [0, 0, 1, 0],
        //     [0,0,0,0,1,1,3],
        //     [0,1,0,0,0,1,5],
        //     [0,0,1,1,1,0,4],
        //     [1,1,0,1,0,0,7],
        // };

        // AugmentedMatrix augmentedMatrix = new(matrix);
        // augmentedMatrix.RowReduce();

        // System.Console.WriteLine(augmentedMatrix);

        // List<string> equations = augmentedMatrix.GetEquation();

        // var yo = FindMin(equations);

        // return yo.Item1;
    }

    private static List<List<T>> GetCombinations<T>(List<T> original, int size)
    {
        List<List<T>> combos = [];

        if (size == 0)
        {
            return combos;
        }
        if (size == 1)
        {
            foreach (T item in original)
            {
                combos.Add([item]);
            }

            return combos;
        }

        for (int i = 0; i < original.Count; i++)
        {
            List<List<T>> smallerCombos = GetCombinations([.. original.Skip(i + 1)], size - 1);

            for (int j = 0; j < smallerCombos.Count; j++)
            {
                smallerCombos[j] = [original[i], .. smallerCombos[j]];
            }

            combos = [.. combos.Concat(smallerCombos)];
        }

        return combos;
    }

    private static (int, bool) FindMin(List<string> strings)
    {
        HashSet<string> freeVars = [];

        foreach (string equation in strings)
        {
            Regex regex = new(@"x\d+");
            MatchCollection matchCollection = regex.Matches(equation);

            foreach (Match match in matchCollection)
            {
                freeVars.Add(match.ToString());
            }
        }

        if (freeVars.Count == 0)
        {
            int sum = 0;

            foreach (string equation in strings)
            {
                int total = equation.Split(' ').Select(int.Parse).Aggregate((a, b) => a + b);

                if (total < 0)
                {
                    return (-1, false);
                }

                sum += total;
            }

            return (sum, true);
        }

        int min = int.MaxValue - 10000;

        for (int i = 0; i < 5; i++)
        {
            string freeVar = freeVars.ToArray()[0];

            List<string> copy = [.. strings];

            for (int j = 0; j < copy.Count; j++)
            {
                copy[j] = copy[j].Replace(freeVar, i.ToString());
            }

            var yo = FindMin(copy);

            int hi = yo.Item1 + i;

            if (yo.Item2 && hi < min)
            {
                min = hi;
            }
        }

        return (min, true);
    }

    [GeneratedRegex(@"\[[\.#]+\]")]
    private static partial Regex IndicatorRegex();

    [GeneratedRegex(@"\([\d,]*\d\)")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"{[\d,]*\d}")]
    private static partial Regex JoltageRegex();
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

    public void RowReduce()
    {
        for (int pivotRow = 0; pivotRow < _rows; pivotRow++)
        {
            int pivotIndex = -1;

            for (int x = pivotRow; x < _cols - 1 && pivotIndex == -1; x++)
            {
                for (int y = pivotRow; y < _rows && pivotIndex == -1; y++)
                {
                    if (_augmentedMatrix[y][x] != 0)
                    {
                        int inverse = 1 / _augmentedMatrix[y][x];

                        MultiplyRow(y, inverse);
                        SwapRows(y, pivotRow);

                        pivotIndex = x;
                    }
                }
            }

            if (pivotIndex != -1)
            {
                for (int y = 0; y < _rows; y++)
                {
                    if (y == pivotRow)
                    {
                        continue;
                    }

                    int val = _augmentedMatrix[y][pivotIndex];

                    for (int x = pivotIndex; x < _cols; x++)
                    {
                        _augmentedMatrix[y][x] -= val * _augmentedMatrix[pivotRow][x];
                    }
                }
            }
        }
    }

    private void SwapRows(int indexOne, int indexTwo)
    {
        (_augmentedMatrix[indexTwo], _augmentedMatrix[indexOne]) = (_augmentedMatrix[indexOne], _augmentedMatrix[indexTwo]);
    }

    private void MultiplyRow(int y, double multiplier)
    {
        for (int x = 0; x < _cols; x++)
        {
            _augmentedMatrix[y][x] = (int)(_augmentedMatrix[y][x] * multiplier);
        }
    }

    public List<string> GetEquation()
    {
        List<string> equations = [];

        for (int y = _rows - 1; y >= 0; y--)
        {
            bool foundPivot = false;

            string equation = "";

            for (int x = 0; x < _cols - 1; x++)
            {
                if (_augmentedMatrix[y][x] != 0 && !foundPivot)
                {
                    equation += $"{_augmentedMatrix[y][_cols - 1]} ";
                    foundPivot = true;
                }
                else if (_augmentedMatrix[y][x] != 0)
                {
                    equation += _augmentedMatrix[y][x] > 0 ? "-" : "+";

                    equation += $"x{x} ";
                }
            }

            if (!string.IsNullOrEmpty(equation))
            {
                equations.Add(equation.Trim());
            }
        }

        return equations;
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