using System.Collections.Generic;
using System.Data;


namespace WebAPI
{
    public interface IServiceAPI
    {
        int CreateNewAccount(string cnp, string email, string password, string salt);
        bool UserAuthentication(string cnp, string passsword);
        bool CreateDemand(string cnp, string addressee, string request, string mention);
        string GetSalt(string cnp);
        bool CreateCertificate(string cnp, string reason);
        DataTable GetStudentInfo(string cnp);
        DataTable GetMarks(string cnp, string year);
        bool PayReceipt(string cnp, string subject, string teacher );
    }
}