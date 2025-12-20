using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day07;

[PuzzleName("No Space Left On Device")]
public class Solution : Solver
{
    public static List<int> DirectoryFiles = new List<int>();

    // Added ?s to fix terminal warnings in 2025
    public override object SolvePartOne(string[] input)
    {
        Directory mainDirectory = new Directory();
        Directory? currentDirectory = mainDirectory;

        foreach (string line in input)
        {
            // Added in 2025
            if (line == "$ cd /")
            {
                continue;
            }

            int lineNum = 0;
            if (line.Contains("$ cd .."))
            {
                currentDirectory = currentDirectory?.Parent;
            }
            else if (line.Contains("$ cd"))
            {
                currentDirectory?.CurrentChild += 1;
                currentDirectory = currentDirectory?.Children[currentDirectory.CurrentChild - 1];
            }
            else if (line.Contains("dir"))
            {
                currentDirectory?.AddDirectory();
            }
            else if (int.TryParse(line.Substring(0, line.IndexOf(" ")), out lineNum))
            {
                currentDirectory?.FileSize += lineNum;
            }
        }

        mainDirectory.GetFileSize();

        int sum = 0;
        foreach (var item in DirectoryFiles)
        {
            if (item <= 100000)
            {
                sum += item;
            }
        }

        return sum;
    }

    // Added ?s to fix terminal warnings in 2025
    public override object SolvePartTwo(string[] input)
    {
        DirectoryFiles.Clear(); // Added ion 2025
        Directory mainDirectory = new Directory();
        Directory? currentDirectory = mainDirectory;

        foreach (string line in input)
        {
            // Added in 2025
            if (line == "$ cd /")
            {
                continue;
            }

            int lineNum = 0;
            if (line.Contains("$ cd .."))
            {
                currentDirectory = currentDirectory?.Parent;
            }
            else if (line.Contains("$ cd"))
            {
                currentDirectory?.CurrentChild += 1;
                currentDirectory = currentDirectory?.Children[currentDirectory.CurrentChild - 1];
            }
            else if (line.Contains("dir"))
            {
                currentDirectory?.AddDirectory();
            }
            else if (int.TryParse(line.Substring(0, line.IndexOf(" ")), out lineNum))
            {
                currentDirectory?.FileSize += lineNum;
            }
        }

        int total = mainDirectory.GetFileSize();

        List<int> works = new List<int>();
        foreach (var item in DirectoryFiles)
        {
            if (total - item <= 40000000)
            {
                works.Add(item);
            }
        }

        return works.Min();
    }
}