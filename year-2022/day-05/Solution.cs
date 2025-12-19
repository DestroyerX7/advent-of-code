using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day05;

[PuzzleName("Supply Stacks")]
public class Solution : Solver
{
    // private static List<List<char>> lists = new List<List<char>>
    //     {
    //         new List<char> { 'F', 'C', 'J', 'P', 'H', 'T', 'W' },
    //         new List<char> { 'G', 'R', 'V', 'F', 'Z', 'J', 'B', 'H' },
    //         new List<char> { 'H', 'P', 'T', 'R' },
    //         new List<char> { 'Z', 'S', 'N', 'P', 'H', 'T' },
    //         new List<char> { 'N', 'V', 'F', 'Z', 'H', 'J', 'C', 'D' },
    //         new List<char> { 'P', 'M', 'G', 'F', 'W', 'D', 'Z' },
    //         new List<char> { 'M', 'V', 'Z', 'W', 'S', 'J', 'D', 'P' },
    //         new List<char> { 'N', 'D', 'S' },
    //         new List<char> { 'D', 'Z', 'S', 'F', 'M' }
    //     };

    // private static List<List<char>> partTwoLists = new List<List<char>>
    //     {
    //         new List<char> { 'F', 'C', 'J', 'P', 'H', 'T', 'W' },
    //         new List<char> { 'G', 'R', 'V', 'F', 'Z', 'J', 'B', 'H' },
    //         new List<char> { 'H', 'P', 'T', 'R' },
    //         new List<char> { 'Z', 'S', 'N', 'P', 'H', 'T' },
    //         new List<char> { 'N', 'V', 'F', 'Z', 'H', 'J', 'C', 'D' },
    //         new List<char> { 'P', 'M', 'G', 'F', 'W', 'D', 'Z' },
    //         new List<char> { 'M', 'V', 'Z', 'W', 'S', 'J', 'D', 'P' },
    //         new List<char> { 'N', 'D', 'S' },
    //         new List<char> { 'D', 'Z', 'S', 'F', 'M' }
    //     };

    private static List<List<char>> lists = new List<List<char>>
        {
            new List<char> { 'F', 'C', 'P', 'G', 'Q', 'R' },
            new List<char> { 'W', 'T', 'C', 'P' },
            new List<char> { 'B', 'H', 'P', 'M', 'C' },
            new List<char> { 'L', 'T', 'Q', 'S', 'M', 'P', 'R' },
            new List<char> { 'P', 'H', 'J', 'Z', 'V', 'G', 'N' },
            new List<char> { 'D', 'P', 'J' },
            new List<char> { 'L', 'G', 'P', 'Z', 'F', 'J', 'T', 'R' },
            new List<char> { 'N', 'L', 'H', 'C', 'F', 'P', 'T', 'J' },
            new List<char> { 'G', 'V', 'Z', 'Q', 'H', 'T', 'C', 'W' }
        };

    private static List<List<char>> partTwoLists = new List<List<char>>
        {
            new List<char> { 'F', 'C', 'P', 'G', 'Q', 'R' },
            new List<char> { 'W', 'T', 'C', 'P' },
            new List<char> { 'B', 'H', 'P', 'M', 'C' },
            new List<char> { 'L', 'T', 'Q', 'S', 'M', 'P', 'R' },
            new List<char> { 'P', 'H', 'J', 'Z', 'V', 'G', 'N' },
            new List<char> { 'D', 'P', 'J' },
            new List<char> { 'L', 'G', 'P', 'Z', 'F', 'J', 'T', 'R' },
            new List<char> { 'N', 'L', 'H', 'C', 'F', 'P', 'T', 'J' },
            new List<char> { 'G', 'V', 'Z', 'Q', 'H', 'T', 'C', 'W' }
        };

    // Redo this so you dont have to hardcode the lists
    public override object SolvePartOne(string[] input)
    {
        foreach (string line in input)
        {
            // Added in 2025
            if (!line.Contains("move"))
            {
                continue;
            }

            string str = line;
            int moveNum;
            int fromStack;
            int toStack;

            str = str.Substring(5);
            moveNum = int.Parse(str.Substring(0, str.IndexOf(" ")));
            str = str.Substring(str.IndexOf("from") + 5);
            fromStack = int.Parse(str.Substring(0, str.IndexOf(" ")));
            str = str.Substring(str.IndexOf("to") + 3);
            toStack = int.Parse(str);

            Move(moveNum, fromStack, toStack);
        }

        string text = "";

        foreach (List<char> list in lists)
        {
            text += list[list.Count - 1];
        }

        return text;
    }

    public override object SolvePartTwo(string[] input)
    {
        foreach (string line in input)
        {
            // Added in 2025
            if (!line.Contains("move"))
            {
                continue;
            }

            string str = line;
            int moveNum;
            int fromStack;
            int toStack;

            str = str.Substring(5);
            moveNum = int.Parse(str.Substring(0, str.IndexOf(" ")));
            str = str.Substring(str.IndexOf("from") + 5);
            fromStack = int.Parse(str.Substring(0, str.IndexOf(" ")));
            str = str.Substring(str.IndexOf("to") + 3);
            toStack = int.Parse(str);

            PartTwo(moveNum, fromStack, toStack);
        }

        string text = "";

        foreach (List<char> list in partTwoLists)
        {
            text += list[list.Count - 1];
        }

        return text;
    }

    public static void Move(int moveNum, int fromStack, int toStack)
    {
        for (int i = 0; i < moveNum; i++)
        {
            lists[toStack - 1].Add(lists[fromStack - 1][lists[fromStack - 1].Count - 1]);
            lists[fromStack - 1].RemoveAt(lists[fromStack - 1].Count - 1);
        }
    }

    public static void PartTwo(int moveNum, int fromStack, int toStack)
    {
        List<char> newList = new List<char>();

        int index = moveNum;
        for (int i = 0; i < moveNum; i++)
        {
            newList.Add(partTwoLists[fromStack - 1][partTwoLists[fromStack - 1].Count - index]);
            partTwoLists[fromStack - 1].RemoveAt(partTwoLists[fromStack - 1].Count - index);
            index--;
        }

        int num = newList.Count;
        for (int i = 0; i < num; i++)
        {
            partTwoLists[toStack - 1].Add(newList[0]);
            newList.RemoveAt(0);
        }
    }
}