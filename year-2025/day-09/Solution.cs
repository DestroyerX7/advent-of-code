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

        Dictionary<Vector2, CornerType> posToCornerType = [];

        // Determines what each red tiles corner type is and adds it to the dictionary
        for (int i = 0; i < redTilePositions.Count; i++)
        {
            Vector2 current = redTilePositions[i];
            Vector2 prev = redTilePositions[(i - 1 + redTilePositions.Count) % redTilePositions.Count];
            Vector2 next = redTilePositions[(i + 1) % redTilePositions.Count];

            if (next.Y == current.Y && next.X > current.X && prev.Y < current.Y)
            {
                // BL
                posToCornerType[current] = CornerType.BottomLeft;
            }
            else if (next.Y == current.Y && next.X > current.X && prev.Y > current.Y)
            {
                // TL
                posToCornerType[current] = CornerType.TopLeft;
            }
            else if (prev.Y == current.Y && prev.X > current.X && next.Y < current.Y)
            {
                // BL
                posToCornerType[current] = CornerType.BottomLeft;
            }
            else if (prev.Y == current.Y && prev.X > current.X && next.Y > current.Y)
            {
                // TL
                posToCornerType[current] = CornerType.TopLeft;
            }
            else if (next.Y == current.Y && next.X < current.X && prev.Y < current.Y)
            {
                // BR
                posToCornerType[current] = CornerType.BottomRight;
            }
            else if (next.Y == current.Y && next.X < current.X && prev.Y > current.Y)
            {
                // TR
                posToCornerType[current] = CornerType.TopRight;
            }
            else if (prev.Y == current.Y && prev.X < current.X && next.Y < current.Y)
            {
                // BR
                posToCornerType[current] = CornerType.BottomRight;
            }
            else if (prev.Y == current.Y && prev.X < current.X && next.Y > current.Y)
            {
                // TR
                posToCornerType[current] = CornerType.TopRight;
            }
        }

        Dictionary<int, List<Wall>> verticalWalls = [];
        Dictionary<int, List<Wall>> horizontalWalls = [];

        // Groups vertical and horizontal walls by x and y value
        for (int i = 0; i < redTilePositions.Count + 1; i++)
        {
            Vector2 posOne = redTilePositions[i % redTilePositions.Count];
            Vector2 posTwo = redTilePositions[(i - 1 + redTilePositions.Count) % redTilePositions.Count];
            Wall wall = new(posOne, posTwo);

            if (wall.IsVertical)
            {
                if (verticalWalls.TryGetValue(wall.RedTileOne.X, out List<Wall>? value))
                {
                    value.Add(wall);
                }
                else
                {
                    verticalWalls[wall.RedTileOne.X] = [wall];
                }
            }
            else
            {
                if (horizontalWalls.TryGetValue(wall.RedTileOne.Y, out List<Wall>? value))
                {
                    value.Add(wall);
                }
                else
                {
                    horizontalWalls[wall.RedTileOne.Y] = [wall];
                }
            }
        }

        List<int> xValues = [.. verticalWalls.Select(k => k.Key).OrderBy(x => x)];
        List<int> yValues = [.. horizontalWalls.Select(k => k.Key).OrderBy(y => y)];

        List<Wall> segments = [];

        // Goes through each y value and adds full horizontal walls that are in the shape
        foreach (int y in yValues)
        {
            Stack<Vector2> stack = [];
            Vector2 enteredPos = -Vector2.One;

            foreach (int x in xValues)
            {
                Vector2 pos = new(x, y);
                bool posIsCorner = posToCornerType.ContainsKey(pos);

                if (stack.Count == 0)
                {
                    if (posIsCorner)
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                    else if (verticalWalls[x].Any(w => w.Intersects(pos)))
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                }
                else
                {
                    if (posIsCorner)
                    {
                        Vector2 peeked = stack.Peek();

                        if (!posToCornerType.ContainsKey(peeked))
                        {
                            stack.Push(pos);
                            continue;
                        }

                        if (CancelsMovingX(posToCornerType[peeked], posToCornerType[pos]))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            stack.Push(pos);
                        }

                        if (!posToCornerType.ContainsKey(enteredPos))
                        {
                            bool cornersFormVerticalWall = CornersFormWall(posToCornerType[peeked], posToCornerType[pos]);

                            if (cornersFormVerticalWall)
                            {
                                stack.Clear();
                            }
                        }
                    }
                    else if (verticalWalls[x].Any(w => w.Intersects(pos)))
                    {
                        stack.Clear();
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
                throw new Exception($"Never exited row {y}");
            }
        }

        // Goes through each x value and adds full vertical walls that are in the shape
        foreach (int x in xValues)
        {
            Stack<Vector2> stack = [];
            Vector2 enteredPos = -Vector2.One;

            foreach (int y in yValues)
            {
                Vector2 pos = new(x, y);
                bool posIsCorner = posToCornerType.ContainsKey(pos);

                if (stack.Count == 0)
                {
                    if (posIsCorner)
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                    else if (horizontalWalls[y].Any(w => w.Intersects(pos)))
                    {
                        enteredPos = pos;
                        stack.Push(pos);
                    }
                }
                else
                {
                    if (posIsCorner)
                    {
                        Vector2 peeked = stack.Peek();

                        if (!posToCornerType.ContainsKey(peeked))
                        {
                            stack.Push(pos);
                            continue;
                        }

                        if (CancelsMovingY(posToCornerType[peeked], posToCornerType[pos]))
                        {
                            stack.Pop();
                        }
                        else
                        {
                            stack.Push(pos);
                        }

                        if (!posToCornerType.ContainsKey(enteredPos))
                        {
                            bool cornersFormHorizontalWall = CornersFormWall(posToCornerType[peeked], posToCornerType[pos]);

                            if (cornersFormHorizontalWall)
                            {
                                stack.Clear();
                            }
                        }
                    }
                    else if (horizontalWalls[y].Any(w => w.Intersects(pos)))
                    {
                        stack.Clear();
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
                throw new Exception($"Never exited col {x}");
            }
        }

        PriorityQueue<(Vector2, Vector2), long> priorityQueue = new();

        for (int i = 0; i < redTilePositions.Count; i++)
        {
            for (int j = i + 1; j < redTilePositions.Count; j++)
            {
                long width = Math.Abs(redTilePositions[i].X - redTilePositions[j].X) + 1;
                long height = Math.Abs(redTilePositions[i].Y - redTilePositions[j].Y) + 1;
                priorityQueue.Enqueue((redTilePositions[i], redTilePositions[j]), -width * height);
            }
        }

        while (priorityQueue.Count > 0)
        {
            (Vector2, Vector2) pair = priorityQueue.Dequeue();

            Vector2 posOne = pair.Item1;
            Vector2 posTwo = pair.Item2;

            bool existsWallOne = segments.Any(w => w.Intersects(posOne) && w.Intersects(new(posTwo.X, posOne.Y)));
            bool existsWallTwo = segments.Any(w => w.Intersects(posOne) && w.Intersects(new(posOne.X, posTwo.Y)));
            bool existsWallThree = segments.Any(w => w.Intersects(posTwo) && w.Intersects(new(posOne.X, posTwo.Y)));
            bool existsWallFour = segments.Any(w => w.Intersects(posTwo) && w.Intersects(new(posTwo.X, posOne.Y)));

            if (existsWallOne && existsWallTwo && existsWallThree && existsWallFour)
            {
                long width = Math.Abs(posOne.X - posTwo.X) + 1;
                long height = Math.Abs(posOne.Y - posTwo.Y) + 1;

                return width * height;
            }
        }

        return -1;
    }

    private static void PrintCornerType(CornerType cornerType)
    {
        if (cornerType == CornerType.TopLeft)
        {
            Console.Write("┏");
        }
        else if (cornerType == CornerType.TopRight)
        {
            Console.Write("┓");
        }
        else if (cornerType == CornerType.BottomRight)
        {
            Console.Write("┛");
        }
        else
        {
            Console.Write("┗");
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

    private static bool CornersFormWall(CornerType cornerTypeOne, CornerType cornerTypeTwo)
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