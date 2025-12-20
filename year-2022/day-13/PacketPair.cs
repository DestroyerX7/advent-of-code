namespace AdventOfCode.Year2022.Day13;

public class PacketPair
{
    public Packet PacketOne = new Packet();
    public Packet PacketTwo = new Packet();

    public PacketPair(string packetOneString, string packetTwoString)
    {
        PacketOne.AddData(packetOneString);
        PacketTwo.AddData(packetTwoString);
    }

    public bool CompareValues()
    {
        while (PacketOne.Data.Count > 0 && PacketTwo.Data.Count > 0)
        {
            int[] packetOneData = PacketOne.GetData();
            int[] packetTwoData = PacketTwo.GetData();

            for (int i = 0; i < packetOneData.Length; i++)
            {
                if (i >= packetTwoData.Length)
                {
                    return false;
                }
                else if (packetOneData[i] < packetTwoData[i])
                {
                    return true;
                }
                else if (packetOneData[i] > packetTwoData[i])
                {
                    return false;
                }
                else if (i == packetOneData.Length - 1 && packetTwoData.Length > i + 1)
                {
                    return true;
                }
            }
        }

        if (PacketOne.Data.Count == 0 && PacketTwo.Data.Count > 0)
        {
            return true;
        }
        else if (PacketTwo.Data.Count == 0 && PacketOne.Data.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
