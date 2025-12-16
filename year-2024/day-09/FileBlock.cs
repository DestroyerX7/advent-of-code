using System;

namespace AdventOfCode.Year2024.Day09;

public class FileBlock
{
    public int Id { get; private set; }
    public string[] Files { get; private set; }
    public int Size { get; private set; }
    public bool IsFreeSpace { get; private set; }
    public bool AlreadyMoved { get; private set; }

    public FileBlock(int size)
    {
        IsFreeSpace = true;
        Size = size;
        Files = new string[Size];

        for (int i = 0; i < Size; i++)
        {
            Files[i] = ".";
        }
    }

    public FileBlock(int size, int id)
    {
        Id = id;
        Size = size;
        Files = new string[size];

        for (int i = 0; i < Size; i++)
        {
            Files[i] = Id.ToString();
        }
    }

    public void SetFreeSpaceAmount(int amount)
    {
        Size = amount;
        Files = new string[Size];

        for (int i = 0; i < Size; i++)
        {
            Files[i] = ".";
        }
    }

    public int GetIdSum()
    {
        int sum = 0;

        for (int i = Id; i < Id + Size; i++)
        {
            int num = int.Parse(Files[i - Id]);
            sum += i * num;
        }

        return sum;
    }

    public void Print()
    {
        foreach (string file in Files)
        {
            Console.Write(file);
        }
    }

    public void SetAsMoved()
    {
        AlreadyMoved = true;
    }
}