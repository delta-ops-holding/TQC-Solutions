using DataAccessLibrary.Handlers.Helpers;
using DataAccessLibrary.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using ObjectLibrary.Clan;
using ObjectLibrary.Clan.Abstractions;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private readonly ISqlHandler _sqlHandler;
        private readonly IConfiguration _configuration;

        public ClanRepository(ISqlHandler sqlHandler, IConfiguration configuration)
        {
            _sqlHandler = sqlHandler;
            _configuration = configuration;
        }

        public Task<int> AssignClanAuthority(IClan clan, IMember member)
        {
            throw new NotImplementedException();
        }

        public Task<int> AssignClanEmoteAsync(IClan clan, ClanEmote clanEmote, bool overrideExistingData = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> AssignClanMentionRoleAsync(IClan clan, MentionRole mentionRole, bool overrideExistingData = true)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync(IClan entity)
        {
            try
            {
                string sp = "tqc_create_clan";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@platformId", entity.Platform.Id),
                    new SqlParameter("@name", entity.Name),
                    new SqlParameter("@about", entity.About)
                };

                return await _sqlHandler.ExecuteNonQueryAsync(sp, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IClan> GetByIdAsync(int id)
        {
            try
            {
                string sp = "tqc_get_clan";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                //using (var reader = await _sqlHandler.ExecuteReaderAsync(sp, CommandType.StoredProcedure, parameters))
                using (var reader = await SqlHelper.ExecuteReaderAsync(_configuration.GetConnectionString("TestDb"), sp, CommandType.StoredProcedure, parameters))
                {
                    if (reader.HasRows)
                    {
                        IClan clan = null;

                        while (await reader.ReadAsync())
                        {
                            if (clan != null)
                            {
                                if (clan.Id == reader.GetInt32(6) && reader.GetString(10).Equals("Founder"))
                                {
                                    ClanFounder founder = new(
                                        id: reader.GetInt32(7),
                                        userName: reader.GetString(8),
                                        new AuthorityType(
                                                id: reader.GetInt32(9),
                                                typeName:reader.GetString(10)));

                                    clan.AddClanFounder(founder);
                                }
                                else
                                {
                                    ClanAdmin admin = new(
                                        reader.GetInt32(7),
                                        reader.GetString(8),
                                        new AuthorityType(
                                            reader.GetInt32(9),
                                            reader.GetString(10)));

                                    clan.AddClanMember(admin);
                                }
                            }
                            else
                            {
                                clan = new Clan(
                                    id: reader.GetInt32(0),
                                    name: reader.GetString(1),
                                    about: reader.GetString(2),
                                    platform: new Platform(
                                        id: reader.GetInt32(3),
                                        name: reader.GetString(4),
                                        imagePath: reader.GetString(5)
                                    )
                                );
                            }
                        }

                        return await Task.FromResult(clan);
                    }

                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<IClan>> GetEntities()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RemoveAsync(IClan entity)
        {
            string sp = "tqc_remove_clan";
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@id", entity.Id)
            };

            return await _sqlHandler.ExecuteNonQueryAsync(sp, CommandType.StoredProcedure, parameters);
        }

        public Task<int> RemoveClanAuthority(IClan clan, IMember member)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveClanEmoteAsync(IClan clan)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveClanMentionRoleAsync(IClan clan)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(IClan entity)
        {
            try
            {
                string sp = "tqc_update_clan";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@platformId", entity.Platform.Id),
                    new SqlParameter("@name", entity.Name),
                    new SqlParameter("@about", entity.About)
                };

                return await _sqlHandler.ExecuteNonQueryAsync(sp, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
