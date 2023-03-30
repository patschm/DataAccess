namespace Scanner;

internal class Program
{
    static void Main(string[] args)
    {
        string dir = @"D:\AZ400";
        ReadContent(dir);
    }

    private static void ReadContent(string dir)
    {
        
        var start = new DirectoryInfo(dir);

        foreach (DirectoryInfo di in start.GetDirectories())
        {
            Console.WriteLine("+" + di.FullName);
            try
            {
                ReadContent(di.FullName);
            }
            catch { }
        }
        foreach (FileInfo fi in start.GetFiles())
        {
            Console.WriteLine("-" + fi.FullName);
        }
    }
}