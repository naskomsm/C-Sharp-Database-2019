namespace EntityFramework_test
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class SeventhProblem
    {
        public void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
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

                while (list.Any())
                {
                    Console.WriteLine(list[0]);
                    Console.WriteLine(list[list.Count - 1]);

                    list.RemoveAt(0);
                    list.RemoveAt(list.Count - 1);
                }
            }
        }
    }
}
