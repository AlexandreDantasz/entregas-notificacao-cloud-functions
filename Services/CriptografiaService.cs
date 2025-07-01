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
    Key = Convert.FromBase64String(
      Environment.GetEnvironmentVariable("ChaveCriptografia") ?? "O+MPn4IhYCzaLR/RGSRXw9jaIp8nbY4BaRSWMGUsN4w="
    );

    IV = Convert.FromBase64String(
      Environment.GetEnvironmentVariable("IVCriptografia") ?? "zjHnEod66pcF46+zWvhY8Q=="
    );
  }

  public Result<string> CriptografarString(string dado)
  {
    using (Aes aesAlg = Aes.Create())
    {
      aesAlg.Key = Key;
      aesAlg.IV = IV;

      ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

      // Buffer na memória (como um arquivo na RAM), recebendo os dados a serem descriptografados
      using (MemoryStream msEncrypt = new MemoryStream())
      {
        // Stream que irá descriptografar todos os dados que estejam na sua fonte (msDecrycpt) 
        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) 
          {
            // Escrevendo o dado 
            swEncrypt.Write(dado);
          }
        }

        return Result<string>.Ok(Convert.ToBase64String(msEncrypt.ToArray()));
      }

    }
    
    return Result<string>.Failure("Ocorreu um erro ao tentar criptografdar os dados");
  }

  public Result<string> DescriptografarString(string dadosCriptografados)
  {
    string dadosDescriptografados = string.Empty;

    if (dadosCriptografados is null)
    {
      dadosCriptografados = string.Empty; 
    }

    using (Aes aesAlg = Aes.Create()) 
    {
      aesAlg.Key = Key;
      aesAlg.IV = IV;

      ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

      // Buffer na memória (como um arquivo na RAM), recebendo os dados a serem descriptografados
      using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(dadosCriptografados)))
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
