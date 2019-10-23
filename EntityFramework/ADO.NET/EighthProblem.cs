namespace EntityFramework
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;

    public class EighthProblem
    {
        public void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                var minionsIds = Console.ReadLine()
                    .Split(" ")
                    .Select(int.Parse)
                    .ToList();

                var getAllMinionsCommand = new SqlCommand("SELECT * FROM Minions", connection);
                var reader = getAllMinionsCommand.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        var id = (int)reader["Id"];
                        var name = (string)reader["Name"];
                        var age = (int)reader["Age"];

                        if (minionsIds.Contains(id))
                        {
                            age += 1;
                            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                            name = textInfo.ToTitleCase(name.ToLower());
                        }

                        Console.WriteLine($"{name} {age}");
                    }
                }

                foreach (var id in minionsIds)
                {
                    //increment age
                    var updateMinionsAgeCommand = new SqlCommand($"UPDATE Minions SET Age += 1 WHERE Id = {id}", connection);
                    updateMinionsAgeCommand.ExecuteNonQuery();

                    //make title case
                    var updateMinionsCommand = new SqlCommand("UPDATE Minions SET Name = UPPER(LEFT(Name,1)) + LOWER(RIGHT(Name, LEN(Name) - 1))", connection);
                    updateMinionsCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
