using Google.Cloud.PubSub.V1;
using System.Text.Json;
using Notificacoes.Enums;
using Notificacoes.DTOs;
using Microsoft.Extensions.Logging;

namespace Notificacoes.Services;

public class EmailInvalidoService : INotificacoesInvalidasService
{
  private readonly ILogger _logger;
  private readonly CriptografiaService _criptografiaService;

  public EmailInvalidoService(ILogger logger, CriptografiaService criptografiaService)
  {
    _logger = logger;
    _criptografiaService = criptografiaService;
  }

  public async Task PublicarListaDeNotificacoesInvalidas(List<string> notificacoesErradas)
  {
    string projectId = Environment.GetEnvironmentVariable("ProjectId");
    string topicId = Environment.GetEnvironmentVariable("TopicId");

    TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
    PublisherClient publisher = await PublisherClient.CreateAsync(topicName);
    
    NotificacaoInvalidaMensagem mensagem = new NotificacaoInvalidaMensagem();

    mensagem.Tipo = ETipoNotificacao.Email;

    foreach (string email in notificacoesErradas)
    {
      mensagem.DadosInvalidos.Add(_criptografiaService.CriptografarString(email).Value); 
    }

    string stringMensagem = JsonSerializer.Serialize(mensagem);

    PubsubMessage pubsubMessage = new PubsubMessage
    {
      Data = Google.Protobuf.ByteString.CopyFromUtf8(stringMensagem),
      Attributes = {  { "contentType", "application/json" }, {  "encryption", "AES" }  }
    };

    try
    {
      await publisher.PublishAsync(pubsubMessage);
    }
    catch (Exception exception)
    {
      _logger.LogError("Não foi possível publicar mensagens de e-mails inválidos"); 
    }

  }
}
