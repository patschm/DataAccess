using Microsoft.Data.SqlClient;
using Primitive.Entities;
using System.Data;
using System.Data.Common;
using System.Net.Security;

namespace Primitive;

internal class Program
{
    static string connectionString = @"Server=.\sqlexpress;Database=ProductCatalog;Trusted_Connection=True;TrustServerCertificate=True;";

    static void Main(string[] args)
    {
        //TestConnection();
        //foreach(ProductGroup pg in TestCommand("2;DELETE FROM Core.ProductGroups WHERE Id = 11;--"))
        foreach (ProductGroup pg in TestCommand("2"))
        {
            Console.WriteLine($"{pg.Id} {pg.Name} {pg.Image} {pg.TimeStamp}");
        }
    }

    static void TestConnection()
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;
        connection.Open();
        Console.WriteLine(connection.State);
        connection.Dispose();
    }
    static IEnumerable<ProductGroup> TestCommand(string? sid)
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;

        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        if (sid != null && long.TryParse(sid, out long id))
        {
            command.CommandText = "SELECT * FROM Core.ProductGroups WHERE Id = @Id";
            // To prevent Sql Injections
            command.Parameters.Add(new SqlParameter("@Id", id));
        }
        else
        {
            command.CommandText = "SELECT * FROM Core.ProductGroups";
        }
        command.CommandType = CommandType.Text;
        SqlDataReader rdr = command.ExecuteReader();
        while (rdr.Read())
        {
            var pg = new ProductGroup
            {
                Id = (uint)rdr.GetInt64(0),
                Name = rdr.GetString(1),
                Image = rdr.GetString(2),
                TimeStamp = (byte[])rdr[3]
            };
            yield return pg;

        }

        connection.Dispose();
    }
}