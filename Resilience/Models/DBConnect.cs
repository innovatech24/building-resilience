using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Resilience.Models
{
    public class DBConnect
    {
        private string connetionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=Test_db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        private SqlConnection cnn;

        public void dbOpen()
        {
            cnn = new SqlConnection(connetionString);
            cnn.Open();
        }

        public string queryToJson(string q)
        {
            string output = "";
            SqlCommand command = new SqlCommand(q, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            output += dataReader.GetValue(0);

            return (output);
        }

        public void dbClose()
        {
            cnn.Close();
        }

    }
}
