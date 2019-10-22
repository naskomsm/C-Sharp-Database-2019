namespace EntityFramework_test
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;

    public class FourthProblem
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                Console.Write("Minion: ");
                var minionArgs = Console.ReadLine()
                    .Split(" ")
                    .ToList();

                var minionName = minionArgs[0];
                var minionAge = int.Parse(minionArgs[1]);
                var minionTown = minionArgs[2];

                Console.Write("Villain: ");
                var villainName = Console.ReadLine();

                // town checker
                var checkTownExistanceCommand = new SqlCommand(@$"SELECT COUNT(*) FROM Towns WHERE Name = '{minionTown}'", connection);
                var checkTownExistance = (int)checkTownExistanceCommand.ExecuteScalar();

                if (checkTownExistance == 0) // town does not exist
                {
                    var addTownCommand = new SqlCommand(@$"INSERT INTO Towns (Name) VALUES ('{minionTown}')", connection);
                    addTownCommand.ExecuteNonQuery();
                    Console.WriteLine($"Town {minionTown} was added to the database.");
                }

                // villain checker
                var checkVillainExistanceCommand = new SqlCommand(@$"SELECT COUNT(*) FROM Villains WHERE Name = '{villainName}'", connection);
                var checkVillainExistance = (int)checkVillainExistanceCommand.ExecuteScalar();

                if (checkVillainExistance == 0) // villain does not exist
                {
                    var addVillainCommand = new SqlCommand(@$"INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('{villainName}', 4)", connection);
                    addVillainCommand.ExecuteNonQuery();
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                // add minion
                var getTownIdCommand = new SqlCommand(@$"SELECT Id FROM Towns WHERE Name = '{minionTown}'", connection);
                var townId = getTownIdCommand.ExecuteScalar();

                var addMinionCommand = new SqlCommand(@$"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', {minionAge}, {townId})", connection);
                addMinionCommand.ExecuteNonQuery();

                // make the minion servent of villain
                var getMinionIdCommand = new SqlCommand(@$"SELECT Id FROM Minions WHERE Name = '{minionName}'", connection);
                var minionId = getMinionIdCommand.ExecuteScalar();

                var getVillainIdCommand = new SqlCommand(@$"SELECT Id FROM Villains WHERE Name = '{villainName}'", connection);
                var villainId = getVillainIdCommand.ExecuteScalar();

                var addToMinionsVillainsCommand = new SqlCommand(@$"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({minionId},{villainId})", connection);

                try
                {
                    addToMinionsVillainsCommand.ExecuteNonQuery();
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot do this operation!");
                }
            }
        }
    }
}
