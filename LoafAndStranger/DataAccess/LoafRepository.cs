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
            using var db = new SqlConnection(ConnectionString);

            var sql = @"SELECT * 
                        FROM Loaves";

            //when i execute a query what kind of thing should i return the results to
            //have to pass what the query is in the ()
            var results = db.Query<Loaf>(sql).ToList();
            return results;
        }

        public void Add(Loaf loaf)
        {
            var sql = @"INSERT INTO [dbo].[Loaves] ([Size], [Type], [WeightInOunces], [Price], [Sliced])
                                    OUTPUT inserted.Id
                                    VALUES(@Size, @Type, @WeightInOunces, @Price, @Sliced)";

            using var db = new SqlConnection(ConnectionString);

            //passing the whole loaf class(with all it's properties) as the substitution values for the query
            var id = db.ExecuteScalar<int>(sql, loaf);

            loaf.Id = id;
        }

        public Loaf Get(int id)
        {
            var sql = @"SELECT *
                        FROM Loaves
                        WHERE Id = @id";

            using var db = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");

            var loaf = db.QueryFirstOrDefault<Loaf>(sql, new { id = id });

            return loaf;
        }

        public void Remove(int id)
        {
            using var db = new SqlConnection(ConnectionString);

            var sql = @"DELETE
                                FROM Loaves
                                WHERE Id = @id";

            db.Execute(sql, new { id });
        }
    }
}
