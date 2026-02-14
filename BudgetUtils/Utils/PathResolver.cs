using System;
using System.IO;

public class PathResolver
{
    public static string ResolveInputPath(string inputPath)
    {
        if (Path.IsPathRooted(inputPath))
        {
            // Absolute path
            return inputPath;
        }
        else
        {
            // Relative path
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, inputPath));
        }
    }

    public static string GetFileNameFromPath(string filePath)
    {
        return Path.GetFileName(filePath);
    }   
}