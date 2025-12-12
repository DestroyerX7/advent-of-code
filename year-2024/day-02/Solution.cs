using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2024.Day02;

[PuzzleName("Red-Nosed Reports")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int numSafe = 0;

        foreach (string line in input)
        {
            int[] nums = line.Split(' ').Select(int.Parse).ToArray();

            bool isSafe = false;

            if (nums[0] < nums[1])
            {
                isSafe = CheckAscending(nums);
            }
            else
            {
                isSafe = CheckDescending(nums);
            }

            if (isSafe)
            {
                numSafe++;
            }
        }

        return numSafe;

        bool CheckAscending(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1; i++)
            {
                if (nums[i] >= nums[i + 1] || nums[i] + 3 < nums[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        bool CheckDescending(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1; i++)
            {
                if (nums[i] <= nums[i + 1] || nums[i] - 3 > nums[i + 1])
                {
                    return false;
                }
            }

            return true;
        }
    }

    public override object SolvePartTwo(string[] input)
    {
        int numSafe = 0;

        foreach (string line in input)
        {
            List<int> nums = line.Split(' ').Select(int.Parse).ToList();

            bool isSafe = CheckAscending(nums, true) || CheckDescending(nums, true);

            if (isSafe)
            {
                numSafe++;
            }
        }

        return numSafe;

        // Kinda iffy making 2 separate new lists, but idc, it works.
        bool CheckAscending(List<int> nums, bool canRemove)
        {
            List<int> newNums = new(nums);

            for (int i = 0; i < newNums.Count - 1; i++)
            {
                if (newNums[i] >= newNums[i + 1] || newNums[i] + 3 < newNums[i + 1])
                {
                    if (canRemove)
                    {
                        List<int> another = new(nums);
                        another.RemoveAt(i);
                        newNums.RemoveAt(i + 1);
                        return CheckAscending(newNums, false) || CheckAscending(another, false);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        bool CheckDescending(List<int> nums, bool canRemove)
        {
            List<int> newNums = new(nums);

            for (int i = 0; i < newNums.Count - 1; i++)
            {
                if (newNums[i] <= newNums[i + 1] || newNums[i] - 3 > newNums[i + 1])
                {
                    if (canRemove)
                    {
                        List<int> another = new(nums);
                        another.RemoveAt(i);
                        newNums.RemoveAt(i + 1);
                        return CheckDescending(newNums, false) || CheckDescending(another, false);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}