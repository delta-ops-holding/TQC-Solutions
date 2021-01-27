using ApiEmily.Database;
using ApiEmily.Models;
using ApiEmily.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace ApiEmily.Repositories
{
    public class ClanMemberRepository : IMemberRepository
    {
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            using var db = SqlDatabase.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanauthority_getall",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            try
            {
                List<Member> clanMembers = new List<Member>();

                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    try
                    {
                        while (await r.ReadAsync())
                        {
                            try
                            {
                                clanMembers.Add(
                                    new ClanMember(
                                        r.GetBoolean(2),
                                        r.GetString(1),
                                        (uint)r.GetInt32(0)));
                            }
                            catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                        }
                    }
                    catch (DbException) { throw new ExternalException($"Error occurred while executing command text.") as DbException; }
                }

                return clanMembers;
            }
            catch (InvalidCastException) { throw; }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (SqlException) { throw; }
            catch (IOException) { throw; }
            catch (Exception) { throw; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Get a clan member by id.
        /// </summary>
        /// <param name="identifier">Defines the identifier of the member.</param>
        /// <returns>A <see cref="NotSupportedException"/>.</returns>
        public Task<Member> GetAsync(uint identifier)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Member>> GetByIdAsync(uint identifier)
        {
            using var db = SqlDatabase.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanauthority_getbyid",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@clanId", int.Parse(identifier.ToString()));

            try
            {
                List<Member> clanMembers = new List<Member>();

                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    try
                    {
                        while (await r.ReadAsync())
                        {
                            try
                            {
                                try
                                {
                                    clanMembers.Add(new ClanMember(r.GetBoolean(2), r.GetString(1), uint.Parse(r.GetInt32(0).ToString())));
                                }
                                catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                            }
                            catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                        }
                    }
                    catch (DbException) { throw new ExternalException($"Error occurred while executing command text.") as DbException; }
                }

                return clanMembers;
            }
            catch (InvalidCastException) { throw; }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (SqlException) { throw; }
            catch (IOException) { throw; }
            catch (Exception) { throw; }
            finally { db.CloseConnection(); }
        }
    }
}