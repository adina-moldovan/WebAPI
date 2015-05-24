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

        public int CreateNewAccount(string cnp, string email, string password, string salt)
        {
            const int INVALIDCNP = 0;
            const int ALREADYEXISTS = 1;
            const int SUCCESS = 2;

            int registerState = INVALIDCNP;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT CNP FROM Utiliz WHERE CNP='" + cnp + "';";
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
                query = "INSERT INTO Utiliz VALUES ('" + cnp + "','" + email + "','" + password + "','" + salt + "');";
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

            string query = "SELECT CNP FROM Utiliz WHERE CNP='" + cnp + "' AND parola='" + passsword + "';";
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

        public bool CreateDemand(string cnp, string addressee, string request, string mention)
        {
            bool demandCreated = false;

            string query = "INSERT INTO cereri VALUES ('" + cnp + "','" + addressee + "', '" + request + "','" + mention + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "');";
            SqlCommand command = new SqlCommand(query, dbConnection);

            if (command.Connection.State.ToString() == "Closed")
            {
                command.Connection.Open();
            }

            if (command.ExecuteNonQuery() == 1)
            {
                demandCreated = true;
            }
           
            command.Connection.Close();

            return demandCreated;
        }

        public string GetSalt(string cnp)
        {
            string salt = "";

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT salt FROM Utiliz WHERE cnp ='" + cnp + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                if (reader.HasRows)
                {
                    salt = reader["salt"].ToString();
                }
            }

            reader.Close();
            dbConnection.Close();
            return salt;
        }
       
            public bool CreateCertificate(string cnp, string reason)
        {
            bool certificateCreated = false;

            string query = "INSERT INTO adev VALUES ('" + cnp + "','" + reason + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "');";
            SqlCommand command = new SqlCommand(query, dbConnection);

            if (command.Connection.State.ToString() == "Closed")
            {
                command.Connection.Open();
            }

            if (command.ExecuteNonQuery() == 1)
            {
                certificateCreated = true;
            }

            command.Connection.Close();

            return certificateCreated;
        }

        public DataTable GetStudentInfo(string cnp)
        {
            var studentInfo = new DataTable();

            studentInfo.Columns.Add(new DataColumn("yearOfStudy", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("lastName", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("firstName", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("domain", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("specialty", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("mark", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("statute", typeof(String)));
            studentInfo.Columns.Add(new DataColumn("financialSource", typeof(String)));

            DataRow row = studentInfo.NewRow();
            
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            
            string query = "SELECT nume_fam, prenume FROM evidstud WHERE cnp ='" + cnp + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                if (reader.HasRows)
                {
                    row["lastName"] = reader["nume_fam"].ToString();
                    row["firstName"] = reader["prenume"].ToString();
                }
            }
            query = "SELECT domeniu, spec, marca, an_stud, statut1,sursa_fin FROM sit_sc WHERE cnp ='" + cnp + "';";
            command = new SqlCommand(query, dbConnection);
            reader = command.ExecuteReader();

            if (reader.Read())
            {
                if (reader.HasRows)
                {
                    row["domain"] = reader["domeniu"].ToString();
                    row["specialty"] = reader["spec"].ToString();
                    row["mark"] = reader["marca"].ToString();
                    row["yearOfStudy"] = reader["an_stud"].ToString();
                    row["statute"] = reader["statut1"].ToString();
                    row["financialSource"] = reader["sursa_fin"].ToString();

                    studentInfo.Rows.Add(row);
                }
            }

            reader.Close();
            dbConnection.Close();
            return studentInfo;
        }
       
        public DataTable GetMarks(string cnp, string year)
        {
            var marks = new DataTable();

            marks.Columns.Add(new DataColumn("subjectName", typeof(String)));
            marks.Columns.Add(new DataColumn("semester", typeof(String)));
            marks.Columns.Add(new DataColumn("attempt1", typeof(String)));
            marks.Columns.Add(new DataColumn("attempt2", typeof(String)));
            marks.Columns.Add(new DataColumn("attempt3", typeof(String)));
            marks.Columns.Add(new DataColumn("activityGrade", typeof(String)));
            marks.Columns.Add(new DataColumn("credits", typeof(String)));
            marks.Columns.Add(new DataColumn("department", typeof(String)));

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
                    marks.Rows.Add(reader["DEN_DISC"], reader["SEM"], reader["PREZ1"], reader["PREZ2"], reader["PREZ3"], reader["NP"], reader["CREDIT"], reader["COD_DEP"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return marks;
        }
        
        public bool PayReceipt(string cnp, string subject, string teacher)
        {
            bool paymentSucceed = false;

            string query = "INSERT INTO chitante VALUES ('" + cnp + "','" + subject + "','" + "','" + teacher + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "');";
            SqlCommand command = new SqlCommand(query, dbConnection);

            if (command.Connection.State.ToString() == "Closed")
            {
                command.Connection.Open();
            }

            if (command.ExecuteNonQuery() == 1)
            {
                paymentSucceed = true;
            }

            command.Connection.Close();

            return paymentSucceed;
        }

       
    }
}