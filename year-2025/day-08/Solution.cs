using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Year2025.Day08;

[PuzzleName("Playground")]
public class Solution : Solver
{
    public override object SolvePartOne(string[] input)
    {
        List<Vector3> positions = [];

        foreach (string line in input)
        {
            string[] axes = line.Split(',');

            Vector3 pos = new()
            {
                X = int.Parse(axes[0]),
                Y = int.Parse(axes[1]),
                Z = int.Parse(axes[2]),
            };

            positions.Add(pos);
        }

        PriorityQueue<Connection, double> priorityQueue = new();

        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                Connection connection = new()
                {
                    PosOne = positions[i],
                    PosTwo = positions[j],
                };

                priorityQueue.Enqueue(connection, connection.Magnitude);
            }
        }

        List<Circuit> circuits = [];

        for (int i = 0; i < 1000; i++)
        {
            Connection connection = priorityQueue.Dequeue();

            Circuit? circuitOne = circuits.FirstOrDefault(c => c.Positions.Contains(connection.PosOne));
            Circuit? circuitTwo = circuits.FirstOrDefault(c => c.Positions.Contains(connection.PosTwo));

            if (circuitOne != null && circuitTwo != null)
            {
                if (circuitOne == circuitTwo)
                {
                    continue;
                }

                // Combine
                foreach (Vector3 pos in circuitTwo.Positions)
                {
                    circuitOne.Positions.Add(pos);
                }

                circuits.Remove(circuitTwo);
            }
            else if (circuitOne != null)
            {
                // Add to circuitOne
                circuitOne.Positions.Add(connection.PosTwo);
            }
            else if (circuitTwo != null)
            {
                // Add to circuitTwo
                circuitTwo.Positions.Add(connection.PosOne);
            }
            else
            {
                // Create new circuit
                Circuit circuit = new();
                circuit.Positions.Add(connection.PosOne);
                circuit.Positions.Add(connection.PosTwo);
                circuits.Add(circuit);
            }
        }

        return circuits.OrderByDescending(c => c.Positions.Count).Take(3).Select(c => c.Positions.Count).Aggregate((a, b) => a * b);
    }

    public override object SolvePartTwo(string[] input)
    {
        List<Vector3> positions = [];

        foreach (string line in input)
        {
            string[] axes = line.Split(',');

            Vector3 pos = new()
            {
                X = int.Parse(axes[0]),
                Y = int.Parse(axes[1]),
                Z = int.Parse(axes[2]),
            };

            positions.Add(pos);
        }

        PriorityQueue<Connection, double> priorityQueue = new();

        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                Connection connection = new()
                {
                    PosOne = positions[i],
                    PosTwo = positions[j],
                };

                priorityQueue.Enqueue(connection, connection.Magnitude);
            }
        }

        HashSet<Vector3> connectedPositions = [];
        Vector3[] lastTwoAdded = new Vector3[2];

        while (priorityQueue.Count > 0)
        {
            Connection connection = priorityQueue.Dequeue();

            if (connectedPositions.Contains(connection.PosOne) && connectedPositions.Contains(connection.PosTwo))
            {
                continue;
            }

            connectedPositions.Add(connection.PosOne);
            connectedPositions.Add(connection.PosTwo);

            lastTwoAdded[0] = connection.PosOne;
            lastTwoAdded[1] = connection.PosTwo;
        }

        return lastTwoAdded[0].X * lastTwoAdded[1].X;
    }
}

public struct Connection
{
    public Vector3 PosOne;
    public Vector3 PosTwo;

    public readonly double Magnitude => (PosOne - PosTwo).Magnitude;

    public readonly override string ToString()
    {
        return $"{PosOne} <===> {PosTwo}";
    }
}
