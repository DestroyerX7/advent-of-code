using System.Collections.Generic;

namespace AdventOfCode.Year2025.Day11;

public class Device(string name)
{
    private readonly string _name = name;
    private readonly HashSet<Device> _outputs = [];
    private long _paths = -1;

    public void AddOutputDevice(Device device)
    {
        _outputs.Add(device);
    }

    public long GetPaths(string destinationName, HashSet<string> currentPath)
    {
        if (_paths != -1)
        {
            return _paths;
        }

        long num = 0;

        foreach (Device device in _outputs)
        {
            if (device._name == destinationName)
            {
                num++;
            }
            else if (!currentPath.Contains(device._name))
            {
                num += device.GetPaths(destinationName, [.. currentPath, _name]);
            }
        }

        _paths = num;
        return num;
    }

    public void ResetPathsVar()
    {
        _paths = -1;
    }

    public override string ToString()
    {
        string output = $"{_name}: ";

        foreach (Device device in _outputs)
        {
            output += device._name + " ";
        }

        return output;
    }
}