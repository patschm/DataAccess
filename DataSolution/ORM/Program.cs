using Microsoft.EntityFrameworkCore;
using ORM.Models;
using System.Threading.Channels;

namespace ORM;

internal class Program
{
    static string connectionString = @"Server=.\sqlexpress;Database=ProductCatalog;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
    
    static void Main(string[] args)
    {
        //TestSelect();
        TestInsert();
    }

    private static void TestInsert()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductCatalogContext>();
        optionsBuilder.UseSqlServer(connectionString, opts => {
            opts.MaxBatchSize(64);
        });
        //optionsBuilder.LogTo(s=>Console.WriteLine(s));

        ProductCatalogContext context = new ProductCatalogContext(optionsBuilder.Options);

        var pg = new ProductGroup
        {
            Name = "Test",
            Image = "test.jpg"
        };

        context.ProductGroups.Add(pg);
        context.SaveChanges();
    }

    private static void TestSelect()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductCatalogContext>();   
        optionsBuilder.UseSqlServer(connectionString);  
        //optionsBuilder.LogTo(s=>Console.WriteLine(s));

        ProductCatalogContext context = new ProductCatalogContext(optionsBuilder.Options);

        var pgs = context.ProductGroups.Include(pg=>pg.Products).AsNoTracking().ToList();
        var query = context.ProductGroups.Include(pg => pg.Products)
            .Where(pg=>pg.Name.StartsWith("M"))
            .Select(pg=> new { pg.Id, pg.Name });

        var query2 = from pg in context.ProductGroups.Include(pg => pg.Products)
                     where pg.Name.StartsWith("M") 
                     select new { GroupId = pg.Id, GroupName = pg.Name, pg.Products };


        //foreach(ProductGroup group in context.ProductGroups.Include(pg=>pg.Products)) 
        foreach (var group in query2)
        {
            Console.WriteLine($"[{group.GroupId}] {group.GroupName} ({group})");
            foreach (Product product in group.Products)
            {
                Console.WriteLine($"\t{product.Name}");
            }
        }

    }
}