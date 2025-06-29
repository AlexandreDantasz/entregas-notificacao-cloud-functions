using System.Text;
using System.Text.Json;
using Notificacoes.Models;
using Notificacoes.Factories;
using Notificacoes.Enums;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Services;

public class EmailService : INotificacaoService
{
  private readonly ILogger _logger;

  private MailAddress emailRemetente;
  private MailAddress emailDestinatario;
  private string mensagem = string.Empty;
  private SmtpClient smtpClient;
  private EAcaoEmail acao;

  private readonly CriptografiaService _criptografiaService;
 
  public EmailService(CriptografiaService criptografiaService, ILogger logger)
  {
    _logger = logger;
    _criptografiaService = criptografiaService;
  }

  public Task Enviar()
  {
    EmailAcoesFactory factoryEmailAcoes = new EmailAcoesFactory(_logger);
    IEmailAcaoService acaoService = factoryEmailAcoes.CriarServicoParaAcao(acao);

    acaoService.Enviar(emailRemetente, emailDestinatario, smtpClient, mensagem);

    return Task.CompletedTask;
  }

  public void Inicializar(string data)
  {
    EmailInfo emailCriptografado = JsonSerializer.Deserialize<EmailInfo>(data); 

    EmailInfo emailDescriptografado = new EmailInfo 
    {
      AcaoEmail = emailCriptografado.AcaoEmail,
      Mensagem = _criptografiaService.DescriptografarString(emailCriptografado.Mensagem).Value,
      EmailRemetente =  _criptografiaService.DescriptografarString(emailCriptografado.EmailRemetente).Value,
      EmailDestinatario =  _criptografiaService.DescriptografarString(emailCriptografado.EmailDestinatario).Value,
      ServidorDeEmail = _criptografiaService.DescriptografarString(emailCriptografado.ServidorDeEmail).Value,
      Credenciais = _criptografiaService.DescriptografarString(emailCriptografado.Credenciais).Value,
      Porta = int.Parse(_criptografiaService.DescriptografarString(emailCriptografado.Porta.ToString()).Value)
    };

    emailRemetente = new MailAddress(emailDescriptografado.EmailRemetente);
    emailDestinatario = new MailAddress(emailDescriptografado.EmailDestinatario);

    smtpClient = new SmtpClient(emailDescriptografado.ServidorDeEmail)
    {
      Port = emailDescriptografado.Porta,
      Credentials = new NetworkCredential(emailRemetente.Address, emailDescriptografado.Credenciais),
      EnableSsl = true
    };

    acao = emailDescriptografado.AcaoEmail;
    mensagem = emailDescriptografado.Mensagem;
  }
}
