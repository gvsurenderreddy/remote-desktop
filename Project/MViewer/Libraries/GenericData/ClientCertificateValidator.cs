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
                Tools.Instance.Logger.LogError("allowedIssuerName not provided for server certificate");
                throw new ArgumentNullException("allowedIssuerName not provided for server certificate");
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

        public override void Validate(X509Certificate2 certificate)
        {
            //Tools.Instance.Logger.LogInfo("performing Server certificate validation into ClientCertificateValidator");

            // Check that there is a certificate.
            if (certificate == null)
            {
                Tools.Instance.Logger.LogError("missing Server certificate");
                throw new ArgumentNullException("missing Server certificate");
            }

            // the client certificate must be in your trusted certificates store
            bool validCertificate = new X509Chain().Build(certificate);

            Tools.Instance.Logger.LogInfo("Server certificate validation: " + validCertificate.ToString());

            if (validCertificate)
            {
                // Check that the certificate issuer matches the configured issuer.
                if (_allowedIssuerName != certificate.IssuerName.Name)
                {
                    Tools.Instance.Logger.LogError("Server Certificate was not issued by a trusted issuer");
                    throw new SecurityTokenValidationException
                      ("Server Certificate was not issued by a trusted issuer");
                }
                if (DateTime.Parse(certificate.GetExpirationDateString()) < DateTime.Now)
                {
                    Tools.Instance.Logger.LogError("Server Certificate Expired");
                    throw new IdentityValidationException("Server Certificate Expired");
                }

            }
            else
            {
                Tools.Instance.Logger.LogError("Server certificate X509 Validation failure. Invalid or Untrusted X509 Server Certificate");
                throw new SecurityTokenValidationException("Server certificate X509 Validation failure. Invalid or Untrusted X509 Server Certificate");
            }

            Tools.Instance.Logger.LogInfo("Server certificate validation ended without exceptions");
        }
    }
}

