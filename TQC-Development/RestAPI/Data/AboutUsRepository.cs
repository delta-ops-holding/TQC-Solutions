using RestAPI.Data.DB;
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
    public class AboutUsRepository : IAboutUsRepository
    {
        public void Delete(int id)
        {
            // Initialzie new command obj.
            using SqlCommand aboutUsCommand = new SqlCommand()
            {
                CommandText = "proc_aboutus_delete",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.GetSqlConnection
            };

            // Define input parameters.
            aboutUsCommand.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Execute news command.
            aboutUsCommand.ExecuteNonQuery();

            // Close connection to database.
            Database.Instance.CloseConnection();
        }

        public IEnumerable<AboutUs> GetAll()
        {
            // Initialzie new command obj.
            using SqlCommand aboutUsCommand = new SqlCommand()
            {
                CommandText = "proc_aboutus_all",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.GetSqlConnection
            };

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialzie new data reader.
            using SqlDataReader aboutUsDataReader = aboutUsCommand.ExecuteReader();

            // Define temporary type.
            List<AboutUs> aboutUsList = null;

            // Check if any rows.
            if (aboutUsDataReader.HasRows)
            {
                // Initialize new list of abouts.
                aboutUsList = new List<AboutUs>();

                // Read data.
                while (aboutUsDataReader.Read())
                {
                    aboutUsList.Add(
                        new AboutUs()
                        {
                            BaseId = aboutUsDataReader.GetInt32("Id"),
                            WelcomeMessage = aboutUsDataReader.GetString("Welcome"),
                            Mission = aboutUsDataReader.GetString("Mission"),
                            Vision = aboutUsDataReader.GetString("Vision"),
                            RegisteredDateTime = aboutUsDataReader.GetDateTime("AboutUsCreatedDate"),
                            LastUpdatedDateTime = aboutUsDataReader.GetDateTime("AboutUsUpdatedDate")
                        }
                    );
                }
            }

            // Return data ?? null if no rows.
            return aboutUsList;
        }

        public AboutUs GetById(int id)
        {
            // Initialzie new command obj.
            using SqlCommand aboutUsCommand = new SqlCommand()
            {
                CommandText = "proc_aboutus_get",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.GetSqlConnection
            };

            // Define input parameters.
            aboutUsCommand.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialzie new data reader.
            using SqlDataReader aboutUsDataReader = aboutUsCommand.ExecuteReader();

            // Define temporary type.
            AboutUs aboutUsObj = null;

            // Check if any rows.
            if (aboutUsDataReader.HasRows)
            {
                // Read data.
                while (aboutUsDataReader.Read())
                {
                    aboutUsObj = new AboutUs()
                    {
                        BaseId = aboutUsDataReader.GetInt32("Id"),
                        WelcomeMessage = aboutUsDataReader.GetString("Welcome"),
                        Mission = aboutUsDataReader.GetString("Mission"),
                        Vision = aboutUsDataReader.GetString("Vision"),
                        RegisteredDateTime = aboutUsDataReader.GetDateTime("AboutUsCreatedDate"),
                        LastUpdatedDateTime = aboutUsDataReader.GetDateTime("AboutUsUpdatedDate")
                    };
                }
            }

            // Return data ?? null if no rows.
            return aboutUsObj;
        }

        public void Insert(AboutUs obj)
        {
            // Initialize new command obj.
            using SqlCommand aboutUsCommand = new SqlCommand()
            {
                CommandText = "proc_aboutus_insert",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.GetSqlConnection
            };

            // Define input parameters.
            aboutUsCommand.Parameters.AddWithValue("@welcomeMessage", obj.WelcomeMessage);
            aboutUsCommand.Parameters.AddWithValue("@mission", obj.Mission);
            aboutUsCommand.Parameters.AddWithValue("@vision", obj.Vision);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Execute news command.
            aboutUsCommand.ExecuteNonQuery();

            // Close connection to database.
            Database.Instance.CloseConnection();
        }

        public void Update(AboutUs obj)
        {
            // Initialize new command obj.
            using SqlCommand aboutUsCommand = new SqlCommand()
            {
                CommandText = "proc_aboutus_update",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.GetSqlConnection
            };

            // Define input parameters.
            aboutUsCommand.Parameters.AddWithValue("@id", obj.BaseId);
            aboutUsCommand.Parameters.AddWithValue("@welcomeMessage", obj.WelcomeMessage);
            aboutUsCommand.Parameters.AddWithValue("@mission", obj.Mission);
            aboutUsCommand.Parameters.AddWithValue("@vision", obj.Vision);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Execute news command.
            aboutUsCommand.ExecuteNonQuery();

            // Close connection to database.
            Database.Instance.CloseConnection();
        }
    }
}
