using System;

namespace AdventOfCode;

public class PuzzleName(string name) : Attribute
{
    public readonly string Name = name;
}