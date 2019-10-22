namespace EntityFramework_test
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class FifthProblem
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=.;Database=MinionsDB;Integrated Security=true");

            connection.Open();

            using (connection)
            {
                var countryName = Console.ReadLine();

                var getCountryIdCommand = new SqlCommand(@$"SELECT Id FROM Countries WHERE Name = '{countryName}'", connection);
                var countryId = getCountryIdCommand.ExecuteScalar();

                var changeToUpperCaseCommand = new SqlCommand($@"UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = {countryId}", connection);
                changeToUpperCaseCommand.ExecuteNonQuery();

                var getAffectedTownsCountCommand = new SqlCommand($@"SELECT COUNT(*) FROM Towns WHERE CountryCode = {countryId}", connection);
                var affectedTownsCount = (int)getAffectedTownsCountCommand.ExecuteScalar();

                if (affectedTownsCount == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }

                else
                {
                    var getUpdatedTownsCommand = new SqlCommand($@"SELECT Name FROM Towns WHERE CountryCode = {countryId}", connection);

                    var reader = getUpdatedTownsCommand.ExecuteReader();

                    using (reader)
                    {
                        var list = new List<string>();
                        while (reader.Read())
                        {
                            list.Add((string)reader["Name"]);
                        }

                        Console.WriteLine(string.Join(" ", list));
                    }
                }
            }
        }
    }
}
