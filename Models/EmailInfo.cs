using Notificacoes.Enums;

namespace Notificacoes.Models;

public class EmailConfig
{
  public required string EmailRemetente { get; set; } = string.Empty;
  public required string ServidorDeEmail { get; set; } = string.Empty;
  public required string Credenciais { get; set; } = string.Empty;
  public required string Porta { get; set; }

}

public class EmailInfo : NotificacaoBase
{
  public EAcaoEmail AcaoEmail { get; set; }
  public required string EmailDestinatario { get; set; } = string.Empty;
  public List<EmailConfig> Configuracoes { get; set; } = new List<EmailConfig>();
}
