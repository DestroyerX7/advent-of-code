using System;
using System.Linq;

namespace AdventOfCode.Year2024.Day07;

[PuzzleName("Bridge Repair")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        long totalCalibrationResultPartOne = 0;

        foreach (string line in input)
        {
            string[] split = line.Split(": ");
            long testValue = long.Parse(split[0]);
            int[] nums = split[1].Split(' ').Select(int.Parse).ToArray();

            if (ValidPermutationPartOne(testValue, nums))
            {
                totalCalibrationResultPartOne += testValue;
            }
        }

        return totalCalibrationResultPartOne;
    }

    public override object SolvePartTwo(string[] input)
    {
        long totalCalibrationResultPartTwo = 0;

        foreach (string line in input)
        {
            string[] split = line.Split(": ");
            long testValue = long.Parse(split[0]);
            int[] nums = split[1].Split(' ').Select(int.Parse).ToArray();

            if (ValidPermutationPartTwo(testValue, nums))
            {
                totalCalibrationResultPartTwo += testValue;
            }
        }

        return totalCalibrationResultPartTwo;
    }

    private static bool ValidPermutationPartOne(long testValue, int[] nums)
    {
        long numPermutations = (long)Math.Pow(2, nums.Length - 1);

        for (int i = 0; i < numPermutations; i++)
        {
            long sum = nums[0];
            string binary = Convert.ToString(i, 2);
            char[] charArray = binary.ToCharArray().Reverse().ToArray();
            binary = new string(charArray);

            for (int j = 1; j < nums.Length && sum <= testValue; j++)
            {
                if (j - 1 >= binary.Length || binary[j - 1] == '0')
                {
                    sum *= nums[j];
                }
                else
                {
                    sum += nums[j];
                }
            }

            if (sum == testValue)
            {
                return true;
            }
        }

        return false;
    }

    private static bool ValidPermutationPartTwo(long testValue, int[] nums)
    {
        long numPermutations = (long)Math.Pow(3, nums.Length - 1);

        for (int i = 0; i < numPermutations; i++)
        {
            long sum = nums[0];
            string binary = ToBaseThree(i);
            char[] charArray = binary.ToCharArray().Reverse().ToArray();
            binary = new string(charArray);

            for (int j = 1; j < nums.Length && sum <= testValue; j++)
            {
                if (j - 1 >= binary.Length || binary[j - 1] == '0')
                {
                    sum *= nums[j];
                }
                else if (binary[j - 1] == '1')
                {
                    sum += nums[j];
                }
                else
                {
                    string sumString = sum.ToString();
                    sumString += nums[j];
                    sum = long.Parse(sumString);
                }
            }

            if (sum == testValue)
            {
                return true;
            }
        }

        return false;
    }

    private static string ToBaseThree(long num)
    {
        string baseThree = "";

        while (num != 0)
        {
            int remainder = (int)num % 3;
            num /= 3;
            baseThree += remainder;
        }

        char[] charArray = baseThree.ToCharArray().Reverse().ToArray();
        return new string(charArray);
    }
}