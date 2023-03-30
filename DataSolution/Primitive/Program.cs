using Microsoft.Data.SqlClient;
using Primitive.Entities;
using System.Data;
using Primitive.ProductCatalogTableAdapters;

//using static Primitive.ProductCatalog;

namespace Primitive;

internal class Program
{
    static string connectionString = @"Server=.\sqlexpress;Database=ProductCatalog;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        //TestConnection();
        //foreach(ProductGroup pg in TestCommand("2;DELETE FROM Core.ProductGroups WHERE Id = 11;--"))
        //foreach (ProductGroup pg in TestCommand())
        //{
        //    Console.WriteLine($"{pg.Id} {pg.Name} {pg.Image} {pg.TimeStamp}");
        //    foreach (Product p in TestCommandProducts(pg.Id))
        //    {
        //        Console.WriteLine($"\t{p.Name}");
        //    }
        //}
        //TestDataSet();
        TestGeneratedDataSet();


    }

    private static void TestGeneratedDataSet()
    {
        var connection = new System.Data.SqlClient.SqlConnection();
        connection.ConnectionString = connectionString;

        var adp = new ProductGroupsTableAdapter();
        adp.Connection = connection;

        ProductCatalog catalog = new ProductCatalog();
        adp.Fill(catalog.ProductGroups);
       foreach(ProductCatalog.ProductGroupsRow row in catalog.ProductGroups.Rows)
        {
            //row.Name = "Kees";
            Console.WriteLine($"{row.Name}");
            foreach (ProductCatalog.ProductsRow row2 in row.GetProductsRows())
            { 
                Console.WriteLine($"\t{row2.Name}");
            }
        }
    }

    private static void TestDataSet()
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;
        connection.Open();

        DataSet ds = new DataSet();
        DataTable t1 = new DataTable();
        t1.TableName = "ProductGroups";
        t1.Columns.Add(new DataColumn("Id", typeof(long)));
        t1.Columns.Add(new DataColumn("Name", typeof(string)));
        t1.Columns.Add(new DataColumn("Image", typeof(string)));
        t1.Columns.Add(new DataColumn("TimeStamp", typeof(byte[])));
        ds.Tables.Add(t1);

        SqlDataAdapter adp = new SqlDataAdapter();
        adp.SelectCommand = CreateCommand(connection);
        adp.Fill(t1);
        //adp.Update(t1);

        foreach (DataRow dr in ds.Tables[0].AsEnumerable())
        {
            Console.WriteLine($"{dr[1]}");
        }
    }
    static SqlCommand CreateCommand(SqlConnection connection)
    {
      
        SqlCommand command = new SqlCommand();
        command.Connection = connection;

        command.CommandText = "SELECT * FROM Core.ProductGroups;SELECT * FROm Products";
        return command;
    }
    static void TestConnection()
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;
        connection.Open();
        Console.WriteLine(connection.State);
        connection.Dispose();
    }
    static IEnumerable<ProductGroup> TestCommand(string? sid = null)
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
        SqlDataReader rdr = command.ExecuteReader(CommandBehavior.SequentialAccess);
        while (rdr.Read())
        {
            var pg = new ProductGroup
            {
                Id = rdr.GetInt64(0),
                Name = rdr.GetString(1),
                Image = rdr.GetString(2),
                TimeStamp = (byte[])rdr[3]
            };

            yield return pg;

        }

        connection.Dispose();
    }
    static IEnumerable<Product> TestCommandProducts(long id)
    {
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = connectionString;

        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Core.Products WHERE ProductGroupId = @Id";
        // To prevent Sql Injections
        command.Parameters.Add(new SqlParameter("@Id", id));

        command.CommandType = CommandType.Text;
        SqlDataReader rdr = command.ExecuteReader(CommandBehavior.SequentialAccess);
        while (rdr.Read())
        {
            var pg = new Product
            {
                Id = rdr.GetInt64(0),
                Name = rdr.GetString(1),
                BrandId = rdr.GetInt64(2),
                ProductGroupId = rdr.GetInt64(3),
                Image = rdr.GetString(4),
                TimeStamp = (byte[])rdr[5]
            };
            yield return pg;

        }

        connection.Dispose();
    }

}