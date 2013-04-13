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
    public class CustomCertificateValidator : X509CertificateValidator
    {
        string _allowedIssuerName;
        X509Certificate2 _clientCertificate;
        X509Certificate2 _severCert;

        public CustomCertificateValidator(X509Certificate2 severCert, string allowedIssuerName, X509Certificate2 clientCertificate)
        {
            _severCert = severCert;
            if (allowedIssuerName == null)
            {
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

        public override void Validate(X509Certificate2 clientCertificate)
        {
            // Check that there is a certificate.
            if (clientCertificate == null)
            {
                throw new ArgumentNullException("missing certificate");
            }

            // the client certificate must be in your trusted certificates store
            bool validCertificate = new X509Chain().Build(clientCertificate);

            if (validCertificate)
            {
                // Check that the certificate issuer matches the configured issuer.
                if (_allowedIssuerName != clientCertificate.IssuerName.Name)
                {
                    throw new SecurityTokenValidationException
                      ("Certificate was not issued by a trusted issuer");
                }
                if (DateTime.Parse(clientCertificate.GetExpirationDateString()) < DateTime.Now)
                {
                    throw new IdentityValidationException("Certificate Expired");
                }
            }
 
            throw new SecurityTokenValidationException();

        }
    }
}
