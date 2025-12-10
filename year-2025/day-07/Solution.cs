using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day07;

[PuzzleName("Laboratories")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int numSplits = 0;

        Vector2 startPos = new(input[0].IndexOf('S'), 0);

        HashSet<Vector2> splitPositions = [startPos];
        Queue<Vector2> beamPositions = new();
        beamPositions.Enqueue(startPos);

        while (beamPositions.Count > 0)
        {
            Vector2 beamPos = beamPositions.Dequeue();

            while (beamPos.Y < input.Length && input[beamPos.Y][beamPos.X] != '^')
            {
                beamPos += Vector2.Up;
            }

            if (beamPos.Y >= input.Length || splitPositions.Contains(beamPos))
            {
                continue;
            }

            splitPositions.Add(beamPos);

            numSplits++;

            Vector2 left = new(beamPos.X - 1, beamPos.Y);
            Vector2 right = new(beamPos.X + 1, beamPos.Y);

            beamPositions.Enqueue(left);
            beamPositions.Enqueue(right);
        }

        return numSplits;
    }

    public override object SolvePartTwo(string[] input)
    {
        Dictionary<Vector2, long> dict = [];

        for (int y = input.Length - 1; y >= 0; y--)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                if (input[y][x] == '^')
                {
                    Vector2 pos = new(x, y);
                    long hi = GetNumTimelines(pos, input, dict);
                    dict[pos] = hi;
                }
            }
        }

        Vector2 startPos = new(input[0].IndexOf('S'), 0);
        return GetNumTimelines(startPos, input, dict);
    }

    private static long GetNumTimelines(Vector2 pos, string[] input, Dictionary<Vector2, long> dict)
    {
        long numTimelines = 0;

        Queue<Vector2> beamPositions = new();
        beamPositions.Enqueue(pos);

        while (beamPositions.Count > 0)
        {
            Vector2 beamPos = beamPositions.Dequeue();

            while (beamPos.Y < input.Length && input[beamPos.Y][beamPos.X] != '^')
            {
                beamPos += Vector2.Up;
            }

            if (beamPos.Y >= input.Length)
            {
                numTimelines++;
                continue;
            }

            if (dict.ContainsKey(beamPos))
            {
                numTimelines += dict[beamPos];
                continue;
            }

            Vector2 left = new(beamPos.X - 1, beamPos.Y);
            Vector2 right = new(beamPos.X + 1, beamPos.Y);

            beamPositions.Enqueue(left);
            beamPositions.Enqueue(right);
        }

        return numTimelines;
    }
}