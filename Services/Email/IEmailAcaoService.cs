using Notificacoes.Models;
using MimeKit;

namespace Notificacoes.Services;

public interface IEmailAcaoService
{
  Task Enviar(string emailDestinatario, List<EmailConfig> configuracoes, string mensagem = "");
  Task NotificarEmailsInvalidos(List<string> emails);
}
