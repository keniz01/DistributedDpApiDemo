using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace SecretManagement.Shared
{
    public static class CyptographyHelper
    {
        public static string Encrypt(X509Certificate2 certificate, byte[] dataToEncrypt)
        {
            var rsa = certificate.GetRSAPrivateKey() ?? RSA.Create();
            var encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(X509Certificate2 certificate, byte[] dataToDecrypt)
        {
            var rsa = certificate.GetRSAPrivateKey() ?? RSA.Create();
            var decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}