using DatabaseAccess.Database;
using DatabaseAccess.Models;
using DatabaseAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.V3
{
    public class ClanFounderV3Repository : IMemberRepository
    {
        public async Task<IEnumerable<Member>> GetAllAsync()
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
                    List<Member> temporaryClanFounders = new List<Member>();

                    try
                    {
                        await db.OpenConnectionAsync();

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {

                            if (!dataReader.HasRows)
                            {
                                return temporaryClanFounders;
                            }

                            try
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    ClanFounder temporaryClanMember = new ClanFounder(
                                        identity: dataReader.GetInt32(0),
                                        userName: dataReader.GetString(1),
                                        isFounder: dataReader.GetBoolean(2));

                                    temporaryClanFounders.Add(temporaryClanMember);
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("Failed to read Member Data.");
                            }
                        }

                        return temporaryClanFounders;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Failed to handle request.");
                    }
                }
            }
        }

        public async Task<Member> GetAsync(uint identifier)
        {
            using (var db = SqlDatabase.SqlInstance)
            {
                using (var command = new SqlCommand
                {
                    CommandText = "proc_clan_authority_v3_get_by_id",
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 10,
                    Connection = db.GetConnection()
                })
                {
                    Member temporaryClanFounder = null;

                    try
                    {
                        command.Parameters.AddWithValue("@id", int.Parse(identifier.ToString()));

                        await db.OpenConnectionAsync();

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {

                            if (!dataReader.HasRows)
                            {
                                return temporaryClanFounder;
                            }

                            try
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    temporaryClanFounder = new ClanFounder(
                                        identity: dataReader.GetInt32(0),
                                        userName: dataReader.GetString(1),
                                        isFounder: dataReader.GetBoolean(2));
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("Failed to read Clan Data.");
                            }
                        }

                        return temporaryClanFounder;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Failed to handle request.");
                    }
                }
            }
        }

        public Task<IEnumerable<Member>> GetByIdAsync(uint identifier)
        {
            throw new NotImplementedException();
        }
    }
}