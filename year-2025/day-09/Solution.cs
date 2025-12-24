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

        List<int> sortedXValues = [.. xValues.OrderBy(x => x)];
        List<int> sortedYValues = [.. yValues.OrderBy(y => y)];

        List<Wall> segments = [];

        foreach (int y in sortedYValues)
        {
            Stack<Vector2> stack = [];
            Vector2 enteredPos = -Vector2.One;

            foreach (int x in sortedXValues)
            {
                Vector2 pos = new(x, y);

                if (stack.Count == 0)
                {
                    if (dict.ContainsKey(pos) && (dict[pos] == CornerType.TopLeft || dict[pos] == CornerType.BottomLeft))
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                    else if (!dict.ContainsKey(pos) && verticalWalls[x].Any(w => w.Intersects(pos)))
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                }
                else
                {
                    if (!dict.ContainsKey(pos) && verticalWalls[x].Any(w => w.Intersects(pos)))
                    {
                        stack.Clear();
                    }
                    else if (dict.ContainsKey(pos))
                    {
                        Vector2 peeked = stack.Peek();

                        if (!dict.ContainsKey(peeked))
                        {
                            stack.Push(pos);
                            continue;
                        }

                        if (CancelsMovingX(dict[peeked], dict[pos]))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            stack.Push(pos);
                        }

                        if (!dict.ContainsKey(enteredPos))
                        {
                            bool cornersFormVerticalWall = CornersFormVerticalWall(dict[peeked], dict[pos]);

                            if (cornersFormVerticalWall)
                            {
                                stack.Clear();
                            }
                        }
                    }
                }

                if (stack.Count == 0 && enteredPos != -Vector2.One)
                {
                    Wall wall = new(enteredPos, pos);
                    segments.Add(wall);
                    enteredPos = -Vector2.One;
                }
            }

            if (stack.Count > 0)
            {
                throw new System.Exception($"Never exited row {y}");
            }
        }

        foreach (int x in sortedXValues)
        {
            Stack<Vector2> stack = [];
            Vector2 enteredPos = -Vector2.One;

            foreach (int y in sortedYValues)
            {
                Vector2 pos = new(x, y);

                if (stack.Count == 0)
                {
                    if (dict.ContainsKey(pos) && (dict[pos] == CornerType.TopLeft || dict[pos] == CornerType.TopRight))
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                    else if (!dict.ContainsKey(pos) && horizontalWalls[y].Any(w => w.Intersects(pos)))
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                }
                else
                {
                    if (!dict.ContainsKey(pos) && horizontalWalls[y].Any(w => w.Intersects(pos)))
                    {
                        stack.Clear();
                    }
                    else if (dict.ContainsKey(pos))
                    {
                        Vector2 peeked = stack.Peek();

                        if (!dict.ContainsKey(peeked))
                        {
                            stack.Push(pos);
                            continue;
                        }

                        if (CancelsMovingY(dict[peeked], dict[pos]))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            stack.Push(pos);
                        }

                        if (!dict.ContainsKey(enteredPos))
                        {
                            bool cornersFormHorizontalWall = CornersFormHorizontalWall(dict[peeked], dict[pos]);

                            if (cornersFormHorizontalWall)
                            {
                                stack.Clear();
                            }
                        }
                    }
                }

                if (stack.Count == 0 && enteredPos != -Vector2.One)
                {
                    Wall wall = new(enteredPos, pos);
                    segments.Add(wall);
                    enteredPos = -Vector2.One;
                }
            }

            if (stack.Count > 0)
            {
                throw new System.Exception($"Never exited col {x}");
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

            bool one = segments.Any(w => w.Intersects(what) && w.Intersects(new(wtf.X, what.Y)));
            bool two = segments.Any(w => w.Intersects(what) && w.Intersects(new(what.X, wtf.Y)));
            bool three = segments.Any(w => w.Intersects(wtf) && w.Intersects(new(what.X, wtf.Y)));
            bool four = segments.Any(w => w.Intersects(wtf) && w.Intersects(new(wtf.X, what.Y)));

            if (one && two && three && four)
            {
                long width = Math.Abs(what.X - wtf.X) + 1;
                long height = Math.Abs(what.Y - wtf.Y) + 1;

                // System.Console.WriteLine(what);
                // System.Console.WriteLine(wtf);

                return width * height;
            }
        }

        return -1;
    }

    private void PrintCornerType(CornerType cornerType)
    {
        if (cornerType == CornerType.TopLeft)
        {
            System.Console.Write("┏");
        }
        else if (cornerType == CornerType.TopRight)
        {
            System.Console.Write("┑");
        }
        else if (cornerType == CornerType.BottomRight)
        {
            System.Console.Write("┙");
        }
        else
        {
            System.Console.Write("┗");
        }
    }

    private static bool CancelsMovingX(CornerType cornerTypeOne, CornerType cornerTypeTwo)
    {
        if (cornerTypeOne == CornerType.TopLeft && cornerTypeTwo == CornerType.TopRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.TopLeft && cornerTypeOne == CornerType.TopRight)
        {
            return true;
        }
        else if (cornerTypeOne == CornerType.BottomLeft && cornerTypeTwo == CornerType.BottomRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.BottomLeft && cornerTypeOne == CornerType.BottomRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool CancelsMovingY(CornerType cornerTypeOne, CornerType cornerTypeTwo)
    {
        if (cornerTypeOne == CornerType.TopLeft && cornerTypeTwo == CornerType.BottomLeft)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.TopLeft && cornerTypeOne == CornerType.BottomLeft)
        {
            return true;
        }
        else if (cornerTypeOne == CornerType.TopRight && cornerTypeTwo == CornerType.BottomRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.TopRight && cornerTypeOne == CornerType.BottomRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool CornersFormVerticalWall(CornerType cornerTypeOne, CornerType cornerTypeTwo)
    {
        if (cornerTypeOne == CornerType.BottomLeft && cornerTypeTwo == CornerType.TopRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.BottomLeft && cornerTypeOne == CornerType.TopRight)
        {
            return true;
        }
        else if (cornerTypeOne == CornerType.TopLeft && cornerTypeTwo == CornerType.BottomRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.TopLeft && cornerTypeOne == CornerType.BottomRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool CornersFormHorizontalWall(CornerType cornerTypeOne, CornerType cornerTypeTwo)
    {
        if (cornerTypeOne == CornerType.BottomLeft && cornerTypeTwo == CornerType.TopRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.BottomLeft && cornerTypeOne == CornerType.TopRight)
        {
            return true;
        }
        else if (cornerTypeOne == CornerType.TopLeft && cornerTypeTwo == CornerType.BottomRight)
        {
            return true;
        }
        else if (cornerTypeTwo == CornerType.TopLeft && cornerTypeOne == CornerType.BottomRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public struct Wall(Vector2 redTileone, Vector2 redTileTwo)
{
    public Vector2 RedTileOne = redTileone;
    public Vector2 RedTileTwo = redTileTwo;

    public readonly bool IsVertical => RedTileOne.X == RedTileTwo.X;
    public readonly bool IsHorizontal => RedTileOne.Y == RedTileTwo.Y;

    public readonly bool Intersects(Vector2 pos)
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

    public readonly bool IsPos(Vector2 pos)
    {
        return RedTileOne == pos || RedTileTwo == pos;
    }

    public readonly override string ToString()
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