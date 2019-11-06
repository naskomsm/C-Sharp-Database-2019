namespace P01_StudentSystem.Client
{
    using System;
    using P01_StudentSystem.Data;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            var context = new StudentSystemContext();

            context.Database.Migrate();

            using (context)
            {
                Console.WriteLine("Working!");
            }
        }
    }
}
