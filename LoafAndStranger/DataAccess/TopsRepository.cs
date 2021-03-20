using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoafAndStranger.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LoafAndStranger.DataAccess
{
    public class TopsRepository
    {
        const string ConnectionString = "Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;";

        public IEnumerable<Top> GetAll()
        {
            using var db = new SqlConnection(ConnectionString);

            var sql = @"SELECT *
                        FROM Tops";

            var tops = db.Query<Top>(sql);

            return tops;
        }

        public Top Add(int numberOfSeats)
        {
            using var db = new SqlConnection(ConnectionString);

            var sql = @"INSERT INTO [Tops] ([NumberOfSeats])
                        OUTPUT inserted.*
                        VALUES (@numberOfSeats)";

            var top = db.QuerySingle<Top>(sql, new {numberOfSeats});

            return top;
        }
    }
}
