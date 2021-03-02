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
    public class ClanMemberV3Repository : IMemberRepository
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
                    List<ClanMember> temporaryClanMembers = new List<ClanMember>();

                    try
                    {
                        await db.OpenConnectionAsync();

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {

                            if (!dataReader.HasRows)
                            {
                                return temporaryClanMembers;
                            }

                            try
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    ClanMember temporaryClanMember = new ClanMember(
                                        identifier: (uint)dataReader.GetInt32(0),
                                        userName: dataReader.GetString(1),
                                        isFounder: dataReader.GetBoolean(2));

                                    temporaryClanMembers.Add(temporaryClanMember);
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("Failed to read Member Data.");
                            }
                        }

                        return temporaryClanMembers;
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
                    ClanMember temporaryClanMember = null;

                    try
                    {
                        command.Parameters.AddWithValue("@id", int.Parse(identifier.ToString()));

                        await db.OpenConnectionAsync();

                        using (var dataReader = await command.ExecuteReaderAsync())
                        {

                            if (!dataReader.HasRows)
                            {
                                return temporaryClanMember;
                            }

                            try
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    temporaryClanMember = new ClanMember(
                                        identifier: (uint)dataReader.GetInt32(0),
                                        userName: dataReader.GetString(1),
                                        isFounder: dataReader.GetBoolean(2));
                                }
                            }
                            catch (Exception)
                            {
                                throw new Exception("Failed to read Clan Data.");
                            }
                        }

                        return temporaryClanMember;
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