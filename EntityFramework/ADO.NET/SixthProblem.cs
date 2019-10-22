namespace EntityFramework_test
{
    using System;
    using System.Data.SqlClient;

    public class SixthProblem
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                var villainId = int.Parse(Console.ReadLine());

                var doesVillainExistCommand = new SqlCommand($@"SELECT COUNT(*) FROM Villains WHERE Id = {villainId}", connection);
                var doesVilainExist = (int)doesVillainExistCommand.ExecuteScalar();

                if (doesVilainExist == 0)
                {
                    Console.WriteLine("No such villain was found.");
                }

                else
                {
                    var deleteVillainConnectionWithMinionCommand = new SqlCommand($@"DELETE FROM MinionsVillains WHERE VillainId = {villainId}", connection);
                    var releasedMinionsCount = deleteVillainConnectionWithMinionCommand.ExecuteNonQuery();

                    var getVillainNameCommand = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = {villainId}", connection);
                    var getVillainName = (string)getVillainNameCommand.ExecuteScalar();

                    var deleteVillainCommand = new SqlCommand($@"DELETE FROM Villains WHERE Id = {villainId}", connection);
                    deleteVillainCommand.ExecuteNonQuery();

                    Console.WriteLine($"{getVillainName} was deleted.");
                    Console.WriteLine($"{releasedMinionsCount} minions were relesed.");
                }
            }
        }
    }
}
