using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Models;
using DatabaseAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private readonly IDatabase _databaseInstance;

        public ClanRepository(IDatabase database)
        {
            _databaseInstance = database;
        }

        public async Task<IEnumerable<Guild>> GetAllAsync()
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clan_getall",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = _databaseInstance.GetConnection()
            };

            try
            {
                await _databaseInstance.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                IList<Clan> tempClans = new List<Clan>();

                if (r.HasRows)
                {
                    try
                    {
                        while (await r.ReadAsync())
                        {
                            try
                            {
                                // Check if the clan exists in the temp list.
                                if (tempClans.Any(x => x.Identifier == r.GetInt32(0)))
                                {
                                    // Loop through the clans.
                                    foreach (var clan in tempClans)
                                    {
                                        if (clan.Identifier == r.GetInt32(7))
                                        {
                                            try
                                            {
                                                clan.Members.Add(
                                                    new ClanMember(r.GetBoolean(9), r.GetString(8), r.GetInt32(7)));
                                            }
                                            catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                                        }
                                    }
                                }
                                // Create new clan in the temp list.
                                else
                                {
                                    try
                                    {
                                        tempClans.Add(
                                            new Clan(r.GetString(1), r.GetString(2),
                                            new ClanPlatform(r.GetString(5), r.GetString(6), r.GetInt32(4)),
                                            new List<Member>(), r.GetInt32(0)));
                                    }
                                    catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }

                                    if (!r.IsDBNull(8))
                                    {
                                        // Loop through the clans.
                                        foreach (var clan in tempClans)
                                        {
                                            if (clan.Identifier == r.GetInt32(7))
                                            {
                                                try
                                                {
                                                    clan.Members.Add(
                                                        new ClanMember(r.GetBoolean(9), r.GetString(8), r.GetInt32(7)));
                                                }
                                                catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (ArgumentNullException) { throw new ArgumentNullException($"Collection was null, an error occurred."); }
                        }
                    }
                    catch (DbException) { throw new ExternalException($"Error occurred while executing command text.") as DbException; }
                }

                return tempClans;
            }
            catch (InvalidCastException) { throw; }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (SqlException) { throw; }
            catch (IOException) { throw; }
            catch (Exception) { throw; }
            finally { _databaseInstance.CloseConnection(); }
        }

        public async Task<Guild> GetAsync(uint identifier)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clan_getbyid",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = _databaseInstance.GetConnection()
            };

            c.Parameters.AddWithValue("@clanId", int.Parse(identifier.ToString()));

            try
            {
                Clan tempClan = null;

                await _databaseInstance.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    try
                    {
                        while (await r.ReadAsync())
                        {
                            if (tempClan != null)
                            {
                                // Check if the clan exists.
                                if (tempClan.Identifier == r.GetInt32(7))
                                {
                                    try
                                    {
                                        tempClan.Members.Add(
                                            new ClanMember(r.GetBoolean(9), r.GetString(8), r.GetInt32(7)));
                                    }
                                    catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                                }
                            }
                            else
                            {
                                tempClan = new Clan(
                                    r.GetString(1),
                                    r.GetString(2),
                                    new ClanPlatform(
                                       r.GetString(5),
                                       r.GetString(6),
                                       r.GetInt32(3)),
                                    new List<Member>()
                                    {
                                    new ClanMember(
                                        r.GetBoolean(9),
                                        r.GetString(8),
                                         r.GetInt32(7))
                                    },
                                    r.GetInt32(0));
                            }
                        }
                    }
                    catch (DbException) { throw new ExternalException($"Error occurred while executing command text.") as DbException; }
                }

                return tempClan;
            }
            catch (InvalidCastException) { throw; }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (SqlException) { throw; }
            catch (IOException) { throw; }
            catch (Exception) { throw; }
            finally { _databaseInstance.CloseConnection(); }
        }
    }
}