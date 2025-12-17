using System;
using System.Reflection;

namespace AdventOfCode.Lib;

public abstract class Solver
{
    public abstract object SolvePartOne(string[] input);
    public abstract object SolvePartTwo(string[] input);

    public string GetYear()
    {
        return GetType()?.FullName?.Split(".")[1][4..] ?? "NO YEAR";
    }

    public string GetDay()
    {
        return GetType()?.FullName?.Split(".")[2][3..] ?? "NO DAY";
    }

    public string GetName()
    {
        Attribute? attribute = GetType()?.GetCustomAttribute(typeof(PuzzleName));

        if (attribute == null)
        {
            return "NO NAME";
        }

        return ((PuzzleName)attribute).Name;
    }
}