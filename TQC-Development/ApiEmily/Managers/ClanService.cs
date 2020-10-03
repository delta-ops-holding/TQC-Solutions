using ApiEmily.Models;
using ApiEmily.Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiEmily.Managers
{
    public class ClanService
    {
        public async Task<IEnumerable<Clan>> GetAllClans()
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clan_getall",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                IList<Clan> tempClans = new List<Clan>();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        // Check if the clan exists in the temp list.
                        if (tempClans.Any(x => x.BaseId == r.GetInt32(0)))
                        {
                            // Loop through the clans.
                            foreach (var clan in tempClans)
                            {
                                if (clan.BaseId == r.GetInt32(7))
                                {
                                    clan.ClanAuthorities.Add(new ClanAuthority() { BaseId = r.GetInt32(7), UserName = r.GetString(8), IsFounder = r.GetBoolean(9) });
                                }
                            }
                        }
                        // Create new clan in the temp list.
                        else
                        {
                            tempClans.Add(
                                new Clan()
                                {
                                    BaseId = r.GetInt32(0),
                                    Name = r.GetString(1),
                                    About = r.GetString(2),
                                    ClanPlatform = new ClanPlatform()
                                    {
                                        BaseId = r.GetInt32(4),
                                        Name = r.GetString(5),
                                        PlatformImageURL = r.GetString(6)
                                    },
                                    ClanAuthorities = new List<ClanAuthority>()
                                });

                            if (!r.IsDBNull(8))
                            {
                                // Loop through the clans.
                                foreach (var clan in tempClans)
                                {
                                    if (clan.BaseId == r.GetInt32(7))
                                    {
                                        clan.ClanAuthorities.Add(new ClanAuthority() { BaseId = r.GetInt32(7), UserName = r.GetString(8), IsFounder = r.GetBoolean(9) });
                                    }
                                }
                            }
                        }
                    }
                }

                return tempClans;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public async Task<IEnumerable<ClanAuthority>> GetAllAuthorities()
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanauthority_getall",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            try
            {
                IList<ClanAuthority> clanAuthorities = new List<ClanAuthority>();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        clanAuthorities.Add(
                            new ClanAuthority()
                            {
                                BaseId = r.GetInt32(0),
                                UserName = r.GetString(1),
                                IsFounder = r.GetBoolean(2)
                            }
                        );
                    }
                }

                return clanAuthorities;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public async Task<IEnumerable<ClanPlatform>> GetAllPlatforms()
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanplatform_getall",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            try
            {
                IList<ClanPlatform> clanPlatforms = new List<ClanPlatform>();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        clanPlatforms.Add(
                            new ClanPlatform()
                            {
                                BaseId = r.GetInt32(0),
                                Name = r.GetString(1),
                                PlatformImageURL = r.GetString(2)
                            }
                        );
                    }
                }

                return clanPlatforms;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public async Task<Clan> GetClanById(int id)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clan_getbyid",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            c.Parameters.AddWithValue("@clanId", id);

            try
            {
                Clan tempClan = new Clan();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        // Check if the clan exists.
                        if (tempClan.BaseId == r.GetInt32(7))
                        {
                            if (tempClan.BaseId == r.GetInt32(7))
                            {
                                tempClan.ClanAuthorities.Add(
                                    new ClanAuthority()
                                    {
                                        BaseId = r.GetInt32(7),
                                        UserName = r.GetString(8),
                                        IsFounder = r.GetBoolean(9)
                                    }
                                );
                            }
                        }
                        else
                        {
                            tempClan = new Clan()
                            {
                                BaseId = r.GetInt32(0),
                                Name = r.GetString(1),
                                About = r.GetString(2),
                                ClanPlatform = new ClanPlatform() { BaseId = r.GetInt32(3), Name = r.GetString(5), PlatformImageURL = r.GetString(6) },
                                ClanAuthorities = new List<ClanAuthority>()
                                {
                                    new ClanAuthority()
                                    {
                                        BaseId = r.GetInt32(7),
                                        UserName = r.GetString(8),
                                        IsFounder = r.GetBoolean(9)
                                    }
                                }
                            };
                        }
                    }
                }

                return tempClan;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public async Task<ClanPlatform> GetPlatformById(int id)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanplatform_getbyid",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            c.Parameters.AddWithValue("@clanPlatformId", id);

            try
            {
                ClanPlatform clanPlatform = new ClanPlatform();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        clanPlatform = new ClanPlatform()
                        {
                            BaseId = r.GetInt32(0),
                            Name = r.GetString(1),
                            PlatformImageURL = r.GetString(2)
                        };
                    }
                }

                return clanPlatform;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public async Task<IEnumerable<ClanAuthority>> GetAuthoritiesById(int id)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_clanauthority_getbyid",
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            c.Parameters.AddWithValue("@clanId", id);

            try
            {
                IList<ClanAuthority> clanAuthorities = new List<ClanAuthority>();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        clanAuthorities.Add(
                            new ClanAuthority()
                            {
                                BaseId = r.GetInt32(0),
                                UserName = r.GetString(1),
                                IsFounder = r.GetBoolean(2)
                            }
                        );
                    }
                }

                return clanAuthorities;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }
    }
}