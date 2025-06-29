using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Services;

public class EmailCheckoutService : IEmailAcaoService
{
  private readonly ILogger _logger;

  public EmailCheckoutService(ILogger logger) =>
    _logger = logger;

  public Task Enviar(MailAddress emailRemetente, MailAddress emailDestinatario, SmtpClient smtpClient, string mensagem = "")
  {
    MailMessage mensagemEmail = new MailMessage(emailRemetente, emailDestinatario);

    mensagemEmail.Subject = "Seu checkout foi confirmado!";
    mensagemEmail.Body = $"<div style='background-color: white;'><h1>Parabéns, seu checkout foi confirmado!</h1><br><p>Sua assinatura: <img src='{mensagem}' /></p></div>";

    mensagemEmail.IsBodyHtml = true;

    try
    {
      _logger.LogInformation($"Enviando e-mail de confirmação de checkout para {emailDestinatario.Address}...");
      smtpClient.SendAsync(mensagemEmail, null);
      _logger.LogInformation($"E-mail enviado com sucesso para {emailDestinatario.Address}");
    }
    catch (SmtpException error)
    {
      _logger.LogError($"Falha ao enviar e-mail para {emailDestinatario.Address}, não foi possível se conectar ao servidor SMTP");
    }

    return Task.CompletedTask;
  }
}
