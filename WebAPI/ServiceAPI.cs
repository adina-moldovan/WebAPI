#define SUCCESS 

using System;
using System.Collections.Generic;
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

        public void CreateDemand(string cnp, string addressee, string request, string mention)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "INSERT INTO cereri VALUES ('" + cnp + "','" + addressee + "', '"+ request + "','" + mention + "','" + DateTime.Now +"';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            dbConnection.Close();
        }
        public void CreateCertificate(string cnp, string reason)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "INSERT INTO adev VALUES ('" + cnp + "','" + reason + "','" + DateTime.Now + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            dbConnection.Close();
        }
        public UniversityYearInfo GetMarks(string cnp, string year)
        {
            var yearInfo = new UniversityYearInfo();

            yearInfo.yearOfStudy = year;

            yearInfo.marksTable.Columns.Add(new DataColumn("subjectName", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("semester", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("attempt1", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("attempt2", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("attempt3", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("activityGrade", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("credits", typeof(String)));
            yearInfo.marksTable.Columns.Add(new DataColumn("department", typeof(String)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT * FROM foi_matr WHERE cnp ='" + cnp + "' AND an_stud = '" + year + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    yearInfo.marksTable.Rows.Add(reader["DEN_DISC"], reader["SEM"], reader["PREZ1"], reader["PREZ2"], reader["PREZ3"], reader["NP"], reader["CREDIT"], reader["COD_DEP"]);
                }
            }

            query = "SELECT nume_fam, prenume FROM evidstud WHERE cnp ='" + cnp + "';";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                yearInfo.lastName = reader["nume_fam"].ToString();
                yearInfo.firstName = reader["prenume"].ToString();
            }

            query = "SELECT domeniu, spec, marca, statut1,sursa_fin FROM sit_sc WHERE cnp ='" + cnp + "';";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                yearInfo.domain = reader["domeniu"].ToString();
                yearInfo.specialty = reader["spec"].ToString();
                yearInfo.mark = reader["marca"].ToString();
                yearInfo.statute = reader["statut1"].ToString();
                yearInfo.financialSource = reader["sursa_fin"].ToString();
        
            }

            reader.Close();
            dbConnection.Close();
            return yearInfo;
        }
        public void PayReceipt(string cnp, string subject, string teacher)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "INSERT INTO adev VALUES ('" + cnp + "','" + subject + "','" + "','"+ teacher + "','" + DateTime.Now + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            dbConnection.Close();
        }

       
    }
}