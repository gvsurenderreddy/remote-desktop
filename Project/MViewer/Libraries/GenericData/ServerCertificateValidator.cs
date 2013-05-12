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
    public class ServerCertificateValidator : X509CertificateValidator
    {
        string _allowedIssuerName;
        X509Certificate2 _clientCertificate;
        X509Certificate2 _severCert;

        public ServerCertificateValidator(X509Certificate2 severCert, string allowedIssuerName, X509Certificate2 clientCertificate)
        {
            _severCert = severCert;
            if (allowedIssuerName == null)
            {
                Tools.Instance.Logger.LogError("allowedIssuerName not provided for server certificate");
                throw new ArgumentNullException("allowedIssuerName not provided");
            }
            try
            {
                _clientCertificate = clientCertificate;
                _allowedIssuerName = allowedIssuerName;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public override void Validate(X509Certificate2 certificate)
        {
            Tools.Instance.Logger.LogInfo("performing Client certificate validation into ServerCertificateValidator");

            // Check that there is a certificate.
            if (certificate == null)
            {
                Tools.Instance.Logger.LogError("missing client certificate");
                throw new ArgumentNullException("missing client certificate");
            }

            // the client certificate must be in your trusted certificates store
            bool validCertificate = new X509Chain().Build(certificate);

            Tools.Instance.Logger.LogInfo("Client certificate validation: " + validCertificate.ToString());

            if (validCertificate)
            {
                // Check that the certificate issuer matches the configured issuer.
                if (_allowedIssuerName != certificate.IssuerName.Name)
                {
                    Tools.Instance.Logger.LogError("client Certificate was not issued by a trusted issuer");
                    throw new SecurityTokenValidationException
                      ("client Certificate was not issued by a trusted issuer");
                }
                if (DateTime.Parse(certificate.GetExpirationDateString()) < DateTime.Now)
                {
                    Tools.Instance.Logger.LogError("client Certificate Expired");
                    throw new IdentityValidationException("client Certificate Expired");
                }
                if (_clientCertificate.Equals(certificate) == false)
                {
                    Tools.Instance.Logger.LogError("Untrusted client Certificate");
                    throw new SecurityTokenValidationException
                      ("Untrusted client Certificate");
                }
            }
            else
            {
                Tools.Instance.Logger.LogError("Client certificate validation X509 Validation failure. Invalid or Untrusted X509 Client Certificate");
                throw new SecurityTokenValidationException("Client certificate validation X509 Validation failure. Invalid or Untrusted X509 Client Certificate");
            }


            Tools.Instance.Logger.LogInfo("Client certificate validation ended without exceptions");
        }
    }
}
