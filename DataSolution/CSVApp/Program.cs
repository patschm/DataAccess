using CsvHelper;
using System.Globalization;

namespace CSVApp;

internal class Program
{
    static void Main(string[] args)
    {
        var stream = File.Create("somefile.csv");
        StreamWriter sw = new StreamWriter(stream);
        CsvWriter writer = new CsvWriter(sw, new CultureInfo("nl-NL"));
        //writer.WriteField("Veld1");
        //writer.WriteField("Veld2");
        writer.WriteRecord(new Person{ Name="Kees", Age=42 });
        writer.WriteRecord(new Person { Name = "Marie", Age = 32 });
        writer.Dispose();

    }
}