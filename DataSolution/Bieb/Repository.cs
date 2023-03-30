using System.Diagnostics;

namespace Bieb;

public class Repository
{
    public Book? GetBook(int id)
    {
        try
        {
            int result = int.Parse("een");
            return new Book { Id = id, Name = "Boek" };
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
            //return new Book { Id = 1, Name = "Boek" };
        }
        finally
        {
            Console.WriteLine("Close Database");
        }
        return null;
    }
}