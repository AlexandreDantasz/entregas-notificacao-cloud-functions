using Microsoft.Extensions.Logging;
using Notificacoes.Models;
using MailKit.Net.Smtp; // Para SmtpClient
using MailKit.Security; // Para SecureSocketOptions
using MimeKit;        // Para MimeMessage, TextPart, MailboxAddress
namespace Notificacoes.Services;

public class EmailCheckoutService : IEmailAcaoService
{
  private readonly ILogger _logger;
  private readonly CriptografiaService _criptografiaService;

  public EmailCheckoutService(ILogger logger, CriptografiaService criptografiaService)
  {
    _logger = logger;
    _criptografiaService = criptografiaService;
  }

  public async Task NotificarEmailsInvalidos(List<string> emails)
  {
    EmailInvalidoService emailInvalidoService = new EmailInvalidoService(_logger, _criptografiaService);
    await emailInvalidoService.PublicarListaDeNotificacoesInvalidas(emails);
  }

  public async Task Enviar(string emailDestinatario, List<EmailConfig> configuracoes, string mensagem = "")
  {
    
    List<string> emailsInvalidos = new List<string>();

    string body = $"<div style='background-color: white;'><h1>Parabéns, seu checkout foi confirmado!</h1><br><p>Sua assinatura: <img src='{mensagem}' /></p></div>";

    foreach (EmailConfig config in configuracoes)
    {
      using (SmtpClient client = new SmtpClient())
      {
        try
        {
          await client.ConnectAsync(config.ServidorDeEmail, int.Parse(config.Porta), SecureSocketOptions.SslOnConnect);

          MimeMessage mimeMessage = new MimeMessage();

          mimeMessage.From.Add(new MailboxAddress("Entregas", config.EmailRemetente));
          mimeMessage.To.Add(new MailboxAddress("Cliente", emailDestinatario));

          mimeMessage.Subject = "Seu checkout foi confirmado!";
          mimeMessage.Body = new TextPart("html")
          {
            Text = body
          };

          if (client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
            await client.AuthenticateAsync(config.EmailRemetente, config.Credenciais);

          await client.SendAsync(mimeMessage);
          
          // Se chegar aqui, o e-mail foi enviado com sucesso.
          // Agora é preciso notificar os e-mails inválidos durante esse processo

          if (emailsInvalidos.Count != 0)
            await NotificarEmailsInvalidos(emailsInvalidos);
          
          // Fechando a conexão e saindo da função

          await client.DisconnectAsync(true);
          return;
        }
        catch (Exception error)
        {
          _logger.LogError($"Falha ao enviar e-mail para {emailDestinatario} através de {config.EmailRemetente}");
          emailsInvalidos.Add(config.EmailRemetente);
        }
        finally
        { 
          // Se chegar aqui, é porque não foi possível enviar nenhum e-mail
          await client.DisconnectAsync(true);
          await NotificarEmailsInvalidos(emailsInvalidos);
        }
      }
      
    }
  }
}
