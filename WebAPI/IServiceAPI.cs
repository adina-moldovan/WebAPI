using System.Data;


namespace WebAPI
{
    public interface IServiceAPI
    {
        int CreateNewAccount(string cnp, string email, string password);
        //DataTable GetUserDetails(string cnp);
        bool UserAuthentication(string cnp, string passsword);
       // DataTable GetDepartmentDetails();
    }
}