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
    private static readonly Regex _equationVarRegex = EquationVarRegex();

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

            double[][] matrix = new double[joltageNums.Length][];

            for (int y = 0; y < joltageNums.Length; y++)
            {
                matrix[y] = new double[buttons.Count + 1];
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
            augmentedMatrix.SetMul();

            List<string> equations = augmentedMatrix.GetEquations();

            int maxTest = joltageNums.Max();
            long min = FindMin(equations, augmentedMatrix.Mul, maxTest);

            totalPresses += min;
        }

        return totalPresses;
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

    private static long FindMin(List<string> equations, double mul, int maxTest)
    {
        HashSet<string> freeVars = [];

        foreach (string equation in equations)
        {
            MatchCollection matchCollection = _equationVarRegex.Matches(equation);

            foreach (Match match in matchCollection)
            {
                freeVars.Add(match.ToString());
            }
        }

        if (freeVars.Count == 0)
        {
            double sum = 0;

            foreach (string equation in equations)
            {
                double total = equation.Split(' ').Select(x =>
                {
                    if (double.TryParse(x, out double num))
                    {
                        return num / mul;
                    }
                    else
                    {
                        int index = x.IndexOf('*');
                        long coef = long.Parse(x[..index]);
                        num = double.Parse(x[(index + 1)..]);

                        return coef * num / mul;
                    }
                }).Aggregate((a, b) => a + b);

                total = double.Round(total, 3);

                if (total < 0 || !double.IsInteger(total))
                {
                    return -1;
                }

                sum += total;
            }

            return (long)sum;
        }

        long min = long.MaxValue - maxTest;

        for (long i = 0; i <= maxTest; i++)
        {
            string freeVar = freeVars.First();

            List<string> replaced = [.. equations.Select(e => e.Replace(freeVar, i.ToString()))];

            long result = FindMin(replaced, mul, maxTest);

            if (result >= 0 && result + i < min)
            {
                min = result + i;
            }
        }

        return min;
    }

    [GeneratedRegex(@"\[[\.#]+\]")]
    private static partial Regex IndicatorRegex();

    [GeneratedRegex(@"\([\d,]*\d\)")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"{[\d,]*\d}")]
    private static partial Regex JoltageRegex();

    [GeneratedRegex(@"x_\d+")]
    private static partial Regex EquationVarRegex();
}

public class AugmentedMatrix
{
    private readonly double[][] _augmentedMatrix;
    private readonly int _rows;
    private readonly int _cols;

    private readonly List<double> _yo = [];
    public double Mul = 1;

    public AugmentedMatrix(double[][] augmentedMatrix)
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
                        double inverse = 1 / _augmentedMatrix[y][x];

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

                    double val = _augmentedMatrix[y][pivotIndex];

                    for (int x = pivotIndex; x < _cols; x++)
                    {
                        _augmentedMatrix[y][x] -= val * _augmentedMatrix[pivotRow][x];
                    }
                }
            }
        }
    }

    public void SetMul()
    {
        Mul = 1;

        foreach (double num in _yo)
        {
            Mul *= 1 / Math.Abs(num);
        }

        Mul = double.Round(Mul, 3);

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _cols; x++)
            {
                _augmentedMatrix[y][x] = double.Round(_augmentedMatrix[y][x] * Mul, 3);

                if (_augmentedMatrix[y][x] == 0)
                {
                    _augmentedMatrix[y][x] = 0;
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
            _augmentedMatrix[y][x] = _augmentedMatrix[y][x] * multiplier;

            _yo.Add(multiplier);
        }
    }

    public List<string> GetEquations()
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
                    equation += $"{Math.Abs(_augmentedMatrix[y][x])}*x_{x} ";
                }
            }

            if (!string.IsNullOrEmpty(equation))
            {
                equations.Add(equation.Trim());
            }
        }

        return equations;
    }

    public bool IsConsistent()
    {
        for (int y = _augmentedMatrix.Length - 1; y >= 0; y--)
        {
            if (_augmentedMatrix[y][_cols - 1] != 0 && _augmentedMatrix[y].Take(_cols - 1).All(n => n == 0))
            {
                return false;
            }
        }

        return true;
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

        return output.Trim('\n');
    }
}