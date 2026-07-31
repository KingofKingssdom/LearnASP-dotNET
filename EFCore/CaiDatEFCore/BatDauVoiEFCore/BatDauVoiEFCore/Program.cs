using BatDauVoiEFCore.Models;

namespace BatDauVoiEFCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new BloggingContext();
            // Note: This sample requires the database to be created before running.
            Console.WriteLine($"Database path: {db.DbPath}.");
            // Create
            Console.WriteLine("Inserting a new blog");
            db.Add(new Blog { Url = "http://blogs.msdn.com/addbogss" });
            db.SaveChangesAsync();
            // Read
            Console.WriteLine("Querying for a blog");
            var blog = db.Blogs
                .OrderBy(b => b.BlogId)
                .First();



        }
    }
}
