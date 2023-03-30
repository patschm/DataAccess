using Bieb;

namespace BiebClient;

internal class Program
{
    static void Main(string[] args)
    {
        Repository repository = new Repository();
        try
        {
            var book = repository.GetBook(0);
            Console.WriteLine(book?.Name);
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine("Hey joh? Geen 0 aub");
            Console.WriteLine(e.StackTrace);
        }
        catch (FormatException fe)
        {
            Console.WriteLine(fe.Message);
            Console.WriteLine(fe.StackTrace);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}