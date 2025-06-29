using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Notificacoes.DTOs;
using Notificacoes.Services;
using Notificacoes.Factories;

namespace Notificacoes;

public class Function : ICloudEventFunction<MessagePublishedData>
{
  private static CriptografiaService _criptografiaService = new CriptografiaService();

  private readonly ILogger _logger;

  public Function(ILogger<Function> logger) =>
    _logger = logger;

  public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
  {
    string stringData = data.Message?.TextData;
   
    Request req = JsonSerializer.Deserialize<Request>(stringData);

    NotificacoesFactory notificacoesFactory = new NotificacoesFactory(_criptografiaService, _logger);
    INotificacaoService notificacaoService = notificacoesFactory.CriarServicoDeNotificacao(req.Tipo);
    
    notificacaoService.Inicializar(req.JsonData);
    notificacaoService.Enviar();
    
    return Task.CompletedTask;

  }
}
