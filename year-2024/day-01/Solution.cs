using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day01;

[PuzzleName("Historian Hysteria")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int[] listOne = new int[input.Length];
        int[] listTwo = new int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            string[] separated = input[i].Split("   ");
            listOne[i] = int.Parse(separated[0]);
            listTwo[i] = int.Parse(separated[1]);
        }

        listOne = listOne.OrderBy(n => n).ToArray();
        listTwo = listTwo.OrderBy(n => n).ToArray();

        int totalDistance = 0;

        for (int i = 0; i < input.Length; i++)
        {
            int distance = Math.Abs(listOne[i] - listTwo[i]);
            totalDistance += distance;
        }

        return totalDistance;
    }

    public override object SolvePartTwo(string[] input)
    {
        int[] listOne = new int[input.Length];
        int[] listTwo = new int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            string[] separated = input[i].Split("   ");
            listOne[i] = int.Parse(separated[0]);
            listTwo[i] = int.Parse(separated[1]);
        }

        Dictionary<int, int> numAppearances = new();

        foreach (int num in listTwo)
        {
            if (numAppearances.ContainsKey(num))
            {
                numAppearances[num]++;
            }
            else
            {
                numAppearances[num] = 1;
            }
        }

        int similarityScore = 0;

        foreach (int num in listOne)
        {
            if (numAppearances.ContainsKey(num))
            {
                similarityScore += numAppearances[num] * num;
            }
        }

        return similarityScore;
    }
}