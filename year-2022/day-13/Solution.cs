using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day13;

[PuzzleName("Distress Signal")]
public class Solution : Solver
{
    // This does not work
    public override object SolvePartOne(string[] input)
    {
        List<PacketPair> packetPairs = new List<PacketPair>();

        for (int i = 0; i < input.Length; i += 3)
        {
            PacketPair newPacketPair = new PacketPair(input[i], input[i + 1]);
            packetPairs.Add(newPacketPair);
        }

        int count = 0;

        for (int i = 0; i < packetPairs.Count; i++)
        {
            if (packetPairs[i].CompareValues())
            {
                count += i + 1;
            }
        }

        return count;
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Solved Yet";
    }
}