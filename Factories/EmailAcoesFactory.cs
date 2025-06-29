using Notificacoes.Services;
using Notificacoes.Enums;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Factories;

public class EmailAcoesFactory
{
  private readonly ILogger _logger;

  public EmailAcoesFactory(ILogger logger) =>
    _logger = logger;

  public IEmailAcaoService CriarServicoParaAcao(EAcaoEmail acao)
  {
    switch (acao)
    {
      case EAcaoEmail.CheckoutCliente:
        return new EmailCheckoutService(_logger);
    }

    return new EmailCheckoutService(_logger);
  }
}
