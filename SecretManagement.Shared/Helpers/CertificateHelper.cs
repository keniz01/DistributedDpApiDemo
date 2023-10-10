using System.Security.Cryptography.X509Certificates;

namespace SecretManagement.Shared.Helpers
{
    public static class CertificateHelper
    {
        public static X509Certificate2 FindCertificate(string thumbPrint)
        {
            return Find(thumbPrint, StoreLocation.CurrentUser) ?? Find(thumbPrint, StoreLocation.LocalMachine);
        }

        public static X509Certificate2 Find(string thumbPrint, StoreLocation location)
        {
            using var store = new X509Store(location);
            store.Open(OpenFlags.OpenExistingOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, true);
            return certificates.OfType<X509Certificate2>().First();
        }
    }
}