using Notificacoes.Services;
using Notificacoes.Enums;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Factories;

public class NotificacoesFactory
{
  private readonly CriptografiaService _criptografiaService;
  private readonly ILogger _logger;

  public NotificacoesFactory(CriptografiaService criptografiaService, ILogger logger) 
  {
    _criptografiaService = criptografiaService;
    _logger = logger;
  }
  public INotificacaoService CriarServicoDeNotificacao(ETipoNotificacao tipo)
  {
    switch (tipo)
    {
      case ETipoNotificacao.Email:
        return new EmailService(_criptografiaService, _logger);
    }

    return new EmailService(_criptografiaService, _logger);
  }
}

