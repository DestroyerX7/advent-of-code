using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2022.Day09;

public class Rope
{
    public List<int[]> VisitedPositions = new List<int[]>() { new int[] { 0, 0 } };
    private List<int[]> _knots = new List<int[]>();
    private int _ropeLength;

    public Rope()
    {
        _ropeLength = 2;

        for (int i = 0; i < _ropeLength; i++)
        {
            _knots.Add(new int[] { 0, 0 });
        }
    }

    public Rope(int ropeLength)
    {
        _ropeLength = ropeLength;

        for (int i = 0; i < _ropeLength; i++)
        {
            _knots.Add(new int[] { 0, 0 });
        }
    }

    public void MoveHead(char moveDirection, int moveNum)
    {
        Dictionary<char, int[]> movements = new Dictionary<char, int[]>()
            {
                { 'U',  new int[] { 0, 1 } },
                { 'D',  new int[] { 0, -1 } },
                { 'L',  new int[] { -1, 0 } },
                { 'R',  new int[] { 1, 0 } }
            };

        int[] moveDir = movements[moveDirection];

        for (int i = 0; i < moveNum; i++)
        {
            _knots[0][0] += moveDir[0];
            _knots[0][1] += moveDir[1];
            MoveTails(moveDir);
        }
    }

    private void MoveTails(int[] moveDir)
    {
        for (int i = 1; i < _knots.Count; i++)
        {
            int xDistance = _knots[i - 1][0] - _knots[i][0];
            int yDistance = _knots[i - 1][1] - _knots[i][1];

            if (Math.Abs(xDistance) <= 1 && Math.Abs(yDistance) <= 1)
            {
                return;
            }
            else
            {
                if (xDistance >= 1)
                {
                    _knots[i][0]++;
                }

                if (xDistance <= -1)
                {
                    _knots[i][0]--;
                }

                if (yDistance >= 1)
                {
                    _knots[i][1]++;
                }

                if (yDistance <= -1)
                {
                    _knots[i][1]--;
                }
            }

            if (i == _knots.Count - 1)
            {
                int[] newArray = new int[] { _knots[i][0], _knots[i][1] };
                bool addArray = true;

                foreach (int[] array in VisitedPositions)
                {
                    if (array[0] == newArray[0] && array[1] == newArray[1])
                    {
                        addArray = false;
                    }
                }

                if (addArray)
                {
                    VisitedPositions.Add(newArray);
                }
            }
        }
    }
}
