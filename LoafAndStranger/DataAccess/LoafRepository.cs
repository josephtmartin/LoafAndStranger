using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoafAndStranger.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LoafAndStranger.DataAccess
{
    public class LoafRepository
    {
        const string ConnectionString = "Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;";

        public List<Loaf> GetAll()
        {
            //create a connection
            using var db = new SqlConnection(ConnectionString);

            //telling the command what you want to do
            var sql = @"SELECT * 
                        FROM Loaves";

            //when i execute a query what kind of thing should i return the results to
            //have to pass what the query is in the ()
            var results = db.Query<Loaf>(sql).ToList();
            return results;
        }

        public void Add(Loaf loaf)
        {
            //created and opened a connection
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            //create command
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO [dbo].[Loaves] ([Size], [Type], [WeightInOunces], [Price], [Sliced])
                                    OUTPUT inserted.Id
                                    VALUES(@Size, @Type, @WeightInOunces, @Price, @Sliced)";

            command.Parameters.AddWithValue("Size", loaf.Size);
            command.Parameters.AddWithValue("Type", loaf.Type);
            command.Parameters.AddWithValue("WeightInOunces", loaf.WeightInOunces);
            command.Parameters.AddWithValue("Price", loaf.Price);
            command.Parameters.AddWithValue("Sliced", loaf.Sliced);

            var id = (int)command.ExecuteScalar();

            loaf.Id = id;
        }

        public Loaf Get(int id)
        {
            var sql = @"SELECT *
                        FROM Loaves
                        WHERE Id = @id";
            //create a connection
            using var connection = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");
            connection.Open();

            //create a command
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("id", id);

            //execute the command
            var reader = command.ExecuteReader();

            //only expect one thing so no need to loop since Read() returns a bool
            if (reader.Read())
            {
                var loaf = MapLoaf(reader);
                return loaf;
            }

            return null;
        }

        public void Remove(int id)
        {

            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE
                                FROM Loaves
                                WHERE Id = @id";

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }

        Loaf MapLoaf(SqlDataReader reader)
        {
            var id = (int)reader["Id"]; //explicit cast (throws exception)
            var size = (LoafSize)reader["Size"];
            var type = reader["Type"] as string; //implicit cast (returns a null if it cant change a thing into that type)
            var weightInOunces = (int)reader["weightInOunces"];
            var price = (decimal)reader["Price"];
            var sliced = (bool)reader["Sliced"];
            var createdDate = (DateTime)reader["createdDate"];

            //make a loaf
            var loaf = new Loaf
            {
                Id = id,
                Size = size,
                Type = type,
                WeightInOunces = weightInOunces,
                Price = price,
                Sliced = sliced,
            };
            return loaf;
        }
    }
}
