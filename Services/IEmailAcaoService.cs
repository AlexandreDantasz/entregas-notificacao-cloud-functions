using System.Net;
using System.Net.Mail;

namespace Notificacoes.Services;

public interface IEmailAcaoService
{
  Task Enviar(MailAddress emailRemetente, MailAddress emailDestinatario, SmtpClient smtpClient, string mensagem = "");
}
