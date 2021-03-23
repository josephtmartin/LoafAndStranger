using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoafAndStranger.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace LoafAndStranger.DataAccess
{
    public class StrangerRepository
    {
        readonly string ConnectionString;

        public StrangerRepository(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("LoafAndStranger");
        }

        public IEnumerable<Stranger> GetAll()
        {
            var sql = @"SELECT *
                        FROM Strangers s
	                        left join Tops t
		                        on s.TopId = t.Id
	                        left join Loaves l
		                        on s.LoafId = l.Id";

            using var db = new SqlConnection(ConnectionString);

            var strangers = db.Query<Stranger, Top, Loaf, Stranger>(sql,
                (stranger, top, loaf) =>
                {
                    stranger.Loaf = loaf;
                    stranger.Top = top;

                    return stranger;
                }, splitOn: "Id");

            return strangers;
        }
    }
}
