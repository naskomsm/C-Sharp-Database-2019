namespace P03_FootballBetting.Client
{
    using System;
    using P03_FootballBetting.Data;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            var context = new FootballBettingContext();

            context.Database.Migrate();

            using (context)
            {
                Console.WriteLine("Working!!!");
            }
        }
    }
}
