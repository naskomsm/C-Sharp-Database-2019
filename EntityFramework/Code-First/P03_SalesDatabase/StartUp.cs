namespace P03_SalesDatabase
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            var context = new SalesContext();

            context.Database.Migrate();

            using (context)
            {
                Console.WriteLine("Working!");
            }
        }
    }
}
