using System.IO.Compression;

class GambiarraAutoSave
{
    static void Main(string[] args)
    {
        string path = @"C:\Users\RLewenstein\Zomboid\Logs";
        Console.WriteLine(NameUser());
        var filename = GetFileName(path);

        if (!string.IsNullOrEmpty(path) || !string.IsNullOrEmpty(filename))
        {
            while (true)
            {
                ReadFile(path, filename);
                CopyDirectory("C:\\Users\\RLewenstein\\Zomboid\\Saves", @"C:\\Users\\RLewenstein\\Zomboid\\BKP\\");
                Thread.Sleep(300000); // 5 minutos
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

    private static void ReadFile(string path, string? filename)
    {
        File.ReadLines(Path.Combine(path, filename))
            .Where(line => line.Contains("[AUTOSAVE]"))
            .ToList()
            .ForEach(Console.WriteLine);
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
                // cria zip apenas uma vez
                string zipPath = destinationDir + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".zip";
                ZipFile.CreateFromDirectory(destinationDir, zipPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying directory: {ex.Message}");
        }
    }

}

