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
    public class ClanPlatformRepository : IClanPlatformRepository
    {
        public async Task<IEnumerable<ClanPlatform>> GetAllAsync()
        {
            using var db = SqlDatabase.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanplatform_getall",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            try
            {
                List<ClanPlatform> clanPlatforms = new List<ClanPlatform>();

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
                                clanPlatforms.Add(
                                    new ClanPlatform(
                                        r.GetString(1),
                                        r.GetString(2),
                                        r.GetInt32(0))
                            );
                            }
                            catch (NotSupportedException) { throw new NotSupportedException($"Collection was read-only, an error has occurred."); }
                        }
                    }
                    catch (DbException) { throw new ExternalException($"Error occurred while executing command text.") as DbException; }
                }

                return clanPlatforms;
            }
            catch (InvalidCastException) { throw; }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (SqlException) { throw; }
            catch (IOException) { throw; }
            catch (Exception) { throw; }
            finally { db.CloseConnection(); }
        }

        public async Task<ClanPlatform> GetAsync(uint identifier)
        {
            using var db = SqlDatabase.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanplatform_getbyid",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@clanPlatformId", int.Parse(identifier.ToString()));

            try
            {
                ClanPlatform clanPlatform = null;

                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    try
                    {
                        while (await r.ReadAsync())
                        {
                            clanPlatform = new ClanPlatform(
                               r.GetString(1),
                               r.GetString(2),
                               r.GetInt32(0));
                        }
                    }
                    catch (DbException) { throw new ExternalException($"Error occurred while executing command text.") as DbException; }
                }

                return clanPlatform;
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