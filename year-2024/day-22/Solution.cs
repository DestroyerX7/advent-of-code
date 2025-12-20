using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day22;

[PuzzleName("Monkey Market")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        Dictionary<string, int>[] sequenceToSellPrices = new Dictionary<string, int>[input.Length];

        long sumSecrets = 0;

        for (int i = 0; i < input.Length; i++)
        {
            long currentSecret = long.Parse(input[i]);

            int[] priceChanges = new int[2000];
            sequenceToSellPrices[i] = new();

            for (int j = 0; j < 2000; j++)
            {
                int oldPrice = (int)(currentSecret % 10);

                currentSecret = (currentSecret * 64) ^ currentSecret;
                currentSecret %= 16777216;

                currentSecret = (currentSecret / 32) ^ currentSecret;
                currentSecret %= 16777216;

                currentSecret = (currentSecret * 2048) ^ currentSecret;
                currentSecret %= 16777216;

                int newPrice = (int)(currentSecret % 10);
                priceChanges[j] = newPrice - oldPrice;

                if (j >= 3)
                {
                    string sequence = $"{priceChanges[j - 3]}{priceChanges[j - 2]}{priceChanges[j - 1]}{priceChanges[j]}";

                    if (!sequenceToSellPrices[i].ContainsKey(sequence))
                    {
                        sequenceToSellPrices[i][sequence] = newPrice;
                    }
                }
            }

            sumSecrets += currentSecret;
        }

        return sumSecrets;
    }

    public override object SolvePartTwo(string[] input)
    {
        Dictionary<string, int>[] sequenceToSellPrices = new Dictionary<string, int>[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            long currentSecret = long.Parse(input[i]);

            int[] priceChanges = new int[2000];
            sequenceToSellPrices[i] = new();

            for (int j = 0; j < 2000; j++)
            {
                int oldPrice = (int)(currentSecret % 10);

                currentSecret = (currentSecret * 64) ^ currentSecret;
                currentSecret %= 16777216;

                currentSecret = (currentSecret / 32) ^ currentSecret;
                currentSecret %= 16777216;

                currentSecret = (currentSecret * 2048) ^ currentSecret;
                currentSecret %= 16777216;

                int newPrice = (int)(currentSecret % 10);
                priceChanges[j] = newPrice - oldPrice;

                if (j >= 3)
                {
                    string sequence = $"{priceChanges[j - 3]}{priceChanges[j - 2]}{priceChanges[j - 1]}{priceChanges[j]}";

                    if (!sequenceToSellPrices[i].ContainsKey(sequence))
                    {
                        sequenceToSellPrices[i][sequence] = newPrice;
                    }
                }
            }
        }

        string[] sequences = sequenceToSellPrices.SelectMany(d => d.Keys).Distinct().ToArray();
        long mostBananas = long.MinValue;

        foreach (string sequence in sequences)
        {
            long numBananas = 0;

            foreach (Dictionary<string, int> sequenceToSellPrice in sequenceToSellPrices)
            {
                if (sequenceToSellPrice.ContainsKey(sequence))
                {
                    numBananas += sequenceToSellPrice[sequence];
                }
            }

            if (numBananas > mostBananas)
            {
                mostBananas = numBananas;
            }
        }

        return mostBananas;
    }
}