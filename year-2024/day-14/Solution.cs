using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day14;

[PuzzleName("Restroom Redoubt")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Regex regex = new(@"-?\d+");

        Robot[] robots = new Robot[input.Length];
        int[] numInQuadrant = new int[4];
        int gridWidth = 101;
        int gridHeight = 103;

        for (int i = 0; i < input.Length; i++)
        {
            MatchCollection matches = regex.Matches(input[i]);
            Vector2 pos = new(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
            Vector2 velocity = new(int.Parse(matches[2].Value), int.Parse(matches[3].Value));
            robots[i] = new(pos, velocity);
            robots[i].MoveForSeconds(100, gridWidth, gridHeight);

            if (robots[i].Position.X > gridWidth / 2 && robots[i].Position.Y < gridHeight / 2)
            {
                numInQuadrant[0]++;
            }
            else if (robots[i].Position.X < gridWidth / 2 && robots[i].Position.Y < gridHeight / 2)
            {
                numInQuadrant[1]++;
            }
            else if (robots[i].Position.X < gridWidth / 2 && robots[i].Position.Y > gridHeight / 2)
            {
                numInQuadrant[2]++;
            }
            else if (robots[i].Position.X > gridWidth / 2 && robots[i].Position.Y > gridHeight / 2)
            {
                numInQuadrant[3]++;
            }
        }

        int safetyFactor = 1;

        foreach (int num in numInQuadrant)
        {
            safetyFactor *= num;
        }

        return safetyFactor;
    }

    public override object SolvePartTwo(string[] input)
    {
        Regex regex = new(@"-?\d+");

        Robot[] robots = new Robot[input.Length];

        int gridWidth = 101;
        int gridHeight = 103;

        for (int i = 0; i < input.Length; i++)
        {
            MatchCollection matches = regex.Matches(input[i]);
            Vector2 pos = new(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
            Vector2 velocity = new(int.Parse(matches[2].Value), int.Parse(matches[3].Value));
            robots[i] = new(pos, velocity);
        }

        // Still a pretty inefficient solution that looks for a horizontal line of robots 25 in length.
        // But, hey it's better than having to manually check the grid for a tree.
        int numSeconds = 0;
        while (true)
        {
            foreach (Robot robot in robots)
            {
                robot.MoveForSeconds(1, gridWidth, gridHeight);
            }

            numSeconds++;

            foreach (Robot robot in robots)
            {
                bool lineFound = true;
                Vector2 currentPos = robot.Position;

                for (int i = 0; i < 25; i++)
                {
                    if (robots.All(r => r.Position != currentPos + Vector2.Right))
                    {
                        lineFound = false;
                        break;
                    }

                    currentPos += Vector2.Right;
                }

                if (lineFound)
                {
                    return numSeconds;
                }
            }
        }
    }

    public static object SolvePartTwoManual(string[] input)
    {
        Regex regex = new(@"-?\d+");

        Robot[] robots = new Robot[input.Length];

        int gridWidth = 101;
        int gridHeight = 103;

        for (int i = 0; i < input.Length; i++)
        {
            MatchCollection matches = regex.Matches(input[i]);
            Vector2 pos = new(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
            Vector2 velocity = new(int.Parse(matches[2].Value), int.Parse(matches[3].Value));
            robots[i] = new(pos, velocity);
        }

        // Maybe one of the dumbest solutions I have come up with.
        // A little manual review required lol.
        int numSeconds = 0;
        while (true)
        {
            foreach (Robot robot in robots)
            {
                robot.MoveForSeconds(1, gridWidth, gridHeight);
            }

            numSeconds++;

            foreach (Robot robot in robots)
            {
                int numConnections = 0;

                Vector2 direction = Vector2.Right;
                for (int i = 0; i < 4; i++)
                {
                    if (robots.Any(r => r.Position == robot.Position + direction))
                    {
                        numConnections++;
                    }

                    direction = direction.Rotate90DegreesClockwise();
                }

                if (numConnections == 4)
                {
                    PrintGrid(robots, gridWidth, gridHeight);

                    string response = Console.ReadLine() ?? "";

                    if (response == "y")
                    {
                        return numSeconds;
                    }
                }
            }
        }
    }

    private static void PrintGrid(Robot[] robots, int gridWidth, int gridHeight)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (robots.Any(r => r.Position == new Vector2(x, y)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }
}