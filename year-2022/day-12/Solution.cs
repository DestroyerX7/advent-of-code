using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day12;

[PuzzleName("Hill Climbing Algorithm")]
public class Solution : Solver
{
    public static Height[,]? HeightMap; // Added ? in 2025 to fix terminal warning
    public static List<Height> Starts = new List<Height>();
    public static Height? End; // Added ? in 2025 to fix terminal warning

    public override object SolvePartOne(string[] input)
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        Dictionary<char, int> heightDictionary = new Dictionary<char, int>()
        {
            { 'S', 0 },
            { 'E', 25 }
        };
        HeightMap = new Height[input.Length, input[0].Length];

        for (int i = 0; i < alphabet.Length; i++)
        {
            heightDictionary.Add(alphabet[i], i);
        }

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                int[] coordinate = new int[] { j, i };
                int elevation = heightDictionary[input[i][j]];
                Height newHeight = new Height(coordinate, elevation);
                HeightMap[i, j] = newHeight;

                if (input[i][j] == 'S')
                {
                    Starts.Add(newHeight);
                }
                else if (input[i][j] == 'E')
                {
                    End = newHeight;
                }
            }
        }

        foreach (Height height in HeightMap)
        {
            height.AddEdges();
        }

        List<int> steps = new List<int>();

        foreach (Height start in Starts)
        {
            int step = FindPath(start);

            if (step != -1)
            {
                steps.Add(step);
            }
        }

        return steps.Min();
    }

    public override object SolvePartTwo(string[] input)
    {
        HeightMap = null;
        Starts.Clear();
        End = null;

        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        Dictionary<char, int> heightDictionary = new Dictionary<char, int>()
        {
            { 'S', 0 },
            { 'E', 25 }
        };
        HeightMap = new Height[input.Length, input[0].Length];

        for (int i = 0; i < alphabet.Length; i++)
        {
            heightDictionary.Add(alphabet[i], i);
        }

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                int[] coordinate = new int[] { j, i };
                int elevation = heightDictionary[input[i][j]];
                Height newHeight = new Height(coordinate, elevation);
                HeightMap[i, j] = newHeight;

                if (input[i][j] == 'S' || input[i][j] == 'a')
                {
                    Starts.Add(newHeight);
                }
                else if (input[i][j] == 'E')
                {
                    End = newHeight;
                }
            }
        }

        foreach (Height height in HeightMap)
        {
            height.AddEdges();
        }

        List<int> steps = new List<int>();

        foreach (Height start in Starts)
        {
            int step = FindPath(start);

            if (step != -1)
            {
                steps.Add(step);
            }
        }

        return steps.Min();
    }

    private static int FindPath(Height Start)
    {
        // Added in 2025 to fix terminal warning
        if (End == null)
        {
            return -1;
        }

        List<Height> open = new List<Height>();
        HashSet<Height> closed = new HashSet<Height>();
        open.Add(Start);

        while (open.Count > 0)
        {
            Height currentHeight = open[0];

            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].Cost < currentHeight.Cost || open[i].Cost == currentHeight.Cost && open[i].EndDistance < currentHeight.EndDistance)
                {
                    currentHeight = open[i];
                }
            }

            open.Remove(currentHeight);
            closed.Add(currentHeight);

            if (currentHeight == End)
            {
                return RetracePath(Start);
            }

            foreach (Height neighbor in currentHeight.Neighbors)
            {
                if (closed.Contains(neighbor))
                {
                    continue;
                }

                double newMovementCostToNeighbor = currentHeight.StartDistance + GetDistance(currentHeight, neighbor);

                if (newMovementCostToNeighbor < neighbor.StartDistance || !open.Contains(neighbor))
                {
                    neighbor.StartDistance = newMovementCostToNeighbor;
                    neighbor.EndDistance = GetDistance(neighbor, End);
                    neighbor.Parent = currentHeight;

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }

        return -1;
    }

    private static int RetracePath(Height Start)
    {
        List<Height> path = new List<Height>();
        Height? currentHeight = End;

        // Added && currentHeight != null in 2025 to fix terminal warning
        while (currentHeight != Start && currentHeight != null)
        {
            path.Add(currentHeight);
            currentHeight = currentHeight.Parent;
        }

        return path.Count;
    }

    private static double GetDistance(Height a, Height b)
    {
        double distX = Math.Abs(a.Coordinate[0] - b.Coordinate[0]);
        double distY = Math.Abs(a.Coordinate[1] - b.Coordinate[1]);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }
}