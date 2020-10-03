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
    public class AboutUsService
    {
        public void Delete(int id)
        {
            // Initialzie new command obj.
            using (SqlCommand aboutUsCommand = new SqlCommand())
            {
                aboutUsCommand.CommandText = "proc_aboutus_delete";
                aboutUsCommand.CommandType = CommandType.StoredProcedure;
                aboutUsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                aboutUsCommand.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Execute news command.
                aboutUsCommand.ExecuteNonQuery();

                // Close connection to database.
                SqlDataAccess.Instance.CloseConnection();
            };            
        }

        public async Task<IEnumerable<AboutUs>> GetAll()
        {
            // Initialzie new command obj.
            using (SqlCommand aboutUsCommand = new SqlCommand())
            {
                aboutUsCommand.CommandText = "proc_aboutus_all";
                aboutUsCommand.CommandType = CommandType.StoredProcedure;
                aboutUsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Initialzie new data reader.
                using (SqlDataReader aboutUsDataReader = aboutUsCommand.ExecuteReader())
                {
                    // Define temporary type.
                    IList<AboutUs> aboutUsList = new List<AboutUs>();

                    // Check if any rows.
                    if (aboutUsDataReader.HasRows)
                    {
                        // Read data.
                        while (await aboutUsDataReader.ReadAsync())
                        {
                            aboutUsList.Add(
                                new AboutUs()
                                {
                                    BaseId = aboutUsDataReader.GetInt32(0),
                                    WelcomeMessage = aboutUsDataReader.GetString(1),
                                    Mission = aboutUsDataReader.GetString(2),
                                    Vision = aboutUsDataReader.GetString(3),
                                    RegisteredDateTime = aboutUsDataReader.GetDateTime(4),
                                    LastUpdatedDateTime = aboutUsDataReader.GetDateTime(5)
                                }
                            );
                        }
                    }

                    // Return data ?? null if no rows.
                    return aboutUsList;
                }                
            }                
        }

        public async Task<AboutUs> GetById(int id)
        {
            // Initialzie new command obj.
            using (SqlCommand aboutUsCommand = new SqlCommand())
            {
                aboutUsCommand.CommandText = "proc_aboutus_get";
                aboutUsCommand.CommandType = CommandType.StoredProcedure;
                aboutUsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                aboutUsCommand.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Initialzie new data reader.
                using (SqlDataReader aboutUsDataReader = aboutUsCommand.ExecuteReader())
                {
                    // Define temporary type.
                    AboutUs aboutUsObj = null;

                    // Check if any rows.
                    if (aboutUsDataReader.HasRows)
                    {
                        // Read data.
                        while (await aboutUsDataReader.ReadAsync())
                        {
                            aboutUsObj = new AboutUs()
                            {
                                BaseId = aboutUsDataReader.GetInt32(0),
                                WelcomeMessage = aboutUsDataReader.GetString(1),
                                Mission = aboutUsDataReader.GetString(2),
                                Vision = aboutUsDataReader.GetString(3),
                                RegisteredDateTime = aboutUsDataReader.GetDateTime(4),
                                LastUpdatedDateTime = aboutUsDataReader.GetDateTime(5)
                            };
                        }
                    }

                    // Return data ?? null if no rows.
                    return aboutUsObj;
                }
            };                       
        }

        public void Insert(AboutUs obj)
        {
            // Initialize new command obj.
            using (SqlCommand aboutUsCommand = new SqlCommand())
            {
                aboutUsCommand.CommandText = "proc_aboutus_insert";
                aboutUsCommand.CommandType = CommandType.StoredProcedure;
                aboutUsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                aboutUsCommand.Parameters.AddWithValue("@welcomeMessage", obj.WelcomeMessage);
                aboutUsCommand.Parameters.AddWithValue("@mission", obj.Mission);
                aboutUsCommand.Parameters.AddWithValue("@vision", obj.Vision);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Execute news command.
                aboutUsCommand.ExecuteNonQuery();

                // Close connection to database.
                SqlDataAccess.Instance.CloseConnection();
            }            
        }

        public void Update(AboutUs obj)
        {
            // Initialize new command obj.
            using (SqlCommand aboutUsCommand = new SqlCommand())
            {
                aboutUsCommand.CommandText = "proc_aboutus_update";
                aboutUsCommand.CommandType = CommandType.StoredProcedure;
                aboutUsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                aboutUsCommand.Parameters.AddWithValue("@id", obj.BaseId);
                aboutUsCommand.Parameters.AddWithValue("@welcomeMessage", obj.WelcomeMessage);
                aboutUsCommand.Parameters.AddWithValue("@mission", obj.Mission);
                aboutUsCommand.Parameters.AddWithValue("@vision", obj.Vision);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Execute news command.
                aboutUsCommand.ExecuteNonQuery();

                // Close connection to database.
                SqlDataAccess.Instance.CloseConnection();
            }            
        }
    }
}