using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Lib;

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

    // This reddit post helped a little
    // https://www.reddit.com/r/adventofcode/comments/1pjghln/2025_day_10_part_2_is_the_problem_possible_to/
    // I was already on the right track using matrix row reduction
    // But this confirmed it and told me to brute force the number of presses once I had the equations with free vars
    public override object SolvePartTwo(string[] input)
    {
        int totalPresses = 0;

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

            List<string> equations = augmentedMatrix.GetEquations();

            int maxTest = joltageNums.Max();
            int min = FindMin(equations, maxTest);

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

    private static int FindMin(List<string> equations, int maxTest)
    {
        string? freeVar = null;

        foreach (string equation in equations)
        {
            Match match = _equationVarRegex.Match(equation);

            if (match.Success)
            {
                freeVar = match.ToString();
                break;
            }
        }

        if (string.IsNullOrEmpty(freeVar))
        {
            int sum = 0;

            foreach (string equation in equations)
            {
                double total = equation.Split(' ').Select(x =>
                {
                    if (double.TryParse(x, out double num))
                    {
                        return num;
                    }
                    else
                    {
                        double[] nums = [.. x.Split('*').Select(double.Parse)];
                        return nums[0] * nums[1];
                    }
                }).Aggregate((a, b) => a + b);

                total = double.Round(total, 3);

                if (total < 0 || !double.IsInteger(total))
                {
                    return -1;
                }

                sum += (int)total;
            }

            return sum;
        }

        int min = int.MaxValue - maxTest;

        for (int i = 0; i <= maxTest; i++)
        {
            List<string> replaced = [.. equations.Select(e => e.Replace(freeVar, i.ToString()))];

            int result = FindMin(replaced, maxTest);

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
