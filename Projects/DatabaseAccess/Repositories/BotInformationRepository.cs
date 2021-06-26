using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Repositories.Interfaces;
using DataClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories
{
    public class BotInformationRepository : IBotInformationRepository
    {
        private readonly IDatabase _database;

        public BotInformationRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task<MinionInformationModel> GetBotInformationAsync()
        {
            using var command = new SqlCommand()
            {
                CommandText = "proc_getbotinformation",
                CommandType = CommandType.StoredProcedure,
                Connection = _database.GetConnection()
            };

            MinionInformationModel model = null;

            try
            {
                await _database.OpenConnectionAsync();

                using var r = await command.ExecuteReaderAsync();

                if (!r.HasRows) throw new Exception("No information.");

                while (await r.ReadAsync())
                {
                    model = new MinionInformationModel(
                        r.GetGuid(0),
                        r.GetString(1),
                        r.GetString(2),
                        r.GetString(3),
                        new List<DisplayStatusModel>());

                    if (r.GetGuid(5).Equals(r.GetGuid(0)))
                    {
                        model.DisplayStatuses.Add(
                            new DisplayStatusModel(
                                r.GetInt32(4),
                                r.GetGuid(5),
                                r.GetString(6)));
                    }
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
