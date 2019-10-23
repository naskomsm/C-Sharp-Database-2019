namespace EntityFramework
{
    using System;
    using System.Data.SqlClient;

    public class SixthProblem
    {
        public void Run()
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
                    var transaction = connection.BeginTransaction();
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = $"DELETE FROM MinionsVillains WHERE VillainId = {villainId}";
                        var releasedMinions = command.ExecuteNonQuery();

                        command.CommandText = $"SELECT Name FROM Villains WHERE Id = {villainId}";
                        var getVillainName = (string)command.ExecuteScalar();

                        command.CommandText = $"DELETE FROM Villains WHERE Id = {villainId}";
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        Console.WriteLine("Transaction was succesfull!");

                        Console.WriteLine($"{getVillainName} was deleted.");
                        Console.WriteLine($"{releasedMinions} minions were relesed.");
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Transaction failed!");
                        Console.WriteLine(e.Message);

                        try
                        {
                            transaction.Rollback();
                        }

                        catch (Exception exRollback)
                        {
                            Console.WriteLine("Transaction rollback failed!");
                            Console.WriteLine(exRollback.Message);
                        }
                    }
                }
            }
        }
    }
}
