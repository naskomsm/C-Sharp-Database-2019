namespace P01_StudentSystem
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data;
    using System;

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
