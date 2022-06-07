using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OCTO.Api.Client
{
    public static class JWTAssinatura
    {
        private static SecurityKey chave;
        private static SigningCredentials credencialAssinatura;

        public static SecurityKey Chave
        {
            get
            {
                if(chave == null)
                {
                    using (var provider = new RSACryptoServiceProvider(2048))
                    {
                        chave = new RsaSecurityKey(provider.ExportParameters(true));
                    }
                }

                return chave;
            }
        }

        public static SigningCredentials CredencialAssinatura
        {
            get
            {
                if(credencialAssinatura == null)
                {
                    credencialAssinatura = new SigningCredentials(Chave, SecurityAlgorithms.RsaSha256Signature);
                }

                return credencialAssinatura;
            }
        }
    }
}
