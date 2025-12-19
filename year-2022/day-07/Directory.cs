using System.Collections.Generic;

namespace AdventOfCode.Year2022.Day07;

public class Directory
{
    // Added ? to fix terminal warning
    public Directory? Parent;
    public List<Directory> Children = new List<Directory>();
    public int FileSize = 0;
    public int CurrentChild = 0;

    public void AddDirectory()
    {
        Directory newDirectory = new Directory();
        Children.Add(newDirectory);
        newDirectory.Parent = this;
    }

    public int GetFileSize()
    {
        int childrenFileSize = 0;

        foreach (Directory directory in Children)
        {
            childrenFileSize += directory.GetFileSize();
        }

        Solution.DirectoryFiles.Add(childrenFileSize + FileSize); // Changed from Program.DirectoryFiles to Solution.DirectoryFiles in 2025 to fit file structure
        return childrenFileSize + FileSize;
    }
}
