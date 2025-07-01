using Notificacoes.Services;
using Notificacoes.Enums;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Factories;

public class EmailAcoesFactory
{
  private readonly ILogger _logger;
  private readonly CriptografiaService _criptografiaService;

  public EmailAcoesFactory(ILogger logger, CriptografiaService criptografiaService)
  {
    _logger = logger;
    _criptografiaService = criptografiaService;
  }

  public IEmailAcaoService CriarServicoParaAcao(EAcaoEmail acao)
  {
    switch (acao)
    {
      case EAcaoEmail.CheckoutCliente:
        return new EmailCheckoutService(_logger, _criptografiaService);
    }

    return new EmailCheckoutService(_logger, _criptografiaService);
  }
}
