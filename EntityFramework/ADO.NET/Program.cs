namespace EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class Program
    {
        public static void Main()
        {
            var problem = new FirstProblem();
            problem.Run();

            var secondProblem = new SecondProblem();
            secondProblem.Run();

            //etc....
        }
    }
}
