using RestAPI.Data.DataAccess;
using RestAPI.Data.Interfaces;
using RestAPI.Data.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data
{
    public class StreamerRepository : IStreamerRepository
    {
        public void Delete(int id)
        {
            // Initialzie new command obj.
            using SqlCommand streamerCommand = new SqlCommand()
            {
                CommandText = "proc_streamer_delete",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Define input parameters.
            streamerCommand.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute news command.
            streamerCommand.ExecuteNonQuery();

            // Close connection to database.
            SqlDataAccess.Instance.CloseConnection();
        }

        public async Task<IEnumerable<Streamer>> GetAll()
        {
            // Initialzie new command obj.
            using SqlCommand streamerCommand = new SqlCommand()
            {
                CommandText = "proc_streamer_all",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie new data reader.
            using SqlDataReader streamerDataReader = streamerCommand.ExecuteReader();

            // Define temporary type.
            List<Streamer> streamersList = null;

            // Check if any rows.
            if (streamerDataReader.HasRows)
            {
                // Initialize new list of streamers.
                streamersList = new List<Streamer>();

                // Read data.
                while (await streamerDataReader.ReadAsync())
                {
                    streamersList.Add(
                        new Streamer()
                        {
                            BaseId = streamerDataReader.GetInt32("Id"),
                            Name = streamerDataReader.GetString("Name"),
                            StreamURL = streamerDataReader.GetString("StreamURL"),
                            StreamerImageURL = streamerDataReader.GetString("StreamerImageURL")
                        }
                    );
                }
            }

            // Return data ?? null if no rows.
            return streamersList;
        }

        public async Task<Streamer> GetById(int id)
        {
            // Initialzie new command obj.
            using SqlCommand streamerCommand = new SqlCommand()
            {
                CommandText = "proc_streamer_get",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Define input parameters.
            streamerCommand.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie new data reader.
            using SqlDataReader streamerDataReader = streamerCommand.ExecuteReader();

            // Define temporary type.
            Streamer streamerObj = null;

            // Check if any rows.
            if (streamerDataReader.HasRows)
            {
                // Read data.
                while (await streamerDataReader.ReadAsync())
                {
                    streamerObj = new Streamer()
                    {
                        BaseId = streamerDataReader.GetInt32("Id"),
                        Name = streamerDataReader.GetString("Name"),
                        StreamURL = streamerDataReader.GetString("StreamURL"),
                        StreamerImageURL = streamerDataReader.GetString("StreamerImageURL")
                    };
                }
            }

            // Return data ?? null if no rows.
            return streamerObj;
        }

        public void Insert(Streamer obj)
        {
            // Initialize new command obj.
            using SqlCommand streamerCommand = new SqlCommand()
            {
                CommandText = "proc_streamer_insert",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Define input parameters.
            streamerCommand.Parameters.AddWithValue("@name", obj.Name);
            streamerCommand.Parameters.AddWithValue("@streamURL", obj.StreamURL);
            streamerCommand.Parameters.AddWithValue("@streamerImageURL", obj.StreamerImageURL);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute news command.
            streamerCommand.ExecuteNonQuery();

            // Close connection to database.
            SqlDataAccess.Instance.CloseConnection();
        }

        public void Update(Streamer obj)
        {
            // Initialize new command obj.
            using SqlCommand streamerCommand = new SqlCommand()
            {
                CommandText = "proc_streamer_update",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Define input parameters.
            streamerCommand.Parameters.AddWithValue("@id", obj.BaseId);
            streamerCommand.Parameters.AddWithValue("@name", obj.Name);
            streamerCommand.Parameters.AddWithValue("@streamURL", obj.StreamURL);
            streamerCommand.Parameters.AddWithValue("@streamerImageURL", obj.StreamerImageURL);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute news command.
            streamerCommand.ExecuteNonQuery();

            // Close connection to database.
            SqlDataAccess.Instance.CloseConnection();
        }
    }
}
