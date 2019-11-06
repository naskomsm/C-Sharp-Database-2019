namespace P03_SalesDatabase.Client
{
    using System;
    using P03_SalesDatabase.Data;
    using Microsoft.EntityFrameworkCore;

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
