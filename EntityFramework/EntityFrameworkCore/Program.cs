namespace SoftUni
{
    using SoftUni.Data;
    using System;

    public class Program
    {
        public static void Main()
        {
            var context = new SoftUniContext();

            using (context)
            {
                var result = StartUp.RemoveTown(context);
                Console.WriteLine(result);
            }
        }
    }
}
