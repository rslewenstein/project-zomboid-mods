using System.ComponentModel;
using System.Dynamic;

class GambiarraAutoSave
{
    static void Main(string[] args)
    {
        string path = @"C:\Users\RLewenstein\Zomboid\Logs";
        Console.WriteLine(NameUser());
        var filename = GetFileName(path);

        if (!string.IsNullOrEmpty(path) || !string.IsNullOrEmpty(filename))
        {
            ReadFile(path, filename);
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

}

