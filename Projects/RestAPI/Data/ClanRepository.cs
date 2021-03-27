using RestAPI.Data.DataAccess;
using RestAPI.Data.Interfaces;
using RestAPI.Data.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data
{
    public class ClanRepository : IClanRepository, IClanAuthorityRepository, IClanPlatformRepository
    {
        void IRepository<Clan>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository<ClanAuthority>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository<ClanPlatform>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository<Clan>.Insert(Clan insert)
        {
            throw new NotImplementedException();
        }

        void IRepository<ClanAuthority>.Insert(ClanAuthority insert)
        {
            throw new NotImplementedException();
        }

        void IRepository<ClanPlatform>.Insert(ClanPlatform insert)
        {
            throw new NotImplementedException();
        }

        void IRepository<Clan>.Update(Clan update)
        {
            throw new NotImplementedException();
        }

        void IRepository<ClanAuthority>.Update(ClanAuthority update)
        {
            throw new NotImplementedException();
        }

        void IRepository<ClanPlatform>.Update(ClanPlatform update)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<Clan>> IRepository<Clan>.GetAll()
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

                            if (!DataReaderExtensions.IsDBNull(r, "UserName"))
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

        async Task<IEnumerable<ClanAuthority>> IRepository<ClanAuthority>.GetAll()
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
                                BaseId = r.GetInt32("ClanId"),
                                UserName = r.GetString("UserName"),
                                IsFounder = r.GetBoolean("IsFounder")
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

        async Task<IEnumerable<ClanPlatform>> IRepository<ClanPlatform>.GetAll()
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
                                BaseId = r.GetInt32("Id"),
                                Name = r.GetString("Name"),
                                PlatformImageURL = r.GetString("PlatformImageURL")
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

        async Task<Clan> IRepository<Clan>.GetById(int id)
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
                        if (tempClan.BaseId == r.GetInt32("ClanId"))
                        {
                            if (tempClan.BaseId == r.GetInt32("ClanId"))
                            {
                                tempClan.ClanAuthorities.Add(
                                    new ClanAuthority()
                                    {
                                        BaseId = r.GetInt32("ClanId"),
                                        UserName = r.GetString("UserName"),
                                        IsFounder = r.GetBoolean("IsFounder")
                                    }
                                );
                            }
                        }
                        else
                        {
                            tempClan = new Clan()
                            {
                                BaseId = r.GetInt32("Id"),
                                Name = r.GetString("Name"),
                                About = r.GetString("About"),
                                ClanPlatform = new ClanPlatform() { BaseId = r.GetInt32("ClanPlatformId"), Name = r.GetString(5), PlatformImageURL = r.GetString("PlatformImageURL") },
                                ClanAuthorities = new List<ClanAuthority>()
                                {
                                    new ClanAuthority()
                                    {
                                        BaseId = r.GetInt32("ClanId"),
                                        UserName = r.GetString("UserName"),
                                        IsFounder = r.GetBoolean("IsFounder")
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

        Task<ClanAuthority> IRepository<ClanAuthority>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        async Task<ClanPlatform> IRepository<ClanPlatform>.GetById(int id)
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
                            BaseId = r.GetInt32("Id"),
                            Name = r.GetString("Name"),
                            PlatformImageURL = r.GetString("PlatformImageURL")
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

        async Task<IEnumerable<ClanAuthority>> IClanAuthorityRepository.GetById(int id)
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
                                BaseId = r.GetInt32("ClanId"),
                                UserName = r.GetString("UserName"),
                                IsFounder = r.GetBoolean("IsFounder")
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
