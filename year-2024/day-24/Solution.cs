using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day24;

[PuzzleName("Crossed Wires")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Dictionary<string, int> dict = new();

        int i = 0;
        while (input[i].Length != 0)
        {
            string[] split = input[i].Split(": ");
            dict[split[0]] = int.Parse(split[1]);
            i++;
        }

        HashSet<string[]> operations = new();

        while (i < input.Length - 1)
        {
            i++;

            string[] split = input[i].Split(' ');

            operations.Add(split);

            CheckOperations(operations, dict);
        }

        char[] bits = dict.Where(k => k.Key.StartsWith('z')).OrderByDescending(k => k.Key).Select(k => k.Value.ToString()[0]).ToArray();
        string binaryNum = new(bits);
        return Convert.ToInt64(binaryNum, 2);
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Solved Yet";
    }

    private static void CheckOperations(HashSet<string[]> operations, Dictionary<string, int> dict)
    {
        foreach (string[] operation in operations)
        {
            if (dict.ContainsKey(operation[0]) && dict.ContainsKey(operation[2]))
            {
                switch (operation[1])
                {
                    case "AND":
                        dict[operation[4]] = dict[operation[0]] & dict[operation[2]];
                        break;
                    case "OR":
                        dict[operation[4]] = dict[operation[0]] | dict[operation[2]];
                        break;
                    case "XOR":
                        dict[operation[4]] = dict[operation[0]] ^ dict[operation[2]];
                        break;
                }

                operations.Remove(operation);

                CheckOperations(operations, dict);
            }
        }
    }
}