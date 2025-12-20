using System.Collections;

namespace AdventOfCode.Year2022.Day13;

public class Packet
{
    public ArrayList Data = new ArrayList();
    public Packet? Parent;

    public void AddData(string dataString)
    {
        int addValue = 0;
        int numBrackets = 0;
        string currentString = dataString.Substring(1, dataString.Length - 2);

        int index = 0;
        while (currentString.Length > 0)
        {
            if (currentString[index] == '[')
            {
                numBrackets++;
            }
            else if (currentString[index] == ']')
            {
                numBrackets--;

                if (numBrackets == 0)
                {
                    Packet newPacket = new Packet();
                    newPacket.Parent = this;
                    Data.Add(newPacket);
                    newPacket.AddData(currentString.Substring(0, index + 1));
                    currentString = currentString.Substring(index + 1).Trim(',');
                    index = 0;
                    continue;
                }
            }
            else if (numBrackets == 0 && currentString.Contains(',') && int.TryParse(currentString.Substring(0, currentString.IndexOf(',')), out addValue))
            {
                Data.Add(addValue);
                currentString = currentString.Substring(currentString.IndexOf(",") + 1);
                index = 0;
                continue;
            }
            else if (numBrackets == 0 && int.TryParse(currentString.Substring(0), out addValue))
            {
                Data.Add(addValue);
                break;
            }

            index++;
        }
    }

    public int[] GetData()
    {
        bool allInts = true;
        foreach (var item in Data)
        {
            if (item.GetType() != typeof(int))
            {
                allInts = false;
            }
        }

        if (allInts)
        {
            int[] items = new int[Data.Count];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = (int)(Data[i] ?? 0); // Added ?? 0 in 2025 to fix terminal warning
            }

            if (Parent != null)
            {
                Parent.Data.RemoveAt(0);
            }
            else
            {
                Data.Clear();
            }

            return items;
        }
        else if (Data[0]?.GetType() == typeof(int))
        {
            int[] array = new int[] { (int)(Data[0] ?? 0) }; // Added ?? 0 in 2025 to fix terminal warning
            Data.RemoveAt(0);
            return array;
        }
        else
        {
            object? obj = Data[0] ?? throw new System.Exception("Object in Data is null."); // Added in 2025 to fix terminal warning
            Packet newPacket = (Packet)obj;
            return newPacket.GetData();
        }
    }
}
