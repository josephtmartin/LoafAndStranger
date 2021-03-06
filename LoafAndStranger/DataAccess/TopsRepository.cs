﻿using System;
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

            //var topsSql = @"SELECT *
            //                FROM Tops";
            //var strangersSql = @"SELECT *
            //                     FROM strangers
            //                     WHERE topid = @id";

            //var tops = db.Query<Top>(topsSql);

            //foreach (var top in tops)
            //{
            //    var realatedStrangers = db.Query<Stranger>(strangersSql, top);
            //    top.Strangers = realatedStrangers.ToList();
            //}

            var topsSql = @"SELECT *
                            FROM Tops";
            var strangersSql = @"SELECT *
                                 FROM strangers
                                 WHERE topid is not null";

            var tops = db.Query<Top>(topsSql);
            var strangers = db.Query<Stranger>(strangersSql);

            foreach (var top in tops)
            {
                top.Strangers = strangers.Where(s => s.TopId == top.Id).ToList();
            }

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
