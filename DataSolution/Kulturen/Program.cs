using System.Globalization;

namespace Kulturen;

internal class Program
{
    static void Main(string[] args)
    {
        //Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-NL");
        Console.WriteLine(Thread.CurrentThread.CurrentCulture);
        Console.WriteLine(Thread.CurrentThread.CurrentUICulture);
        DateTime nu = DateTime.Now;
        decimal prijs = 12.45M;

        //Console.WriteLine($"Vandaag ({nu}) kost dat {prijs}");
        Console.WriteLine(MyTexts.Show, nu, prijs.ToString(new CultureInfo("nl-NL")));
    }
}