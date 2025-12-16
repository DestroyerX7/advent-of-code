using System;
using System.Collections.Generic;
using System.Linq;

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

        return deviceDict["you"].GetPathsToOut([]);
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Finished Yet";

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

        return deviceDict["svr"].GetPathsToOutPartTwo([]);
    }
}

public class Device(string name)
{
    public string Name = name;
    public HashSet<Device> Outputs = [];
    private long _pathToOut = -1;

    // private long _pathToOutPartTwo = -1;

    // private List<HashSet<string>>? _yo = null;

    public void AddOutputDevice(Device device)
    {
        Outputs.Add(device);
    }

    public long GetPathsToOut(HashSet<Device> currentPath)
    {
        if (_pathToOut != -1)
        {
            return _pathToOut;
        }

        long num = 0;

        foreach (Device device in Outputs)
        {
            if (device.Name == "out")
            {
                num++;
            }
            else if (!currentPath.Contains(device))
            {
                num += device.GetPathsToOut([.. currentPath, this]);
            }
        }

        _pathToOut = num;
        return _pathToOut;
    }

    public long GetPathsToOutPartTwo(HashSet<string> currentPath)
    {
        // if (_yo != null)
        // {
        //     return _yo;
        // }

        long num = 0;
        // List<HashSet<string>> paths = [];

        foreach (Device device in Outputs)
        {
            HashSet<string> hi = [.. currentPath, Name, device.Name];
            if (device.Name == "out" /*&& hi.Contains("fft") && hi.Contains("dac")*/)
            {
                // hi.ToList().ForEach(x => Console.Write(x + ", "));
                // System.Console.WriteLine();
                num++;
                // paths.Add([.. currentPath, Name]);
            }
            else if (!currentPath.Contains(Name))
            {
                num += device.GetPathsToOutPartTwo([.. currentPath, Name]);
                // var yo = device.GetPathsToOutPartTwo([.. currentPath, Name]);

                // paths = [.. paths.Concat(yo)];
            }
        }

        // _pathToOutPartTwo = num;
        return num;
        // _yo = paths;
        // return paths;
    }

    public override string ToString()
    {
        string output = $"{Name}: ";

        foreach (Device device in Outputs)
        {
            output += device.Name + " ";
        }

        return output;
    }
}