using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface IMailer {
        Task SendMessageDropPasswordFromUser(string toEmail, string toName, string secretCode);
        Task SendMessageDropPasswordFromAdmin(string toEmail, string toName, string droppedPassword);
        Task SendMessageNewUserPasswordFromAdmin(string toEmail, string toName, string password);
        Task SendMessageSuccessfullRegistration(string toEmail, string toName, string password);
        Task SendMessagePasswordHasBeenChanged(string toEmail, string toName, string password);
    }
}
