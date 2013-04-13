using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using Utils;

namespace GenericObjects
{
    public class ClientCertificateValidator : X509CertificateValidator
    {
        string _allowedIssuerName;

        public ClientCertificateValidator(string allowedIssuerName)
        {
            if (allowedIssuerName == null)
            {
                throw new ArgumentNullException("allowedIssuerName not provided");
            }
            try
            {
                _allowedIssuerName = allowedIssuerName;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void Validate(X509Certificate2 serverCertificate)
        {
            // Check that there is a certificate.
            if (serverCertificate == null)
            {
                throw new ArgumentNullException("missing Server certificate");
            }

            // the client certificate must be in your trusted certificates store
            bool validCertificate = new X509Chain().Build(serverCertificate);

            if (validCertificate)
            {
                // Check that the certificate issuer matches the configured issuer.
                if (_allowedIssuerName != serverCertificate.IssuerName.Name)
                {
                    throw new SecurityTokenValidationException
                      ("Server Certificate was not issued by a trusted issuer");
                }
                if (DateTime.Parse(serverCertificate.GetExpirationDateString()) < DateTime.Now)
                {
                    throw new IdentityValidationException("Server Certificate Expired");
                }
                
            }
            else
            {
                throw new SecurityTokenValidationException("X509 Validation failure. Invalid or Untrusted X509 Server Certificate");
            }
        }
    }
}

