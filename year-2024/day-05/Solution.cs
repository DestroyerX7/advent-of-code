using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day05;

[PuzzleName("Print Queue")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Dictionary<int, HashSet<int>> rules = new();
        int middleSum = 0;

        foreach (string line in input)
        {
            if (line.Contains('|'))
            {
                int[] rule = line.Split('|').Select(int.Parse).ToArray();

                if (rules.ContainsKey(rule[0]))
                {
                    rules[rule[0]].Add(rule[1]);
                }
                else
                {
                    rules[rule[0]] = new()
                    {
                        rule[1]
                    };
                }
            }
            else if (line.Contains(','))
            {
                int[] numsToAdd = line.Split(',').Select(int.Parse).ToArray();
                bool inOrder = IsOrdered(numsToAdd, rules);

                if (inOrder)
                {
                    middleSum += numsToAdd[numsToAdd.Length / 2];
                }
            }
        }

        return middleSum;
    }

    public override object SolvePartTwo(string[] input)
    {
        Dictionary<int, HashSet<int>> rules = new();
        int middleSumUnordered = 0;

        foreach (string line in input)
        {
            if (line.Contains('|'))
            {
                int[] rule = line.Split('|').Select(int.Parse).ToArray();

                if (rules.ContainsKey(rule[0]))
                {
                    rules[rule[0]].Add(rule[1]);
                }
                else
                {
                    rules[rule[0]] = new()
                    {
                        rule[1]
                    };
                }
            }
            else if (line.Contains(','))
            {
                int[] numsToAdd = line.Split(',').Select(int.Parse).ToArray();
                bool inOrder = IsOrdered(numsToAdd, rules);

                if (!inOrder)
                {
                    Reorder(numsToAdd, rules);
                    middleSumUnordered += numsToAdd[numsToAdd.Length / 2];
                }
            }
        }

        return middleSumUnordered;
    }

    private static void Reorder(int[] nums, Dictionary<int, HashSet<int>> rules)
    {
        bool inOrder = false;

        // Swaps numbers that are not in the right order one at a time until they are in order.
        while (!inOrder)
        {
            // Looks for a number that should be before any of the numbers in rules and then swaps them.
            for (int i = 0; i < nums.Length; i++)
            {
                if (rules.ContainsKey(nums[i]))
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (rules[nums[i]].Contains(nums[j]))
                        {
                            (nums[i], nums[j]) = (nums[j], nums[i]);
                            break;
                        }
                    }
                }
            }

            inOrder = IsOrdered(nums, rules);
        }
    }

    private static bool IsOrdered(int[] nums, Dictionary<int, HashSet<int>> rules)
    {
        HashSet<int> numsAdded = new();
        foreach (int num in nums)
        {
            if (rules.ContainsKey(num) && numsAdded.Any(n => rules[num].Contains(n)))
            {
                return false;
            }

            numsAdded.Add(num);
        }

        return true;
    }
}