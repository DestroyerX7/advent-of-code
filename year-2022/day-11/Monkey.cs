using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2022.Day11;

public class Monkey
{
    public List<long> WorryLevels = new List<long>();
    public string Operation = "";
    public long TestNum;
    public long TrueNum;
    public long FalseNum;
    public long InspectNum = 0;

    public void AddItems(string items)
    {
        while (items.Length > 0)
        {
            WorryLevels.Add(long.Parse(items.Substring(0, 2)));

            if (items.Length > 2)
            {
                items = items.Substring(items.IndexOf(" ") + 1);
            }
            else
            {
                items = "";
            }
        }
    }

    public void ChangeWorryLevels(long? LCM)
    {
        if (Operation.Contains("*"))
        {
            for (int i = 0; i < WorryLevels.Count; i++)
            {
                long multiplier;
                if (long.TryParse(Operation.Substring(Operation.IndexOf("*") + 2), out multiplier))
                {
                    WorryLevels[i] *= multiplier;
                }
                else
                {
                    WorryLevels[i] *= WorryLevels[i];
                }
            }
        }
        else
        {
            long addValue = long.Parse(Operation.Substring(Operation.IndexOf("+") + 2));

            for (int i = 0; i < WorryLevels.Count; i++)
            {
                WorryLevels[i] += addValue;
            }
        }


        if (LCM == null)
        {
            //Part 1
            for (int i = 0; i < WorryLevels.Count; i++)
            {
                double worryLevel = WorryLevels[i] / 3;
                WorryLevels[i] = (long)Math.Floor(worryLevel);
            }
        }
        else
        {
            //Part 2
            for (int i = 0; i < WorryLevels.Count; i++)
            {
                WorryLevels[i] %= (long)LCM;
            }
        }

        InspectNum += WorryLevels.Count;
    }

    public void ThrowItems()
    {
        for (int i = 0; i < WorryLevels.Count; i++)
        {
            if (WorryLevels[i] % TestNum == 0)
            {
                Solution.Monkeys[(int)TrueNum].WorryLevels.Add(WorryLevels[i]); // Changed from Program.Monkeys to Solution.Monkeys in 2025 to fit structure
            }
            else
            {
                Solution.Monkeys[(int)FalseNum].WorryLevels.Add(WorryLevels[i]); // Changed from Program.Monkeys to Solution.Monkeys in 2025 to fit structure
            }
        }

        WorryLevels.Clear();
    }
}
