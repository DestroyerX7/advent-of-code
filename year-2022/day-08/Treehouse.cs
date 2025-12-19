using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022.Day08;

public class Treehouse
{
    public static int NumVisable(List<List<int>> list)
    {
        int numVisable = list.Count * list.Count;

        for (int i = 1; i < list.Count - 1; i++)
        {
            for (int j = 1; j < list[i].Count - 1; j++)
            {
                if (!CheckRow(list[i][j], j, list[i]) && !CheckCollum(list[i][j], j, i, list))
                {
                    numVisable--;
                }
            }
        }

        return numVisable;
    }

    public static bool CheckRow(int num, int pos, List<int> list)
    {
        bool left = true;
        bool right = true;

        for (int i = 0; i < pos; i++)
        {
            if (list[i] >= num)
            {
                left = false;
            }
        }

        for (int i = pos + 1; i < list.Count; i++)
        {
            if (list[i] >= num)
            {
                right = false;
            }
        }

        return left || right;
    }

    public static bool CheckCollum(int num, int xpos, int ypos, List<List<int>> list)
    {
        bool top = true;
        bool bottom = true;

        for (int i = 0; i < ypos; i++)
        {
            if (list[i][xpos] >= num)
            {
                top = false;
            }
        }

        for (int i = ypos + 1; i < list.Count; i++)
        {
            if (list[i][xpos] >= num)
            {
                bottom = false;
            }
        }

        return top || bottom;
    }

    public static int BestViewingDistance(List<List<int>> list)
    {
        int numVisable = list.Count * list.Count;
        List<int> viewingDistances = new List<int>();

        for (int i = 1; i < list.Count - 1; i++)
        {
            for (int j = 1; j < list[i].Count - 1; j++)
            {
                viewingDistances.Add(CalculateDistance(list[i][j], j, i, list));
            }
        }

        return viewingDistances.Max();
    }

    public static int CalculateDistance(int num, int xpos, int ypos, List<List<int>> list)
    {
        int left = 1;
        int right = 1;

        for (int i = xpos - 1; i > 0; i--)
        {
            if (list[ypos][i] >= num)
            {
                break;
            }
            else
            {
                left++;
            }
        }

        for (int i = xpos + 1; i < list[ypos].Count - 1; i++)
        {
            if (list[ypos][i] >= num)
            {
                break;
            }
            else
            {
                right++;
            }
        }

        int top = 1;
        int bottom = 1;

        for (int i = ypos - 1; i > 0; i--)
        {
            if (list[i][xpos] >= num)
            {
                break;
            }
            else
            {
                top++;
            }
        }

        for (int i = ypos + 1; i < list.Count - 1; i++)
        {
            if (list[i][xpos] >= num)
            {
                break;
            }
            else
            {
                bottom++;
            }
        }

        return top * right * bottom * left;
    }
}
