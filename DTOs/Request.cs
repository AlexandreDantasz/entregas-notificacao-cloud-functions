using Notificacoes.Enums;

namespace Notificacoes.DTOs;

public class Request
{
  public required ETipoNotificacao Tipo { get; set; } 

  public required string JsonData { get; set; } = string.Empty;
}
