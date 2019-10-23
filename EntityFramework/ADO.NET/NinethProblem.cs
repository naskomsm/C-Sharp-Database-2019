namespace EntityFramework_test
{
    using System;
    using System.Data.SqlClient;

    public class NinethProblem
    {
        public void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                var givenId = int.Parse(Console.ReadLine());
                var command = new SqlCommand($"EXEC usp_GetOlder {givenId}", connection);
                command.ExecuteNonQuery();

                var getMinionCommand = new SqlCommand($"SELECT * FROM Minions WHERE Id = {givenId}", connection);
                var reader = getMinionCommand.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        var name = reader["Name"];
                        var age = reader["Age"];

                        Console.WriteLine($"{name} - {age} years old");
                    }
                }
            }
        }
    }
}
