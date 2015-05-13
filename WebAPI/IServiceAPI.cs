using System.Data;


namespace WebAPI
{
    public interface IServiceAPI
    {
        int CreateNewAccount(string cnp, string email, string password);
        bool UserAuthentication(string cnp, string passsword);
        void CreateDemand(string cnp, string addressee, string request, string mention);
        void CreateCertificate(string cnp, string reason);
        DataTable GetMarks(string cnp, string year);
        void PayReceipt(string cnp, string subject, string teacher );
    }
}