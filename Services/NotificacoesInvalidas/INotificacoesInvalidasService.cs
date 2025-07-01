namespace Notificacoes.Services;

public interface INotificacoesInvalidasService
{
  Task PublicarListaDeNotificacoesInvalidas(List<string> notificacoesErradas);
}
