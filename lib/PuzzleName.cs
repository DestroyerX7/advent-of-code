using System;

namespace AdventOfCode.Lib;

public class PuzzleName(string name) : Attribute
{
    public readonly string Name = name;
}