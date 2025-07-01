using Notificacoes.Enums;

namespace Notificacoes.DTOs;

public class NotificacaoInvalidaMensagem
{
  public ETipoNotificacao Tipo { get; set; }
  public List<string> DadosInvalidos { get; set; } = new List<string>();
}
