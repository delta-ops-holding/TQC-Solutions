using ApiEmily.Database;
using ApiEmily.Models;
using ApiEmily.Models.V3;
using ApiEmily.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiEmily.Repositories.V3
{
    public class ClanV3Repository : IClanRepository
    {
        public async Task<IEnumerable<Guild>> GetAllAsync()
        {
            using (var db = SqlDatabase.SqlInstance)
            {
                using (var command = new SqlCommand
                {
                    CommandText = "proc_clan_v3_get_all",
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 10,
                    Connection = db.GetConnection()
                })
                {
                    var temporaryClans = new List<ClanV3>();

                    try
                    {
                        await db.OpenConnectionAsync();

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {
                            if (!dataReader.HasRows)
                            {
                                return temporaryClans;
                            }

                            try
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var temporaryClan = new ClanV3(
                                        identifier: dataReader.GetInt32(0),
                                        name: dataReader.GetString(1),
                                        about: dataReader.GetString(2),
                                        clanPlatform: new ClanPlatform(
                                            identifier: dataReader.GetInt32(3),
                                            name: dataReader.GetString(5),
                                            platformImageURL: dataReader.GetString(6)),
                                        founder: new ClanFounder(
                                            identity: dataReader.GetInt32(7),
                                            userName: dataReader.GetString(8),
                                            isFounder: dataReader.GetBoolean(9))
                                        );

                                    temporaryClans.Add(temporaryClan);
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("Failed to read Clan Data.");
                            }
                        }

                        return temporaryClans;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public async Task<Guild> GetAsync(uint identifier)
        {
            using (var db = SqlDatabase.SqlInstance)
            {
                using (var command = new SqlCommand
                {
                    CommandText = "proc_clan_v3_get_by_id",
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 10,
                    Connection = db.GetConnection()
                })
                {
                    ClanV3 temporaryClan = null;

                    try
                    {
                        command.Parameters.AddWithValue("@id", int.Parse(identifier.ToString()));

                        await db.OpenConnectionAsync();

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {

                            if (!dataReader.HasRows)
                            {
                                return temporaryClan;
                            }

                            try
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    temporaryClan = new ClanV3(
                                        identifier: dataReader.GetInt32(0),
                                        name: dataReader.GetString(1),
                                        about: dataReader.GetString(2),
                                        clanPlatform: new ClanPlatform(
                                            identifier: dataReader.GetInt32(3),
                                            name: dataReader.GetString(5),
                                            platformImageURL: dataReader.GetString(6)),
                                        founder: new ClanFounder(
                                            identity: dataReader.GetInt32(7),
                                            userName: dataReader.GetString(8),
                                            isFounder: dataReader.GetBoolean(9))
                                        );
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("Failed to read Clan Data.");
                            }
                        }

                        return temporaryClan;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Failed to handle request.");
                    }
                }
            }
        }
    }
}