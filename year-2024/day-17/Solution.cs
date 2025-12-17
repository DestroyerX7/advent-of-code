using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2024.Day17;

[PuzzleName("Chronospatial Computer")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        int[] registers = input.Take(3).Select(l => int.Parse(l[(l.IndexOf(':') + 2)..])).ToArray();

        Dictionary<int, int> operandToComboOperand = new()
        {
            { 0, 0 },
            { 1, 1 },
            { 2, 2 },
            { 3, 3 },
            { 4, registers[0] },
            { 5, registers[1] },
            { 6, registers[2] },
        };

        string instructionsString = input[4][(input[4].IndexOf(':') + 2)..];
        short[] instructions = instructionsString.Split(',').Select(short.Parse).ToArray();

        string output = "";

        for (int i = 0; i < instructions.Length; i += 2)
        {
            int opcode = instructions[i];
            int operand = instructions[i + 1];
            int comboOperand = operandToComboOperand[operand];

            if (opcode == 0)
            {
                registers[0] /= (int)Math.Pow(2, comboOperand);
                operandToComboOperand[4] = registers[0];
            }
            else if (opcode == 1)
            {
                registers[1] ^= operand;
                operandToComboOperand[5] = registers[1];
            }
            else if (opcode == 2)
            {
                registers[1] = comboOperand % 8;
                operandToComboOperand[5] = registers[1];
            }
            else if (opcode == 3 && registers[0] != 0)
            {
                i = operand - 2;
            }
            else if (opcode == 4)
            {
                registers[1] ^= registers[2];
                operandToComboOperand[5] = registers[1];
            }
            else if (opcode == 5)
            {
                output += comboOperand % 8 + ",";
            }
            else if (opcode == 6)
            {
                registers[1] = registers[0] / (int)Math.Pow(2, comboOperand);
                operandToComboOperand[5] = registers[1];
            }
            else
            {
                registers[2] = registers[0] / (int)Math.Pow(2, comboOperand);
                operandToComboOperand[6] = registers[2];
            }
        }

        output = output[..(output.Length - 1)];

        return output;
    }

    public override object SolvePartTwo(string[] input)
    {
        return "Not Solved Yet";

        // Dictionary<int, int> operandToComboOperand = new()
        // {
        //     { 0, 0 },
        //     { 1, 1 },
        //     { 2, 2 },
        //     { 3, 3 },
        //     { 4, 0 },
        //     { 5, 0 },
        //     { 6, 0 },
        // };

        // string instructionsString = input[4][(input[4].IndexOf(':') + 2)..];
        // short[] instructions = instructionsString.Split(',').Select(short.Parse).ToArray();

        // string output = "";

        // int a = 0;
        // while (output != instructionsString)
        // {
        //     int[] registers = { a, 0, 0 };

        //     operandToComboOperand[4] = registers[0];
        //     operandToComboOperand[5] = registers[1];
        //     operandToComboOperand[6] = registers[2];

        //     output = "";

        //     for (int i = 0; i < instructions.Length; i += 2)
        //     {
        //         int opcode = instructions[i];
        //         int operand = instructions[i + 1];
        //         int comboOperand = operandToComboOperand[operand];

        //         if (opcode == 0)
        //         {
        //             registers[0] /= (int)Math.Pow(2, comboOperand);
        //             operandToComboOperand[4] = registers[0];
        //         }
        //         else if (opcode == 1)
        //         {
        //             registers[1] ^= operand;
        //             operandToComboOperand[5] = registers[1];
        //         }
        //         else if (opcode == 2)
        //         {
        //             registers[1] = comboOperand % 8;
        //             operandToComboOperand[5] = registers[1];
        //         }
        //         else if (opcode == 3 && registers[0] != 0)
        //         {
        //             i = operand - 2;
        //         }
        //         else if (opcode == 4)
        //         {
        //             registers[1] ^= registers[2];
        //             operandToComboOperand[5] = registers[1];
        //         }
        //         else if (opcode == 5)
        //         {
        //             output += comboOperand % 8 + ",";

        //             int index = output.Length - 2;
        //             if (output.Length > instructionsString.Length || output[index] != instructionsString[index])
        //             {
        //                 break;
        //             }
        //         }
        //         else if (opcode == 6)
        //         {
        //             registers[1] = registers[0] / (int)Math.Pow(2, comboOperand);
        //             operandToComboOperand[5] = registers[1];
        //         }
        //         else
        //         {
        //             registers[2] = registers[0] / (int)Math.Pow(2, comboOperand);
        //             operandToComboOperand[6] = registers[2];
        //         }
        //     }

        //     output = output[..(output.Length - 1)];
        //     a++;
        // }

        // return a - 1;
    }
}