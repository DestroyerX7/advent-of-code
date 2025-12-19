using System.Collections.Generic;


namespace AdventOfCode.Year2022.Day03;

public class Rucksack
{
    public static int Priorities(string text)
    {
        string lowerAlphabet = "abcdefghijklmnopqrstuvwxyz";
        string upperAlphabet = lowerAlphabet.ToUpper();
        Dictionary<char, int> lowerValues = new Dictionary<char, int>();
        Dictionary<char, int> upperValues = new Dictionary<char, int>();

        string str1 = text.Substring(0, text.Length / 2);
        string str2 = text.Substring(text.Length / 2);
        char same = 'a';

        for (int i = 0; i < str1.Length; i++)
        {
            if (str2.Contains(str1[i]))
            {
                same = str1[i];
                break;
            }
        }

        for (int i = 1; i <= lowerAlphabet.Length; i++)
        {
            lowerValues.Add(lowerAlphabet[i - 1], i);
            upperValues.Add(upperAlphabet[i - 1], i + 26);
        }

        if (lowerValues.ContainsKey(same))
        {
            return lowerValues[same];
        }
        else
        {
            return upperValues[same];
        }
    }

    public static int PartTwo(string elf1, string elf2, string elf3)
    {
        string lowerAlphabet = "abcdefghijklmnopqrstuvwxyz";
        string upperAlphabet = lowerAlphabet.ToUpper();
        Dictionary<char, int> lowerValues = new Dictionary<char, int>();
        Dictionary<char, int> upperValues = new Dictionary<char, int>();

        char same = 'a';

        foreach (char letter in elf1)
        {
            if (elf2.Contains(letter) && elf3.Contains(letter))
            {
                same = letter;
                break;
            }
        }

        for (int i = 1; i <= lowerAlphabet.Length; i++)
        {
            lowerValues.Add(lowerAlphabet[i - 1], i);
            upperValues.Add(upperAlphabet[i - 1], i + 26);
        }

        if (lowerValues.ContainsKey(same))
        {
            return lowerValues[same];
        }
        else
        {
            return upperValues[same];
        }
    }
}
