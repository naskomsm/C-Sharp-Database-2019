namespace EntityFramework
{
    using System;
    using System.Data.SqlClient;

    public class SecondProblem
    {
        public void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();
            using (connection)
            {
                var command = new SqlCommand("SELECT v.Name, COUNT(mv.MinionId) AS MinionsCount " +
                    "FROM Villains AS v " +
                    "JOIN MinionsVillains AS mv ON v.Id = mv.VillainId " +
                    "GROUP BY v.Name " +
                    "HAVING COUNT(mv.MinionId) > 3 " +
                    "ORDER BY COUNT(mv.MinionId) DESC", connection);

                var reader = command.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        var name = (string)reader["Name"];
                        var minions = (int)reader["MinionsCount"];
                        Console.WriteLine($"{name} - {minions}");
                    }
                }
            }
        }
    }
}
