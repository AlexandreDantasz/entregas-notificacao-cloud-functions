namespace Notificacoes.Services;

public interface INotificacaoService
{
  Task Enviar();
  void Inicializar(string data);
}
