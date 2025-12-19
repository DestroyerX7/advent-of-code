using System.Collections.Generic;

namespace AdventOfCode.Year2022.Day12;

// Changed from Program.HeightMap to Solution.HeightMap in 2025 to fit structure
public class Height
{
    public int[] Coordinate;
    public int Elevation;
    public List<Height> Neighbors = new List<Height>();

    public double StartDistance;
    public double EndDistance;
    public double Cost => StartDistance + EndDistance;
    public Height? Parent; // Added ? in 2025 to fix terminal warning

    public Height(int[] coordinate, int elevation)
    {
        Coordinate = new int[] { coordinate[0], coordinate[1] };
        Elevation = elevation;
    }

    public void AddEdges()
    {
        // Added in 2025 to fix terminal warning
        if (Solution.HeightMap == null)
        {
            throw new System.Exception("HeightMap is null.");
        }

        //top
        if (Coordinate[1] != 0)
        {
            int[] newHeightCoor = new int[] { Coordinate[0], Coordinate[1] - 1 };

            if (Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]].Elevation <= Elevation + 1)
            {
                Height newHeight = Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]];
                Neighbors.Add(newHeight);
            }
        }

        //bottom
        if (Coordinate[1] != Solution.HeightMap.GetLength(0) - 1)
        {
            int[] newHeightCoor = new int[] { Coordinate[0], Coordinate[1] + 1 };

            if (Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]].Elevation <= Elevation + 1)
            {
                Height newHeight = Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]];
                Neighbors.Add(newHeight);
            }
        }

        //right
        if (Coordinate[0] != Solution.HeightMap.GetLength(1) - 1)
        {
            int[] newHeightCoor = new int[] { Coordinate[0] + 1, Coordinate[1] };

            if (Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]].Elevation <= Elevation + 1)
            {
                Height newHeight = Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]];
                Neighbors.Add(newHeight);
            }
        }

        //left
        if (Coordinate[0] != 0)
        {
            int[] newHeightCoor = new int[] { Coordinate[0] - 1, Coordinate[1] };

            if (Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]].Elevation <= Elevation + 1)
            {
                Height newHeight = Solution.HeightMap[newHeightCoor[1], newHeightCoor[0]];
                Neighbors.Add(newHeight);
            }
        }
    }
}
