using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2022.Day10;

[PuzzleName("Cathode-Ray Tube")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int register = 1;
        List<Instruction> instructions = new List<Instruction>();
        int registerSum = 0;
        char[] pixels = new char[240];

        foreach (string line in input)
        {
            Instruction newInstruction;
            if (line.Contains("noop"))
            {
                newInstruction = new Instruction(1, 0);
            }
            else
            {
                int @value = int.Parse(line.Substring(5));
                newInstruction = new Instruction(2, @value);
            }

            instructions.Add(newInstruction);
        }

        for (int i = 1; i <= 240; i++)
        {
            if ((i - 20) % 40 == 0)
            {
                registerSum += register * i;
            }

            if ((i - 1) % 40 >= register - 1 && (i - 1) % 40 <= register + 1)
            {
                pixels[i - 1] = '█';
            }
            else
            {
                pixels[i - 1] = ' ';
            }

            Instruction currentInstruction = instructions[0];
            currentInstruction.Duration--;

            if (currentInstruction.Duration == 0)
            {
                register += currentInstruction.Value;
                instructions.RemoveAt(0);
            }
        }

        return registerSum;
    }

    public override object SolvePartTwo(string[] input)
    {
        int register = 1;
        List<Instruction> instructions = new List<Instruction>();
        char[] pixels = new char[240];

        foreach (string line in input)
        {
            Instruction newInstruction;
            if (line.Contains("noop"))
            {
                newInstruction = new Instruction(1, 0);
            }
            else
            {
                int @value = int.Parse(line.Substring(5));
                newInstruction = new Instruction(2, @value);
            }

            instructions.Add(newInstruction);
        }

        for (int i = 1; i <= 240; i++)
        {
            if ((i - 1) % 40 >= register - 1 && (i - 1) % 40 <= register + 1)
            {
                pixels[i - 1] = '█';
            }
            else
            {
                pixels[i - 1] = ' ';
            }

            Instruction currentInstruction = instructions[0];
            currentInstruction.Duration--;

            if (currentInstruction.Duration == 0)
            {
                register += currentInstruction.Value;
                instructions.RemoveAt(0);
            }
        }

        string text = "\n";

        for (int i = 0; i < 240; i++)
        {
            text += pixels[i];

            if ((i + 1) % 40 == 0)
            {
                text += "\n";
            }
        }

        return text.TrimEnd('\n');
    }
}