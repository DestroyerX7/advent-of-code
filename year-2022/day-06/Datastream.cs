using System.Linq;

namespace AdventOfCode.Year2022.Day06;

public class Datastream
{
    public static int StartOfPacket(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            string str = text.Substring(i, 4);

            if (str.All(c => str.IndexOf(c) == str.LastIndexOf(c)))
            {
                return i + 4;
            }
        }

        return -1;
    }

    public static int StartOfMessage(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            string str = text.Substring(i, 14);

            if (str.All(c => str.IndexOf(c) == str.LastIndexOf(c)))
            {
                return i + 14;
            }
        }

        return -1;
    }
}
