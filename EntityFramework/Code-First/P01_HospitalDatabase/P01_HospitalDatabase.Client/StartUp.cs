namespace P01_HospitalDatabase.Client
{
    using System;
    using P01_HospitalDatabase.Data;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            var context = new HospitalContext();

            context.Database.Migrate();

            using (context)
            {
                Console.WriteLine("Working!");
            }
        }
    }
}
