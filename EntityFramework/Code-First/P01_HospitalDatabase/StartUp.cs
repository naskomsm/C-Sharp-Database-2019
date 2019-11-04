namespace P01_HospitalDatabase
{
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data;
    using System;

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
