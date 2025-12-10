namespace AdventOfCode.Year2025.Day01;

[PuzzleName("Secret Entrance")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int count = 0;

        int currentNum = 50;

        foreach (string line in input)
        {
            int num = int.Parse(line[1..]);

            if (line.Contains('L'))
            {
                currentNum += 100 - num;
            }
            else
            {
                currentNum += num;
            }

            currentNum %= 100;

            if (currentNum == 0)
            {
                count++;
            }
        }

        return count;
    }

    public override object SolvePartTwo(string[] input)
    {
        int count = 0;

        int currentNum = 50;

        foreach (string line in input)
        {
            int num = int.Parse(line[1..]);

            if (line.Contains('R'))
            {
                for (int i = 0; i < num; i++)
                {
                    currentNum++;
                    currentNum %= 100;

                    if (currentNum == 0)
                    {
                        count++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    currentNum--;

                    if (currentNum < 0)
                    {
                        currentNum = 99;
                    }

                    if (currentNum == 0)
                    {
                        count++;
                    }
                }
            }

        }

        return count;
    }
}