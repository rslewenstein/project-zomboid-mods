﻿using System.IO.Compression;

class GambiarraAutoSave
{
    static void Main(string[] args)
    {
        int lastPosition = 0;
        string path = @"C:\Users\" + NameUser() + @"\Zomboid\Logs";
        var filename = GetFileName(path);

        if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(filename))
        {
            while (true)
            {
                var numLines = ReadFile(path, filename);
                if (numLines > lastPosition)
                {
                    CopyDirectory("C:\\Users\\" + NameUser() + "\\Zomboid\\Saves", @"C:\\Users\\" + NameUser() + "\\Zomboid\\BKP\\");
                    lastPosition = numLines;
                }
                Thread.Sleep(1000); //300000 = 5 minutos
            }
        }  
    }

    private static string NameUser()
    {
        return Environment.UserName;
    }

    private static string? GetFileName(string path)
    {
        try
        {
            if (!Directory.Exists(path))
                return null;

            return Directory.EnumerateFiles(path, "*DebugLog*.txt")
                    .Select(Path.GetFileName)
                    .FirstOrDefault();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    private static int ReadFile(string path, string filename)
    {
        string fullPath = Path.Combine(path, filename);
        int count = 0;

        using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fs);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Contains("[AUTOSAVE]"))
            {
                Console.WriteLine(line);
                count++;
            }
        }

        return count;
    }

    private static void CopyDirectory(string sourceDir, string destinationDir)
    {
        try
        {
            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine($"Source directory '{sourceDir}' does not exist.");
                return;
            }

            Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string zipPath = destinationDir + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
                ZipFile.CreateFromDirectory(sourceDir, zipPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying directory: {ex.Message}");
        }
    }

}