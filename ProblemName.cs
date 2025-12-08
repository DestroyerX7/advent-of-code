using System;

namespace AdventOfCode;

public class ProblemName(string name) : Attribute
{
    public readonly string Name = name;
}