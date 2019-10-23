namespace EntityFramework
{
    using System;
    using System.Data.SqlClient;

    public class ThirdProblem
    {
        public void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                Console.Write("Villain ID: ");
                var givenId = int.Parse(Console.ReadLine());

                var command = new SqlCommand(@$"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum, 
                    m.Name, m.Age, v.Name AS villainName
                    FROM MinionsVillains AS mv
                    JOIN Minions AS m ON mv.MinionId = m.Id
                    JOIN Villains AS v ON v.Id = mv.VillainId
                    WHERE mv.VillainId = {givenId}
                    ORDER BY m.Name", connection);  

                var reader = command.ExecuteReader();

                using (reader)
                {
                    if (reader.Read())
                    {
                        var villainName = reader["villainName"];
                        Console.WriteLine($"Villain name -> {villainName}");

                        while (reader.Read())
                        {
                            var rowNum = reader["RowNum"];
                            var name = (string)reader["Name"];
                            var age = (int)reader["Age"];

                            Console.WriteLine($"Row -> {rowNum}, Name -> {name}, Age -> {age}");
                        }
                    }

                    else
                    {
                        Console.WriteLine($"No villain with ID {givenId} exists in the database.");
                    }
                }
            }
        }
    }
}
