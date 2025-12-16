using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2024.Day12;

public class GardenPlot
{
    private Vector2 _position;
    public char PlantType { get; private set; }
    public List<GardenPlot> Connections { get; private set; } = new();
    public bool IsVisited { get; private set; }
    private readonly List<Vector2> _directionsChecked = new();

    public GardenPlot(Vector2 pos, char plantType)
    {
        _position = pos;
        PlantType = plantType;
    }

    public void AddConnection(GardenPlot gardenPlot)
    {
        Connections.Add(gardenPlot);
    }

    public RegionData GetRegionData()
    {
        RegionData regionData = new(1, 4 - Connections.Count, 0);
        IsVisited = true;

        if (Connections.Count != 4)
        {
            Vector2 checkDirection = Vector2.Right;
            for (int i = 0; i < 4; i++)
            {
                if (!IsDirectionsChecked(checkDirection) && Connections.All(c => c._position != _position + checkDirection))
                {
                    MoveInDirection(checkDirection.Rotate90DegreesClockwise(), checkDirection);
                    MoveInDirection(checkDirection.Rotate90DegreesCounterclockwise(), checkDirection);
                    regionData.SidesCount++;
                }

                checkDirection = checkDirection.Rotate90DegreesClockwise();
            }
        }

        foreach (GardenPlot gardenPlot in Connections)
        {
            if (!gardenPlot.IsVisited)
            {
                regionData += gardenPlot.GetRegionData();
            }
        }

        return regionData;
    }

    private void MoveInDirection(Vector2 moveDirection, Vector2 checkDirection)
    {
        if (Connections.All(c => c._position != _position + checkDirection))
        {
            _directionsChecked.Add(checkDirection);
            Connections.FirstOrDefault(c => c._position == _position + moveDirection)?.MoveInDirection(moveDirection, checkDirection);
        }
    }

    private bool IsDirectionsChecked(Vector2 direction)
    {
        return _directionsChecked.Any(d => d == direction);
    }

    public void Reset()
    {
        IsVisited = false;
        _directionsChecked.Clear();
    }
}

public struct RegionData
{
    public int Area;
    public int Perimeter;
    public int SidesCount;

    public RegionData(int area, int perimeter, int sidesCount)
    {
        Area = area;
        Perimeter = perimeter;
        SidesCount = sidesCount;
    }

    public static RegionData operator +(RegionData rd1, RegionData rd2)
    {
        return new(rd1.Area + rd2.Area, rd1.Perimeter + rd2.Perimeter, rd1.SidesCount + rd2.SidesCount);
    }

    public static RegionData operator -(RegionData rd1, RegionData rd2)
    {
        return new(rd1.Area - rd2.Area, rd1.Perimeter - rd2.Perimeter, rd1.SidesCount - rd2.SidesCount);
    }

    public override string ToString()
    {
        return $"Area : {Area}, Perimeter : {Perimeter}, Sides Count : {SidesCount}";
    }
}
