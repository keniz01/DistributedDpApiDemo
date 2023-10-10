using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SecretManagement.DataProtectionCertificate.Models;
using SecretManagement.Shared;

namespace SecretManagement.DataProtectionCertificate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptographyController : ControllerBase
    {
        private readonly CertificateSettings _certificateSettings;

        public CryptographyController(IOptions<CertificateSettings> certificateSettingOptions)
        {
            _certificateSettings = certificateSettingOptions.Value;
        }

        [HttpPost(Name = "encrypt")]
        public IResult Encrypt(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var certificate = CertificateHelper.FindCertificate(_certificateSettings.ThumbPrint);
            var encryptedData = CyptographyHelper.Encrypt(certificate, bytes);
            return Results.Ok(encryptedData);
        }

        [HttpPost(Name = "decrypt")]
        public IResult Decrypt(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var certificate = CertificateHelper.FindCertificate(_certificateSettings.ThumbPrint);
            var response = CyptographyHelper.Decrypt(certificate, bytes);
            return Results.Ok(response);
        }
    }
}