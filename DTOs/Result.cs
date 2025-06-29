namespace Notificacoes.DTOs;

public class Result<T>
{
  public T? Value { get; }
  public string Error { get; }
  public bool IsOk { get; }

  protected Result(T? value, string error, bool isOk)
  {
    Value = value;
    Error = error;
    IsOk = isOk;
  }

  public static Result<T> Ok(T? value) => new Result<T>(value, string.Empty, true);
  public static Result<T> Failure(string error) => new Result<T>(default, error, false);

}
