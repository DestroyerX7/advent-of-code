namespace AdventOfCode.Year2022.Day04;

public class Cleanup
{
    public static bool FullyContain(string pair1, string pair2)
    {
        int index1 = pair1.IndexOf("-");
        int index2 = pair2.IndexOf("-");

        int lowerBounds1 = int.Parse(pair1.Substring(0, index1));
        int upperBounds1 = int.Parse(pair1.Substring(index1 + 1));
        int lowerBounds2 = int.Parse(pair2.Substring(0, index2));
        int upperBounds2 = int.Parse(pair2.Substring(index2 + 1));

        if (lowerBounds1 == lowerBounds2 || upperBounds1 == upperBounds2)
        {
            return true;
        }
        else if (lowerBounds1 > lowerBounds2)
        {
            return upperBounds1 < upperBounds2;
        }
        else
        {
            return upperBounds2 < upperBounds1;
        }
    }

    public static bool Overlap(string pair1, string pair2)
    {
        int index1 = pair1.IndexOf("-");
        int index2 = pair2.IndexOf("-");

        int lowerBounds1 = int.Parse(pair1.Substring(0, index1));
        int upperBounds1 = int.Parse(pair1.Substring(index1 + 1));
        int lowerBounds2 = int.Parse(pair2.Substring(0, index2));
        int upperBounds2 = int.Parse(pair2.Substring(index2 + 1));

        for (int i = lowerBounds1; i <= upperBounds1; i++)
        {
            if (i >= lowerBounds2 && i <= upperBounds2)
            {
                return true;
            }
        }

        return false;
    }
}
