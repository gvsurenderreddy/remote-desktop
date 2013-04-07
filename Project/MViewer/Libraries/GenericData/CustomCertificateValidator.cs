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

        public CustomCertificateValidator(string allowedIssuerName, X509Certificate2 clientCertificate)
        {
            try
            {
                if (allowedIssuerName == null)
                {
                    throw new ArgumentNullException("allowedIssuerName not provided");
                }
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
            // Check that there is a certificate.
            if (certificate == null)
            {
                throw new ArgumentNullException("missing certificate");
            }

            // Check that the certificate issuer matches the configured issuer.
            if (_allowedIssuerName != certificate.IssuerName.Name)
            {
                throw new SecurityTokenValidationException
                  ("Certificate was not issued by a trusted issuer");
            }

            // todo: check expiration date also
            // todo: check the _clientCertificate against the provided certificate
        }
    }
}
