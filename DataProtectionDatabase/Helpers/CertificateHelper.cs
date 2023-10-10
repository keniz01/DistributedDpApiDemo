using System.Security.Cryptography.X509Certificates;

namespace SecretManagement.DataProtectionDatabase.Helpers
{
    public static class CertificateHelper
    {
        public static X509Certificate2 FindCertificate(string serialNumber)
        {
            return Find(serialNumber, StoreLocation.CurrentUser) ?? Find(serialNumber, StoreLocation.LocalMachine);
        }

        public static X509Certificate2 Find(string serialNumber, StoreLocation location)
        {
            using (var store = new X509Store(location))
            {
                store.Open(OpenFlags.OpenExistingOnly);
                var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
                return certs.OfType<X509Certificate2>().First();
            }
        }
    }
}
