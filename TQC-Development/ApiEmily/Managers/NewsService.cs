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
    public class NewsService
    {
        public void Delete(int id)
        {
            // Initialzie new command obj.
            using (SqlCommand newsCommand = new SqlCommand())
            {
                newsCommand.CommandText = "proc_news_delete";
                newsCommand.CommandType = CommandType.StoredProcedure;
                newsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                newsCommand.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Execute news command.
                newsCommand.ExecuteNonQuery();

                // Close connection to database.
                SqlDataAccess.Instance.CloseConnection();
            };            
        }

        public async Task<IEnumerable<News>> GetAll()
        {
            // Initialzie new command obj.
            using SqlCommand newsCommand = new SqlCommand
            {
                CommandText = "proc_news_all",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie new data reader.
            using (SqlDataReader newsDataReader = newsCommand.ExecuteReader())
            {
                // Define temporary type.
                List<News> newsList = null;

                // Check if any rows.
                if (newsDataReader.HasRows)
                {
                    // Initialize new list of news.
                    newsList = new List<News>();

                    // Read data.
                    while (await newsDataReader.ReadAsync())
                    {
                        newsList.Add(
                            new News()
                            {
                                BaseId = newsDataReader.GetInt32(0),
                                Version = newsDataReader.GetString(1),
                                Title = newsDataReader.GetString(2),
                                Content = newsDataReader.GetString(3),
                                RegisteredDateTime = newsDataReader.GetDateTime(4),
                                LastUpdatedDateTime = newsDataReader.GetDateTime(5)
                            }
                        );
                    }
                }

                // Return data ?? null if no rows.
                return newsList;
            }
        }

        public async Task<News> GetById(int id)
        {
            // Initialzie new command obj.
            using SqlCommand newsCommand = new SqlCommand
            {
                CommandText = "proc_news_get",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.GetSqlConnection
            };

            // Define input parameters.
            newsCommand.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie new data reader.
            using (SqlDataReader newsDataReader = newsCommand.ExecuteReader())
            {
                // Define temporary type.
                News newsObj = null;

                // Check if any rows.
                if (newsDataReader.HasRows)
                {
                    // Read data.
                    while (await newsDataReader.ReadAsync())
                    {
                        newsObj = new News()
                        {
                            BaseId = newsDataReader.GetInt32(0),
                            Version = newsDataReader.GetString(1),
                            Title = newsDataReader.GetString(2),
                            Content = newsDataReader.GetString(3),
                            RegisteredDateTime = newsDataReader.GetDateTime(4),
                            LastUpdatedDateTime = newsDataReader.GetDateTime(5)
                        };
                    }
                }

                // Return data ?? null if no rows.
                return newsObj;
            }
        }

        public void Insert(News obj)
        {
            // Initialize new command obj.
            using (SqlCommand newsCommand = new SqlCommand())
            {
                newsCommand.CommandText = "proc_news_insert";
                newsCommand.CommandType = CommandType.StoredProcedure;
                newsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                newsCommand.Parameters.AddWithValue("@version", obj.Version);
                newsCommand.Parameters.AddWithValue("@title", obj.Title);
                newsCommand.Parameters.AddWithValue("@content", obj.Content);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Execute news command.
                newsCommand.ExecuteNonQuery();

                // Close connection to database.
                SqlDataAccess.Instance.CloseConnection();
            }           
        }

        public void Update(News obj)
        {
            // Initialize new command obj.
            using (SqlCommand newsCommand = new SqlCommand())
            {
                newsCommand.CommandText = "proc_news_update";
                newsCommand.CommandType = CommandType.StoredProcedure;
                newsCommand.Connection = SqlDataAccess.Instance.GetSqlConnection;

                // Define input parameters.
                newsCommand.Parameters.AddWithValue("@id", obj.BaseId);
                newsCommand.Parameters.AddWithValue("@version", obj.Version);
                newsCommand.Parameters.AddWithValue("@title", obj.Title);
                newsCommand.Parameters.AddWithValue("@content", obj.Content);

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Execute news command.
                newsCommand.ExecuteNonQuery();

                // Close connection to database.
                SqlDataAccess.Instance.CloseConnection();
            }            
        }
    }
}