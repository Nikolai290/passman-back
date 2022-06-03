using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using passman_back.Business.Interfaces.Services;
using passman_back.Infrastructure.Business.Settigns;
using System.Linq;
using System.Threading.Tasks;

namespace passman_back.Infrastructure.Business.MailService {
    public class Mailer : IMailer {
        private MailSettings mailSettings;
        private PassmanSettings passmanSettings;

        public Mailer(
            IOptions<MailSettings> mailSettings,
            IOptions<PassmanSettings> passmanSettings
        ) {
            this.mailSettings = mailSettings.Value.GetSettings();
            this.passmanSettings = passmanSettings.Value;
        }

        public async Task SendMessageDropPasswordFromUser(string toEmail, string toName, string secretCode) {
            var HtmlBody = @$"
                <div>
                  <p>
                    Начата процедура смены пароля.<br />
                    Время действия кода 1 час.
                  </p>
                  <p>
                    Для восстановления пароля пройдите по ссылке
                    <a href='{passmanSettings.GetFrontendUrl()}/new-password?key={secretCode}'>ссылке</a>
                  </p>
                </div>
                ";
            await SendMessageWithHtmlBody(toEmail, toName, HtmlBody);
        }

        public async Task SendMessageDropPasswordFromAdmin(string toEmail, string toName, string droppedPassword) {
            var HtmlBody = @$"
                <div>
                    <p>
                        Администратор сервиса Passman сбросил пароль для Вашей учётной записи.
                        <br />
                        Произведите вход по новому паролю
                    </p>
                    <p>Ваш новый пароль: <strong>{droppedPassword}</strong></p>
                    <p>
                        Для авторизации пройдите по
                        <a href='{passmanSettings.GetFrontendUrl()}/authorization'>ссылке</a>
                    </p>
                </div>
                ";
            await SendMessageWithHtmlBody(toEmail, toName, HtmlBody);
        }

        public async Task SendMessageNewUserPasswordFromAdmin(string toEmail, string toName, string password) {
            var HtmlBody = @$"
                <div>
                  <p>
                    Для Вас создана учётная запись в приложении Passman.
                    <br />
                    После первого входа обязательно смените пароль.
                  </p>
                  <p>Логин: <strong>{toName}</strong></p>
                  <p>Пароль: <strong>{password}</strong></p>
                  <p>
                    Для авторизации пройдите по
                    <a href='{passmanSettings.GetFrontendUrl()}/authorization'>ссылке</a>
                  </p>
                </div>
                ";
            await SendMessageWithHtmlBody(toEmail, toName, HtmlBody);
        }

        public async Task SendMessageSuccessfullRegistration(string toEmail, string toName, string password) {
            var HtmlBody = @$"
                <div>
                    <p>Вы успешно зарегистрировались в приложении Passman.</p>
                    <p>Логин: <strong>{toName}</strong></p>
                    <p>Пароль: <strong>{password}</strong></p>
                    <p>
                        Для авторизации пройдите по
                        <a href='{passmanSettings.GetFrontendUrl()}/authorization'>ссылке</a>
                    </p>
                </div>
                ";
            await SendMessageWithHtmlBody(toEmail, toName, HtmlBody);
        }

        public async Task SendMessagePasswordHasBeenChanged(string toEmail, string toName, string password) {
            var HtmlBody = @$"
                <div>
                    <p>Пароль успешно изменён!</p>
                    <p>Ваш новый пароль: <strong>{password}</strong></p>
                    <p>
                        Для авторизации пройдите по
                        <a href='{passmanSettings.GetFrontendUrl()}/authorization'>ссылке</a>
                    </p>
                </div>
                ";
            await SendMessageWithHtmlBody(toEmail, toName, HtmlBody);
        }

        private async Task SendMessageWithHtmlBody(string toEmail, string toName, string HtmlBody) {
            var email = mailSettings.Email;
            var name = email.Split("@").First();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(name, email));
            message.To.Add(new MailboxAddress(toName, toEmail));

            message.Subject = "Noreply: drop password";
            var body = new BodyBuilder() { HtmlBody = HtmlBody };


            message.Body = body.ToMessageBody();
            await SendMessage(message);
        }

        private async Task SendMessage(MimeMessage message) {
            using (var client = new SmtpClient()) {
                client.Connect(mailSettings.Host, mailSettings.Port, true);
                client.Authenticate(mailSettings.Email, mailSettings.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}