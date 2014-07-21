using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectModel;
using System.Configuration;
using System.Data.SqlClient;

namespace Data
{
    public class SQLNewsRepository : IRepository<News>
    {
        string connectionString;
        public SQLNewsRepository()
            : this("virtualMachineCS")
        {
        }
        public SQLNewsRepository(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (cs == null)
                throw new ApplicationException(string.Format("ConnectionString '{0}' not found", connectionStringName));
            connectionString = cs.ConnectionString;
        }
        public IEnumerable<News> GetAll()
        {
            List<News> news = new List<News>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT [ID]
                                ,[Data]
                                ,[Titolo]
                                ,[Testo]
                                ,[UrlImmagine]
                                FROM [dbo].[News]";

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            News news_obj = new News();
                            news_obj.Id = reader.GetValue<int>("ID");
                            news_obj.DataPubblicazione = reader.GetValue<string>("Data");
                            news_obj.Titolo = reader.GetValue<string>("Titolo");
                            news_obj.Testo = reader.GetValue<string>("Testo");
                            news_obj.UrlFoto = reader.GetValue<string>("UrlImmagine");

                            news.Add(news_obj);
                        }
                    }
                }
            }

            return news;
        }
        public IEnumerable<News> GetLastFive()
        {
            List<News> news = new List<News>();

            string query = @"SELECT top 5 [ID]
                            ,[Data]
                            ,[Titolo]
                            ,[Testo]
                            ,[UrlImmagine]
                             FROM News
                             ORDER BY Data DESC";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                News newsObj = new News();
                                newsObj.Id = reader.GetValue<int>("ID");
                                newsObj.DataPubblicazione = reader.GetValue<string>("Data");
                                newsObj.Titolo = reader.GetValue<string>("Titolo");
                                newsObj.Testo = reader.GetValue<string>("Testo");
                                newsObj.UrlFoto = reader.GetValue<string>("UrlImmagine");
                                news.Add(newsObj);
                            }
                            return news;
                        }
                    }
                }
            }
        }
        public News Get(int id)
        {
            string query = @"SELECT 
                                ID,
                                Data,
                                Titolo,
                                Testo,
                                UrlImmagine
                                FROM News
                                WHERE ID = @id";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));

                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                News news_obj = new News();
                                news_obj.Id = reader.GetValue<int>("ID");
                                news_obj.DataPubblicazione = reader.GetValue<string>("Data");
                                news_obj.Titolo = reader.GetValue<string>("Titolo");
                                news_obj.Testo = reader.GetValue<string>("Testo");
                                news_obj.UrlFoto = reader.GetValue<string>("UrlImmagine");

                                return news_obj;
                            }
                            return null;
                        }
                    }
                }
            }
        }
        public int Post(News news)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO [dbo].[News]
                                ([Data]
                                ,[Titolo]
                                ,[Testo]
                                ,[UrlImmagine])
                                OUTPUT INSERTED.ID
                                VALUES
                                (@Data
                                ,@Titolo
                                ,@Testo
                                ,@UrlImmagine);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Data", Date.getDate()));
                    command.Parameters.Add(new SqlParameter("@Titolo", news.Titolo));
                    command.Parameters.Add(new SqlParameter("@Testo", news.Testo));
                    command.Parameters.Add(new SqlParameter("@UrlImmagine", news.UrlFoto));

                    int lastID = (int)command.ExecuteScalar();

                    connection.Close();

                    string key = "News_id_" + lastID;

                    RedisNotificationRepository.Insert(key);

                    return lastID;
                }
            }
        }
        public bool Put(News news)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"UPDATE [dbo].[News]
                                    SET [Data] = @Data
                                       ,[Titolo] = @Titolo
                                       ,[Testo] = @Testo
                                       ,[UrlImmagine] = @UrlImmagine
                                 WHERE ID = @Id ;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(SqlHelper.CreateParameter("Id", (news.Id).ToString()));
                    command.Parameters.Add(SqlHelper.CreateParameter("Data", news.DataPubblicazione));
                    command.Parameters.Add(SqlHelper.CreateParameter("Titolo", news.Titolo));
                    command.Parameters.Add(SqlHelper.CreateParameter("Testo", news.Testo));
                    command.Parameters.Add(SqlHelper.CreateParameter("UrlImmagine", news.UrlFoto));

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows == 1)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"DELETE FROM [dbo].[News]
                                WHERE ID = @id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@id", id));

                    int count = command.ExecuteNonQuery();
                }
                connection.Close();

                RedisNotificationRepository.Delete("News_id_" + id);
            }
        }
        public IEnumerable<News> Search(string keyWord)
        {
            List<News> searched_news = new List<News>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT [ID]
                                ,[Data]
                                ,[Titolo]
                                ,[Testo]
                                ,[UrlImmagine]
                                FROM [dbo].[News]
                                WHERE (Titolo LIKE '%" + keyWord + "%') OR (Testo LIKE '%" + keyWord + "%')";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Keyword", keyWord));

                    using (
                        SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            News news = new News();
                            news.Id = reader.GetValue<int>("ID");
                            news.DataPubblicazione = reader.GetValue<string>("Data");
                            news.Titolo = reader.GetValue<string>("Titolo");
                            news.Testo = reader.GetValue<string>("Testo");
                            news.UrlFoto = reader.GetValue<string>("UrlImmagine");

                            searched_news.Add(news);
                        }
                    }
                }
            }
            return searched_news;
        }
    }
}
