﻿using System.IO.Compression;
using System.Text.Json;

class GambiarraAutoSave
{
    static void Main(string[] args)
    {
        try
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
                        CopyDirectory(GetSavePath(), GetBackupPath());
                        lastPosition = numLines;
                        Console.WriteLine("Número de registros de AUTOSAVE: " + numLines);
                        Console.WriteLine("Backup realizado com sucesso!");
                        DeleteOldBackups();
                    }
                    Thread.Sleep(GetThreadSleepTime());
                }
            }    
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }       
    }

    private static string GetLogPath()
    {
        try
        {
            string json = File.ReadAllText(
            Path.Combine(AppContext.BaseDirectory, "configs.json"));
            var config = JsonSerializer.Deserialize<Config>(json);
            return config?.LogPath?.Replace("%USERNAME%", NameUser()) ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return string.Empty;
        }
    }

    private static string GetSavePath()
    {
        try
        {
            string json = File.ReadAllText("configs.json");
            var config = JsonSerializer.Deserialize<Config>(json);
            return config?.SavePath?.Replace("%USERNAME%", NameUser()) ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return string.Empty;
        }
    }

    private static string GetBackupPath()
    {
        try
        {
            string json = File.ReadAllText("configs.json");
            var config = JsonSerializer.Deserialize<Config>(json);
            return config?.BackupPath?.Replace("%USERNAME%", NameUser()) ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return string.Empty;
        }
    }

    private static int GetThreadSleepTime()
    {
        try
        {
            string json = File.ReadAllText("configs.json");
            var config = JsonSerializer.Deserialize<Config>(json);
            return config?.ThreadSleepTime ?? 10000;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading configuration: {ex.Message}");
            return 10000;
        }
    }

    private static void DeleteOldBackups()
    {
        try
        {
            string backupPath = GetBackupPath();
            if (!Directory.Exists(backupPath))
                return;

            var files = Directory.GetFiles(backupPath, "*.zip")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();

            string json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "configs.json"));
            var config = JsonSerializer.Deserialize<Config>(json);

            int maxBackups = config?.MaxBackups ?? 20;

            for (int i = maxBackups; i < files.Count; i++)
            {
                Console.WriteLine($"Deleted old backup: {files[i].FullName}");
                files[i].Delete();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting old backups: {ex.Message}");
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
    public string? LogPath { get; }
    public string? SavePath { get; }
    public string? BackupPath { get; }
    public int ThreadSleepTime { get; }
    public int MaxBackups { get; }

    public Config(string? logPath, string? savePath, string? backupPath,
                  int threadSleepTime, int maxBackups)
    {
        LogPath = logPath;
        SavePath = savePath;
        BackupPath = backupPath;
        ThreadSleepTime = threadSleepTime;
        MaxBackups = maxBackups;
    }
}