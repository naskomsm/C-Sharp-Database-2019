namespace EntityFramework_test
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;

    public class Program
    {
        public static void Main()
        {
            //7

            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                var leftIndex = 0;
                var rightIndex = 0;

                var getRightIndexCommand = new SqlCommand("SELECT COUNT(*) FROM Minions", connection);
                rightIndex = (int)getRightIndexCommand.ExecuteScalar() - 1;

                var getAllMinionsCommand = new SqlCommand("SELECT Name FROM Minions", connection);
                var reader = getAllMinionsCommand.ExecuteReader();

                var list = new List<string>();

                using (reader)
                {
                    while (reader.Read())
                    {
                        list.Add((string)reader["Name"]);
                    }
                }

                // algorithm to print the names :D
            }
        }
    }
}
