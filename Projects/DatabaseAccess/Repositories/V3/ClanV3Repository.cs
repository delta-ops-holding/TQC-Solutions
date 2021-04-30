using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Models;
using DatabaseAccess.Models.V3;
using DatabaseAccess.Repositories.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.V3
{
    public class ClanV3Repository : IClanRepository
    {
        private readonly IDatabase _databaseInstance;

        public ClanV3Repository(IDatabase configuration)
        {
            _databaseInstance = configuration;
        }

        public async Task<IEnumerable<Guild>> GetAllAsync()
        {
            using var command = new SqlCommand
            {
                CommandText = "proc_clan_v3_get_all",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 10,
                Connection = _databaseInstance.GetConnection()
            };
            var temporaryClans = new List<ClanV3>();

            try
            {
                await _databaseInstance.OpenConnectionAsync();

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
                            // Check if clan already exists.
                            if (temporaryClans.Any(clan => clan.Identifier == dataReader.GetInt32(0)) == false)
                            {
                                var clanObj = new ClanV3(
                                    identifier: dataReader.GetInt32(0),
                                    name: dataReader.GetString(1),
                                    about: dataReader.GetString(2),
                                    clanPlatform: new ClanPlatform(identifier: dataReader.GetInt32(3), name: dataReader.GetString(5), platformImageURL: dataReader.GetString(6)),
                                    founder: dataReader.GetBoolean(9) == true ? new ClanFounder(identity: dataReader.GetInt32(7), userName: dataReader.GetString(8), isFounder: dataReader.GetBoolean(9)) : null,
                                    admins: new List<Member>()
                                );

                                temporaryClans.Add(clanObj);
                            }                            

                            // If member is not founder.
                            if (dataReader.GetBoolean(9) == false)
                            {
                                var memberObj = new ClanAdminV3(
                                        identifier: dataReader.GetInt32(7),
                                        userName: dataReader.GetString(8)
                                    );

                                if (temporaryClans.Any(clan => clan.Identifier == dataReader.GetInt32(7)))
                                {
                                    var getClan = temporaryClans.Where(clan => clan.Identifier == dataReader.GetInt32(7)).FirstOrDefault();

                                    getClan.Admins.Add(memberObj);
                                }
                            }

                            //var temporaryClan = new ClanV3(
                            //    identifier: dataReader.GetInt32(0),
                            //    name: dataReader.GetString(1),
                            //    about: dataReader.GetString(2),
                            //    clanPlatform: new ClanPlatform(
                            //        identifier: dataReader.GetInt32(3),
                            //        name: dataReader.GetString(5),
                            //        platformImageURL: dataReader.GetString(6)),
                            //    founder: new ClanFounder(
                            //        identity: dataReader.GetInt32(7),
                            //        userName: dataReader.GetString(8),
                            //        isFounder: dataReader.GetBoolean(9))
                            //    );
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
                throw;
            }
        }

        public async Task<Guild> GetAsync(uint identifier)
        {
            using var command = new SqlCommand
            {
                CommandText = "proc_clan_v3_get_by_id",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 10,
                Connection = _databaseInstance.GetConnection()
            };
            ClanV3 temporaryClan = null;

            try
            {
                command.Parameters.AddWithValue("@id", int.Parse(identifier.ToString()));

                await _databaseInstance.OpenConnectionAsync();

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
                            if (temporaryClan == null)
                            {
                                temporaryClan = new ClanV3(
                                    identifier: dataReader.GetInt32(0),
                                    name: dataReader.GetString(1),
                                    about: dataReader.GetString(2),
                                    clanPlatform: new ClanPlatform(identifier: dataReader.GetInt32(3), name: dataReader.GetString(5), platformImageURL: dataReader.GetString(6)),
                                    founder: dataReader.GetBoolean(9) == true ? new ClanFounder(identity: dataReader.GetInt32(7), userName: dataReader.GetString(8), isFounder: dataReader.GetBoolean(9)) : null,
                                    admins: new List<Member>()
                                );
                            }

                            // If member is not founder.
                            if (dataReader.GetBoolean(9) == false)
                            {
                                var memberObj = new ClanAdminV3(
                                        identifier: dataReader.GetInt32(7),
                                        userName: dataReader.GetString(8)
                                    );

                                temporaryClan.Admins.Add(memberObj);
                            }

                            //temporaryClan = new ClanV3(
                            //    identifier: dataReader.GetInt32(0),
                            //    name: dataReader.GetString(1),
                            //    about: dataReader.GetString(2),
                            //    clanPlatform: new ClanPlatform(
                            //        identifier: dataReader.GetInt32(3),
                            //        name: dataReader.GetString(5),
                            //        platformImageURL: dataReader.GetString(6)),
                            //    founder: new ClanFounder(
                            //        identity: dataReader.GetInt32(7),
                            //        userName: dataReader.GetString(8),
                            //        isFounder: dataReader.GetBoolean(9))
                            //    );
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