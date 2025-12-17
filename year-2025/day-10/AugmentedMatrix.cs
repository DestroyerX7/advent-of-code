using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2025.Day10;

public class AugmentedMatrix
{
    private readonly double[][] _augmentedMatrix;
    private readonly int _rows;
    private readonly int _cols;

    public AugmentedMatrix(double[][] augmentedMatrix)
    {
        if (augmentedMatrix[0].Length < 2)
        {
            throw new Exception("Invalid augmented matrix dimensions.");
        }

        _augmentedMatrix = augmentedMatrix;
        _rows = augmentedMatrix.Length;
        _cols = augmentedMatrix[0].Length;
    }

    public void RowReduce()
    {
        for (int pivotRow = 0; pivotRow < _rows; pivotRow++)
        {
            int pivotIndex = -1;

            for (int x = pivotRow; x < _cols - 1 && pivotIndex == -1; x++)
            {
                for (int y = pivotRow; y < _rows && pivotIndex == -1; y++)
                {
                    if (_augmentedMatrix[y][x] != 0)
                    {
                        double inverse = 1 / _augmentedMatrix[y][x];

                        MultiplyRow(y, inverse);
                        SwapRows(y, pivotRow);

                        pivotIndex = x;
                    }
                }
            }

            if (pivotIndex != -1)
            {
                for (int y = 0; y < _rows; y++)
                {
                    if (y == pivotRow)
                    {
                        continue;
                    }

                    double val = _augmentedMatrix[y][pivotIndex];

                    for (int x = pivotIndex; x < _cols; x++)
                    {
                        _augmentedMatrix[y][x] -= val * _augmentedMatrix[pivotRow][x];
                    }
                }
            }
        }
    }

    private void SwapRows(int indexOne, int indexTwo)
    {
        (_augmentedMatrix[indexTwo], _augmentedMatrix[indexOne]) = (_augmentedMatrix[indexOne], _augmentedMatrix[indexTwo]);
    }

    private void MultiplyRow(int y, double multiplier)
    {
        for (int x = 0; x < _cols; x++)
        {
            _augmentedMatrix[y][x] = _augmentedMatrix[y][x] * multiplier;
        }
    }

    public List<string> GetEquations()
    {
        List<string> equations = [];

        for (int y = _rows - 1; y >= 0; y--)
        {
            bool foundPivot = false;

            string equation = "";

            for (int x = 0; x < _cols - 1; x++)
            {
                if (_augmentedMatrix[y][x] != 0 && !foundPivot)
                {
                    equation += $"{_augmentedMatrix[y][_cols - 1]} ";
                    foundPivot = true;
                }
                else if (_augmentedMatrix[y][x] != 0)
                {
                    equation += _augmentedMatrix[y][x] > 0 ? "-" : "+";
                    equation += $"{Math.Abs(_augmentedMatrix[y][x])}*x_{x} ";
                }
            }

            if (!string.IsNullOrEmpty(equation))
            {
                equations.Add(equation.Trim());
            }
        }

        return equations;
    }

    public bool IsConsistent()
    {
        for (int y = _augmentedMatrix.Length - 1; y >= 0; y--)
        {
            if (_augmentedMatrix[y][_cols - 1] != 0 && _augmentedMatrix[y].Take(_cols - 1).All(n => n == 0))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        string output = "";

        for (int y = 0; y < _rows; y++)
        {
            output += "| ";

            for (int x = 0; x < _cols; x++)
            {
                output += _augmentedMatrix[y][x] + " ";

                if (x == _cols - 2)
                {
                    output += "| ";
                }
            }

            output += "|\n";
        }

        return output.Trim('\n');
    }
}