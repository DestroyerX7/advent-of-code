using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day09;

[PuzzleName("Movie Theater")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Vector2[] tilePositions = [.. input.Select(l =>
        {
            string[] split = l.Split(',');
            return new Vector2(int.Parse(split[0]), int.Parse(split[1]));
        })];

        long maxArea = 0;

        for (int i = 0; i < tilePositions.Length; i++)
        {
            for (int j = i + 1; j < tilePositions.Length; j++)
            {
                long width = Math.Abs(tilePositions[i].X - tilePositions[j].X) + 1;
                long height = Math.Abs(tilePositions[i].Y - tilePositions[j].Y) + 1;

                if (width * height > maxArea)
                {
                    maxArea = width * height;
                }
            }
        }

        return maxArea;
    }

    public override object SolvePartTwo(string[] input)
    {
        List<Vector2> redTilePositions = [];

        foreach (string line in input)
        {
            int[] split = [.. line.Split(',').Select(int.Parse)];
            Vector2 redTilePos = new(split[0], split[1]);
            redTilePositions.Add(redTilePos);
        }

        Dictionary<Vector2, CornerType> dict = [];

        for (int i = 0; i < redTilePositions.Count; i++)
        {
            Vector2 current = redTilePositions[i];
            Vector2 prev = redTilePositions[(i - 1 + redTilePositions.Count) % redTilePositions.Count];
            Vector2 next = redTilePositions[(i + 1) % redTilePositions.Count];

            if (next.Y == current.Y && next.X > current.X && prev.Y < current.Y)
            {
                // BL
                dict[current] = CornerType.BottomLeft;
            }
            else if (next.Y == current.Y && next.X > current.X && prev.Y > current.Y)
            {
                // TL
                dict[current] = CornerType.TopLeft;
            }

            if (prev.Y == current.Y && prev.X > current.X && next.Y < current.Y)
            {
                // BL
                dict[current] = CornerType.BottomLeft;
            }
            else if (prev.Y == current.Y && prev.X > current.X && next.Y > current.Y)
            {
                // TL
                dict[current] = CornerType.TopLeft;
            }

            if (next.Y == current.Y && next.X < current.X && prev.Y < current.Y)
            {
                // BR
                dict[current] = CornerType.BottomRight;
            }
            else if (next.Y == current.Y && next.X < current.X && prev.Y > current.Y)
            {
                // TR
                dict[current] = CornerType.TopRight;
            }

            if (prev.Y == current.Y && prev.X < current.X && next.Y < current.Y)
            {
                // BR
                dict[current] = CornerType.BottomRight;
            }
            else if (prev.Y == current.Y && prev.X < current.X && next.Y > current.Y)
            {
                // TR
                dict[current] = CornerType.TopRight;
            }
        }

        Dictionary<int, List<Wall>> verticalWalls = [];
        Dictionary<int, List<Wall>> horizontalWalls = [];

        HashSet<int> xValues = [];
        HashSet<int> yValues = [];

        for (int i = 0; i < redTilePositions.Count + 1; i++)
        {
            Vector2 posOne = redTilePositions[i % redTilePositions.Count];
            Vector2 posTwo = redTilePositions[(i - 1 + redTilePositions.Count) % redTilePositions.Count];
            Wall wall = new(posOne, posTwo);

            if (wall.IsVertical)
            {
                if (verticalWalls.ContainsKey(wall.RedTileOne.X))
                {
                    verticalWalls[wall.RedTileOne.X].Add(wall);
                }
                else
                {
                    verticalWalls[wall.RedTileOne.X] = [wall];
                }
            }
            else
            {
                if (horizontalWalls.ContainsKey(wall.RedTileOne.Y))
                {
                    horizontalWalls[wall.RedTileOne.Y].Add(wall);
                }
                else
                {
                    horizontalWalls[wall.RedTileOne.Y] = [wall];
                }
            }

            xValues.Add(posOne.X);
            xValues.Add(posTwo.X);
            yValues.Add(posOne.Y);
            yValues.Add(posTwo.Y);
        }

        List<int> yo = [.. xValues.OrderBy(x => x)];
        List<int> hi = [.. yValues.OrderBy(y => y)];

        List<Wall> segments = [];

        foreach (int y in hi)
        {
            Stack<Vector2> stack = [];
            Vector2 lastInPos = default;
            bool foundIn = false;

            foreach (int x in yo)
            {
                Vector2 pos = new(x, y);

                if (stack.Count > 0)
                {
                    Wall wallOne = new(stack.Peek(), pos);
                    segments.Add(wallOne);
                    // System.Console.WriteLine(wallOne);

                    if (foundIn)
                    {
                        Wall wallTwo = new(lastInPos, pos);
                        segments.Add(wallTwo);
                        // System.Console.WriteLine(wallTwo);
                    }

                    lastInPos = pos;
                    foundIn = true;
                }

                if (stack.Count == 0 && dict.ContainsKey(pos) && (dict[pos] == CornerType.TopLeft || dict[pos] == CornerType.BottomLeft))
                {
                    // if not in shape and hit an enter corner mark enter as true
                    // System.Console.Write(pos);
                    // System.Console.WriteLine(" Entered");
                    stack.Push(pos);
                    foundIn = false;
                }
                else if (stack.Count > 0 && dict.ContainsKey(pos))
                {
                    // if in shape and hit an exit corner mark enter as false
                    var popped = stack.Peek();

                    if ((!dict.ContainsKey(popped) && (dict[pos] == CornerType.TopRight || dict[pos] == CornerType.BottomRight)) || (dict.ContainsKey(popped) && dict[popped] == CornerType.TopLeft && dict[pos] == CornerType.TopRight) || (dict.ContainsKey(popped) && dict[popped] == CornerType.BottomLeft && dict[pos] == CornerType.BottomRight))
                    {
                        // System.Console.Write(pos);
                        // System.Console.WriteLine(" Exited");
                        stack.Pop();
                    }
                }
                else if (verticalWalls[x].Any(w => w.Intersects(pos)))
                {
                    // Check is pos intersects a wall

                    if (stack.Count > 0)
                    {
                        stack.Pop();
                        // System.Console.Write(pos);
                        // System.Console.WriteLine(" Exited");
                    }
                    else
                    {
                        stack.Push(pos);
                        // System.Console.Write(pos);
                        // System.Console.WriteLine(" Entered");
                    }
                }
            }
        }

        foreach (int x in yo)
        {
            Stack<Vector2> stack = [];
            Vector2 lastInPos = default;
            bool foundIn = false;

            foreach (int y in hi)
            {
                Vector2 pos = new(x, y);

                if (stack.Count > 0)
                {
                    Wall wallOne = new(stack.Peek(), pos);
                    segments.Add(wallOne);
                    // System.Console.WriteLine(wallOne);

                    if (foundIn)
                    {
                        Wall wallTwo = new(lastInPos, pos);
                        segments.Add(wallTwo);
                        // System.Console.WriteLine(wallTwo);
                    }

                    lastInPos = pos;
                    foundIn = true;
                }

                if (stack.Count == 0 && dict.ContainsKey(pos) && (dict[pos] == CornerType.TopLeft || dict[pos] == CornerType.BottomLeft))
                {
                    // if not in shape and hit an enter corner mark enter as true
                    // System.Console.Write(pos);
                    // System.Console.WriteLine(" Entered");
                    stack.Push(pos);
                    foundIn = false;
                }
                else if (stack.Count > 0 && dict.ContainsKey(pos))
                {
                    // if in shape and hit an exit corner mark enter as false
                    var popped = stack.Peek();

                    if ((!dict.ContainsKey(popped) && (dict[pos] == CornerType.BottomLeft || dict[pos] == CornerType.BottomRight)) || (dict.ContainsKey(popped) && dict[popped] == CornerType.TopLeft && dict[pos] == CornerType.BottomLeft) || (dict.ContainsKey(popped) && dict[popped] == CornerType.TopRight && dict[pos] == CornerType.BottomRight))
                    {
                        // System.Console.Write(pos);
                        // System.Console.WriteLine(" Exited");
                        stack.Pop();
                    }
                }
                else if (horizontalWalls[y].Any(w => w.Intersects(pos)))
                {
                    // Check is pos intersects a wall

                    if (stack.Count > 0)
                    {
                        stack.Pop();
                        // System.Console.Write(pos);
                        // System.Console.WriteLine(" Exited");
                    }
                    else
                    {
                        stack.Push(pos);
                        // System.Console.Write(pos);
                        // System.Console.WriteLine(" Entered");
                    }
                }
            }
        }

        PriorityQueue<Vector2[], long> priorityQueue = new();

        for (int i = 0; i < redTilePositions.Count; i++)
        {
            for (int j = i + 1; j < redTilePositions.Count; j++)
            {
                long width = Math.Abs(redTilePositions[i].X - redTilePositions[j].X) + 1;
                long height = Math.Abs(redTilePositions[i].Y - redTilePositions[j].Y) + 1;
                priorityQueue.Enqueue([redTilePositions[i], redTilePositions[j]], -width * height);
            }
        }

        while (priorityQueue.Count > 0)
        {
            Vector2[] pair = priorityQueue.Dequeue();

            Vector2 what = pair[0];
            Vector2 wtf = pair[1];

            bool one = segments.Any(s => s.IsPos(what) && s.IsPos(new(wtf.X, what.Y)));
            bool two = segments.Any(s => s.IsPos(what) && s.IsPos(new(what.X, wtf.Y)));
            bool three = segments.Any(s => s.IsPos(new(what.X, wtf.Y)) && s.IsPos(wtf));
            bool four = segments.Any(s => s.IsPos(new(wtf.X, what.Y)) && s.IsPos(wtf));

            if (one && two && three && four)
            {
                long width = Math.Abs(what.X - wtf.X) + 1;
                long height = Math.Abs(what.Y - wtf.Y) + 1;

                System.Console.WriteLine(what);
                System.Console.WriteLine(wtf);
                System.Console.WriteLine();

                return width * height;
            }
        }

        return -1;
    }
}

public class Wall(Vector2 redTileone, Vector2 redTileTwo)
{
    public Vector2 RedTileOne = redTileone;
    public Vector2 RedTileTwo = redTileTwo;

    public bool IsVertical => RedTileOne.X == RedTileTwo.X;
    public bool IsHorizontal => RedTileOne.Y == RedTileTwo.Y;

    public bool Intersects(Vector2 pos)
    {
        if (pos.X == RedTileOne.X)
        {
            if ((pos.Y >= RedTileOne.Y && pos.Y <= RedTileTwo.Y) || (pos.Y >= RedTileTwo.Y && pos.Y <= RedTileOne.Y))
            {
                return true;
            }
        }

        if (pos.Y == RedTileOne.Y)
        {
            if ((pos.X >= RedTileOne.X && pos.X <= RedTileTwo.X) || (pos.X >= RedTileTwo.X && pos.X <= RedTileOne.X))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsPos(Vector2 pos)
    {
        return RedTileOne == pos || RedTileTwo == pos;
    }

    public override string ToString()
    {
        return RedTileOne.ToString() + "-----" + RedTileTwo.ToString();
    }
}

public enum CornerType
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}