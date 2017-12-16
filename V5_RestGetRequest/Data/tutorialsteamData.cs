using System.Configuration;
using System.Data.SqlClient;

namespace V5_RestGetRequest.Data
{
    public class TutorialsteamData
    {
        public static SqlConnection GetConnection()
        {
            //string connectionString = "Data Source=NICKS-ACER\\SQLEXPRESS;Initial Catalog=tutorialsteam;Integrated Security=SSPI;";
            string connectionString = ConfigurationManager.ConnectionStrings["ProductsEntities"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}



 
