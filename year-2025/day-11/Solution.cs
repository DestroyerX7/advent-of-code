using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day11;

[PuzzleName("Reactor")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Dictionary<string, Device> deviceDict = new()
        {
            { "out", new("out") },
        };

        foreach (string line in input)
        {
            int colonIndex = line.IndexOf(':');

            string deviceName = line[..colonIndex];
            Device device = new(deviceName);
            deviceDict.Add(deviceName, device);
        }

        foreach (string line in input)
        {
            int colonIndex = line.IndexOf(':');
            string deviceName = line[..colonIndex];

            string[] outputNames = line[(colonIndex + 2)..].Split(' ');

            foreach (string outputName in outputNames)
            {
                deviceDict[deviceName].AddOutputDevice(deviceDict[outputName]);
            }
        }

        return deviceDict["you"].GetPaths("out", []);
    }

    // This reddit post helped a lot
    // https://www.reddit.com/r/adventofcode/comments/1poub2o/2025_day_11_part_2_stuck_on_part_2/
    // Idk y I didn't think to break the paths up into sections
    public override object SolvePartTwo(string[] input)
    {
        Dictionary<string, Device> deviceDict = new()
        {
            { "out", new("out") },
        };

        foreach (string line in input)
        {
            int colonIndex = line.IndexOf(':');

            string deviceName = line[..colonIndex];
            Device device = new(deviceName);
            deviceDict.Add(deviceName, device);
        }

        foreach (string line in input)
        {
            int colonIndex = line.IndexOf(':');
            string deviceName = line[..colonIndex];

            string[] outputNames = line[(colonIndex + 2)..].Split(' ');

            foreach (string outputName in outputNames)
            {
                deviceDict[deviceName].AddOutputDevice(deviceDict[outputName]);
            }
        }

        // dac => fft == 0, so there is no svr => dac => fft
        long partOne = deviceDict["svr"].GetPaths("fft", []); // Correct
        ResetAllDevices();

        long partTwo = deviceDict["fft"].GetPaths("dac", []); // Correct
        ResetAllDevices();

        // dac => fft == 0, so there is no dac => fft => out
        long partThree = deviceDict["dac"].GetPaths("out", []); // Correct

        long totalPaths = partOne * partTwo * partThree;
        return totalPaths;

        void ResetAllDevices()
        {
            foreach (KeyValuePair<string, Device> item in deviceDict)
            {
                item.Value.ResetPathsVar();
            }
        }
    }
}
