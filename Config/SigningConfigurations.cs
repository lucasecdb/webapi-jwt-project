using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace JwtTestProject.Config
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations(string certificatePath)
        {
            AsymmetricCipherKeyPair keyPair;

            using (var sr = new StreamReader(certificatePath))
            {
                var pr = new PemReader(sr);
                keyPair = (AsymmetricCipherKeyPair) pr.ReadObject();
            }

            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters) keyPair.Private);

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportParameters(rsaParams);
                Key = new RsaSecurityKey(provider);
            }

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256);
        }
    }
}