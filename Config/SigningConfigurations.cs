using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace JwtTestProject.Config
{
    public class SigningConfigurations : IDisposable
    {
        private RSACryptoServiceProvider _cryptoServiceProvider;
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
            
            _cryptoServiceProvider = new RSACryptoServiceProvider();

            _cryptoServiceProvider.ImportParameters(rsaParams);
            Key = new RsaSecurityKey(_cryptoServiceProvider);

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cryptoServiceProvider?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SigningConfigurations()
        {
            Dispose(false);
        }
    }
}