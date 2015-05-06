#define SUCCESS 

using System;
using System.Data;
using System.Data.SqlClient;


namespace WebAPI
{
    public class ServiceAPI : IServiceAPI
    {
        SqlConnection dbConnection;

        public ServiceAPI()
        {
            dbConnection = DBConnect.getConnection();
        }

        public int CreateNewAccount(string cnp, string email, string password)
        {
            const int INVALIDCNP = 0;
            const int ALREADYEXISTS = 1;
            const int SUCCESS = 2;

            int registerState = INVALIDCNP;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT CNP FROM Users WHERE CNP='" + cnp + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                registerState = ALREADYEXISTS;
            }

            query = "SELECT CNP FROM evidstud WHERE CNP='" + cnp + "';";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                registerState = SUCCESS;
                query = "INSERT INTO Users VALUES ('" + cnp + "','" + email + "','" + password + "');";
                command = new SqlCommand(query, dbConnection);
                command.ExecuteNonQuery();
            }
            else
            {
                registerState = INVALIDCNP;
            }

            dbConnection.Close();


            return registerState;
        }

        public bool UserAuthentication(string cnp, string passsword)
        {
            bool auth = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT CNP FROM Users WHERE CNP='" + cnp + "' AND password='" + passsword + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                auth = true;
            }

            reader.Close();
            dbConnection.Close();

            return auth;

        }
    }
}