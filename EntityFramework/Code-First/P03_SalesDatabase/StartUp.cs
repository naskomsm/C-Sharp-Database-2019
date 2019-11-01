namespace P03_SalesDatabase
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var context = new SalesContext();

            context.Database.Migrate();

            using (context)
            {
                var result = context.Customers.FirstOrDefault();

                Console.WriteLine(result);
            }
        }
    }
}
