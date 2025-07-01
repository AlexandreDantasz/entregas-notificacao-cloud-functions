using System.Text;
using System.Text.Json;
using MimeKit; // Para MailboxAddress
using Notificacoes.Models;
using Notificacoes.Factories;
using Notificacoes.Enums;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Services;

public class EmailService : INotificacaoService
{
  private readonly ILogger _logger;

  private string emailDestinatario;
  private string mensagem = string.Empty;
  private EAcaoEmail acao;
  private List<EmailConfig> _configuracoes = new List<EmailConfig>(); 

  private readonly CriptografiaService _criptografiaService;
 
  public EmailService(CriptografiaService criptografiaService, ILogger logger)
  {
    _logger = logger;
    _criptografiaService = criptografiaService;
  }

  public Task Enviar()
  {
    EmailAcoesFactory factoryEmailAcoes = new EmailAcoesFactory(_logger, _criptografiaService);
    IEmailAcaoService acaoService = factoryEmailAcoes.CriarServicoParaAcao(acao);

    acaoService.Enviar(emailDestinatario, _configuracoes, mensagem);

    return Task.CompletedTask;
  }

  public void Inicializar(string data)
  {
    EmailInfo emailCriptografado = JsonSerializer.Deserialize<EmailInfo>(data); 
    
    acao = emailCriptografado.AcaoEmail;
    mensagem = _criptografiaService.DescriptografarString(emailCriptografado.Mensagem).Value;
    emailDestinatario = _criptografiaService.DescriptografarString(emailCriptografado.EmailDestinatario).Value;

    foreach (EmailConfig config in emailCriptografado.Configuracoes)
    {
      EmailConfig configDescriptografada = new EmailConfig 
      {
        EmailRemetente = _criptografiaService.DescriptografarString(config.EmailRemetente).Value,
        ServidorDeEmail = _criptografiaService.DescriptografarString(config.ServidorDeEmail).Value,
        Credenciais = _criptografiaService.DescriptografarString(config.Credenciais).Value,
        Porta = _criptografiaService.DescriptografarString(config.Porta).Value
      };

      _configuracoes.Add(configDescriptografada);
    }
  }
}
