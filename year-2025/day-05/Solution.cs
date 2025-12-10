using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2025.Day05;

[PuzzleName("Cafeteria")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int numFresh = 0;
        List<Range> ranges = [];

        foreach (string line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (line.Contains('-'))
            {
                string[] split = line.Split('-');
                Range range = new()
                {
                    Start = long.Parse(split[0]),
                    End = long.Parse(split[1]),
                };

                ranges.Add(range);
            }
            else
            {
                foreach (Range range in ranges)
                {
                    long id = long.Parse(line);

                    if (id >= range.Start && id <= range.End)
                    {
                        numFresh++;
                        break;
                    }
                }
            }
        }

        return numFresh;
    }

    public override object SolvePartTwo(string[] input)
    {
        List<Range> ranges = [];

        foreach (string line in input)
        {
            if (!line.Contains('-'))
            {
                break;
            }

            string[] split = line.Split('-');
            Range newRange = new()
            {
                Start = long.Parse(split[0]),
                End = long.Parse(split[1]),
            };

            ranges.Add(newRange);
        }

        bool removed;
        do
        {
            removed = false;

            for (int i = 0; i < ranges.Count; i++)
            {
                for (int j = i + 1; j < ranges.Count; j++)
                {
                    if (ranges[i].Start == ranges[j].Start && ranges[i].End == ranges[j].End)
                    {
                        ranges.Remove(ranges[j]);
                        removed = true;
                        break;
                    }

                    if ((ranges[i].Start >= ranges[j].Start && ranges[i].Start <= ranges[j].End) || (ranges[i].End >= ranges[j].Start && ranges[i].End <= ranges[j].End))
                    {
                        Range newRange = new()
                        {
                            Start = Math.Min(ranges[j].Start, ranges[i].Start),
                            End = Math.Max(ranges[j].End, ranges[i].End),
                        };

                        ranges.Remove(ranges[j]);
                        ranges.Remove(ranges[i]);
                        ranges.Add(newRange);
                        removed = true;
                        break;
                    }

                    if ((ranges[j].Start >= ranges[i].Start && ranges[j].Start <= ranges[i].End) || (ranges[j].End >= ranges[i].Start && ranges[j].End <= ranges[i].End))
                    {
                        Range newRange = new()
                        {
                            Start = Math.Min(ranges[j].Start, ranges[i].Start),
                            End = Math.Max(ranges[j].End, ranges[i].End),
                        };

                        ranges.Remove(ranges[j]);
                        ranges.Remove(ranges[i]);
                        ranges.Add(newRange);
                        removed = true;
                        break;
                    }
                }

                if (removed)
                {
                    break;
                }
            }
        }
        while (removed);

        long numFreshIds = 0;

        foreach (Range range in ranges)
        {
            numFreshIds += range.End - range.Start + 1;
        }

        return numFreshIds;
    }
}

public struct Range
{
    public long Start;
    public long End;
}