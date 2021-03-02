using ApiEmily.Database;
using ApiEmily.Models;
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
        public async Task<IEnumerable<Clan>> GetAllAsync()
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
                    IList<Clan> temporaryClans = new List<Clan>();

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
                                    Clan temporaryClan = new Clan(
                                        identifier: (uint)dataReader.GetInt32(0),
                                        name: dataReader.GetString(1),
                                        about: dataReader.GetString(2),
                                        clanPlatform: new ClanPlatform(
                                            identifier: (uint)dataReader.GetInt32(3),
                                            name: dataReader.GetString(5),
                                            platformImageURL: dataReader.GetString(6)),
                                        members: new List<Member>()
                                        {
                                            new ClanMember(
                                            identifier: (uint)dataReader.GetInt32(7),
                                            userName: dataReader.GetString(8),
                                            isFounder: dataReader.GetBoolean(9))
                                        });

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
                    catch (Exception)
                    {
                        throw new Exception("Failed to handle request.");
                    }
                }
            }
        }

        public async Task<Clan> GetAsync(uint identifier)
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
                    Clan temporaryClan = null;

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
                                    temporaryClan = new Clan(
                                        identifier: (uint)dataReader.GetInt32(0),
                                        name: dataReader.GetString(1),
                                        about: dataReader.GetString(2),
                                        clanPlatform: new ClanPlatform(
                                            identifier: (uint)dataReader.GetInt32(3),
                                            name: dataReader.GetString(5),
                                            platformImageURL: dataReader.GetString(6)),
                                        members: new List<Member>()
                                        {
                                            new ClanMember(
                                            identifier: (uint)dataReader.GetInt32(7),
                                            userName: dataReader.GetString(8),
                                            isFounder: dataReader.GetBoolean(9))
                                        });
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