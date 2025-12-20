using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2022.Day01;

public class Calories
{
    // Adde topCount in 2025 to make it usable for both parts since I didn't save my code for part 1 oringinally
    public static int MaxCalories(string[] input, int topCount)
    {
        List<int> elfCalories = new List<int>();
        int sum = 0;
        int topSum = 0;

        foreach (string line in input)
        {
            int num;
            if (int.TryParse(line, out num))
            {
                sum += num;
            }
            else
            {
                elfCalories.Add(sum);
                sum = 0;
                continue;
            }
        }

        for (int i = 0; i < topCount; i++)
        {
            int top = elfCalories.Max();
            topSum += top;
            elfCalories.Remove(top);
        }

        return topSum;
    }
}
