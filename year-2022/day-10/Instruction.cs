namespace AdventOfCode.Year2022.Day10;

public class Instruction
{
    public int Duration;
    public int Value;

    public Instruction(int duration, int @value)
    {
        Duration = duration;
        Value = @value;
    }
}
