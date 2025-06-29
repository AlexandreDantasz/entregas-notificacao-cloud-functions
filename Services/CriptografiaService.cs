using System.Security.Cryptography; // para AES
using Notificacoes.DTOs;
using System.Text;

namespace Notificacoes.Services;

public class CriptografiaService
{
  private readonly byte[] Key;
  private readonly byte[] IV;

  public CriptografiaService()
  {
    Key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ChaveCriptografia"));
    IV = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("IVCriptografia"));
  }

  public Result<string> DescriptografarString(string dadosCriptografados)
  {
    string dadosDescriptografados = string.Empty;

    using (Aes aesAlg = Aes.Create()) 
    {
      aesAlg.Key = Key;
      aesAlg.IV = IV;

      ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

      // Buffer na memória (como um arquivo na RAM), recebendo os dados a serem descriptografados
      using (MemoryStream msDecrypt = new MemoryStream(Encoding.UTF8.GetBytes(dadosCriptografados)))
      {
        // Stream que irá descriptografar todos os dados que estejam na sua fonte (msDecrycpt) 
        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        {
          using (StreamReader srDecrypt = new StreamReader(csDecrypt)) 
          {
            // Lendo os dados 
            dadosDescriptografados = srDecrypt.ReadToEnd();
          }
        }
      }

      return Result<string>.Ok(dadosDescriptografados);
    }
    
    return Result<string>.Failure("Ocorreu um erro ao tentar descriptografar os dados");
  }
}
