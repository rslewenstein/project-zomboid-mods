﻿using System.IO.Compression;

class GambiarraAutoSave
{
    static void Main(string[] args)
    {
        int lastPosition = 0;
        string logPath = GetLogPath();
        var filename = GetFileName(logPath);

        if (!string.IsNullOrEmpty(logPath) && !string.IsNullOrEmpty(filename))
        {
            while (true)
            {
                var numLines = ReadFile(logPath, filename);
                if (numLines > lastPosition)
                {
                    CopyDirectory("C:\\Users\\" + NameUser() + "\\Zomboid\\Saves", @"C:\\Users\\" + NameUser() + "\\Zomboid\\BKP\\");
                    lastPosition = numLines;
                    Console.WriteLine("Número de registros de AUTOSAVE: " + numLines);
                    Console.WriteLine("Backup realizado com sucesso!");
                }
                Thread.Sleep(1000); //300000 = 5 minutos
            }
        }  
    }

    private static string GetLogPath()
    {
        try
        {
            string json = File.ReadAllText("configs.json");
            var config = System.Text.Json.JsonSerializer.Deserialize<Config>(json);
            return config?.LogPath?.Replace("%USERNAME%", NameUser()) ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return string.Empty;
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

class Config
{
    public string? LogPath { get; set; }
}