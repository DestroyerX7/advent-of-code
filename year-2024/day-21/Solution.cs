using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day21;

[PuzzleName("Keypad Conundrum")]
public class Solution : Solver
{
    private readonly static Dictionary<char, Vector2> _keypadDict = new()
    {
        { '7', Vector2.Zero },
        { '8', Vector2.Right },
        { '9', new(2, 0) },
        { '4', Vector2.Down },
        { '5', new(1, -1) },
        { '6', new(2, -1) },
        { '1', new(0, -2) },
        { '2', new(1, -2) },
        { '3', new(2, -2) },
        { '0', new(1, -3) },
        { 'A', new(2, -3) },
    };

    private readonly static Dictionary<char, Vector2> _directionalKeypadDict = new()
    {
        { '^', Vector2.Right },
        { 'A', new(2, 0) },
        { '<', Vector2.Down },
        { 'v', new(1, -1) },
        { '>', new(2, -1) },
    };

    private readonly static Dictionary<string, List<string>> _helloOne = new();

    private readonly static Dictionary<string, List<string>> _helloTwo = new();

    public override object SolvePartOne(string[] input)
    {
        long totalComplexities = 0;
        // long totalComplexitiesPartTwo = 0;

        foreach (string code in input)
        {
            List<string> one = YoNumPad(code, _keypadDict, _helloOne);
            List<string> two = What(2, new(one));
            // List<string> three = What(3, new(one));

            int num = int.Parse(code[..3]);
            totalComplexities += two.Min(s => s.Length) * num;
            // totalComplexitiesPartTwo += three.Min(s => s.Length) * num;
        }

        return totalComplexities;
    }

    public override object SolvePartTwo(string[] input)
    {
        // Added in 2025
        _helloOne.Clear();
        _helloTwo.Clear();

        // long totalComplexities = 0;
        long totalComplexitiesPartTwo = 0;

        foreach (string code in input)
        {
            List<string> one = YoNumPad(code, _keypadDict, _helloOne);
            List<string> two = What(2, new(one));
            List<string> three = What(3, new(one));

            int num = int.Parse(code[..3]);
            // totalComplexities += two.Min(s => s.Length) * num;
            totalComplexitiesPartTwo += three.Min(s => s.Length) * num;
        }

        return totalComplexitiesPartTwo;
    }

    private static List<string> What(int numRobots, List<string> start)
    {
        List<string> current = start;
        List<string> newList = new();

        for (int i = 0; i < numRobots; i++)
        {
            foreach (var item in current)
            {
                newList = newList.Concat(YoNumPad(item, _directionalKeypadDict, _helloTwo)).ToList();
            }

            int min = newList.Min(s => s.Length);

            current = new(newList.Where(s => s.Length == min));
            newList.Clear();
        }

        return current;
    }

    private static List<string> YoNumPad(string code, Dictionary<char, Vector2> dict, Dictionary<string, List<string>> other)
    {
        Vector2 currentPos = dict['A'];
        List<string> options = new();

        foreach (char character in code)
        {
            var yo = GetOptions(currentPos, dict[character], dict, other);

            if (options.Count == 0)
            {
                foreach (string item in yo)
                {
                    options.Add(item);
                }
            }
            else
            {
                List<string> newOptions = new();

                foreach (string v in options)
                {
                    foreach (string item in yo)
                    {
                        newOptions.Add(v + item);
                    }
                }

                options = newOptions;
            }

            currentPos = dict[character];
        }

        return options;
    }

    private static List<string> GetOptions(Vector2 current, Vector2 end, Dictionary<char, Vector2> dict, Dictionary<string, List<string>> other)
    {
        if (other.ContainsKey(current.ToString() + " => " + end.ToString()))
        {
            return other[current.ToString() + " => " + end.ToString()];
        }

        if (current == end)
        {
            return new()
            {
                "A",
            };
        }

        List<string> yo = new();

        if (end.X > current.X && dict.ContainsValue(current + Vector2.Right))
        {
            var hi = GetOptions(current + Vector2.Right, end, dict, other);

            if (hi.Count > 0)
            {
                foreach (string item in hi)
                {
                    yo.Add(">" + item);
                }
            }
            else
            {
                yo.Add(">");
            }
        }

        if (current.X > end.X && dict.ContainsValue(current + Vector2.Left))
        {
            var hi = GetOptions(current + Vector2.Left, end, dict, other);

            if (hi.Count > 0)
            {
                foreach (string item in hi)
                {
                    yo.Add("<" + item);
                }
            }
            else
            {
                yo.Add("<");
            }
        }

        if (end.Y > current.Y && dict.ContainsValue(current + Vector2.Up))
        {
            var hi = GetOptions(current + Vector2.Up, end, dict, other);

            if (hi.Count > 0)
            {
                foreach (string item in hi)
                {
                    yo.Add("^" + item);
                }
            }
            else
            {
                yo.Add("^");
            }
        }

        if (current.Y > end.Y && dict.ContainsValue(current + Vector2.Down))
        {
            var hi = GetOptions(current + Vector2.Down, end, dict, other);

            if (hi.Count > 0)
            {
                foreach (string item in hi)
                {
                    yo.Add("v" + item);
                }
            }
            else
            {
                yo.Add("v");
            }
        }

        other[current.ToString() + " => " + end.ToString()] = yo;

        return yo;
    }
}

public struct Vector2 : IEquatable<Vector2>
{
    public int X;
    public int Y;

    public static readonly Vector2 Right = new(1, 0);
    public static readonly Vector2 Left = new(-1, 0);
    public static readonly Vector2 Up = new(0, 1);
    public static readonly Vector2 Down = new(0, -1);
    public static readonly Vector2 Zero = new(0, 0);
    public static readonly Vector2 One = new(1, 1);

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector2 operator *(Vector2 vector, int num)
    {
        return new(vector.X * num, vector.Y * num);
    }

    public static Vector2 operator *(int num, Vector2 vector)
    {
        return vector * num;
    }

    public Vector2 Rotate90DegreesClockwise()
    {
        int oldX = X;
        return new(Y, -oldX);
    }

    public Vector2 Rotate90DegreesCounterclockwise()
    {
        int oldX = X;
        return new(-Y, oldX);
    }

    public static bool operator ==(Vector2 v1, Vector2 v2)
    {
        return v1.X == v2.X && v1.Y == v2.Y;
    }

    public static bool operator !=(Vector2 v1, Vector2 v2)
    {
        return v1.X != v2.X || v1.Y != v2.Y;
    }

    public static double Distance(Vector2 v1, Vector2 v2)
    {
        Vector2 delta = v2 - v1;
        return Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public bool Equals(Vector2 other)
    {
        return this == other;
    }

    // Added in 2025 to stop termianl warning
    public readonly override bool Equals(object? obj)
    {
        return obj is Vector2 vector && vector.Equals(this);
    }

    // Added in 2025 to stop termianl warning
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
