using System.ComponentModel;
using System.Dynamic;

class GambiarraAutoSave
{
    static void Main(string[] args)
    {
        Console.WriteLine(NameUser());
        var filename = GetFileName();
    }

    private static string NameUser()
    {
        return Environment.UserName;
    }

    private static string? GetFileName()
    {
        try
        {
            string path = @"C:\Users\RLewenstein\Zomboid\Logs";

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

}

