﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace liver.Models
{

    public interface IMiningRepository
    {
        int Add(MiningDig dig);

        List<MiningDig> GetAllDigs();

        MiningStatistics GetStats();

        decimal GetDifficulty();
    }

    public class MiningRepository : IMiningRepository
    {
        IConfiguration _configuration;

        public MiningRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Add(MiningDig dig)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO [MiningDigs](Client,CoinsMined,MillisecondTaken) VALUES(@Client, @CoinsMined, @MillisecondTaken); SELECT CAST(SCOPE_IDENTITY() as INT);";
                    count = con.Execute(query, dig);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count;
            }
        }

        public List<MiningDig> GetAllDigs()
        {
            throw new NotImplementedException();
        }

        public MiningStatistics GetStats()
        {
            var connectionString = this.GetConnection();
            var _MiningStatistics = new MiningStatistics();
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    var query = "SELECT COUNT (DISTINCT Client) FROM MiningDigs";
                    var MinerCount = con.ExecuteScalar<int>(query);

                    query = "SELECT SUM(CoinsMined) FROM MiningDigs";
                    var CoinsMined = con.ExecuteScalar<decimal>(query);

                    _MiningStatistics.MinedCoinsTotal = CoinsMined;
                    _MiningStatistics.NumberOfMiners = MinerCount;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return _MiningStatistics;
            }
        }

        public decimal GetDifficulty()
        {
            var connectionString = this.GetConnection();
            decimal _DifficultyLevel;
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    
                    var query = "SELECT DifficultyLevel FROM [DifficultySetting] where [Id] = (select max(id) from DifficultySetting)";
                    _DifficultyLevel = con.ExecuteScalar<decimal>(query);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return _DifficultyLevel;
            }
        }


        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("MiningDBContext").Value;
            
            return connection;
        }

    }

}