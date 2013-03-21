using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GenericDataLayer
{
    public abstract class Product
    {
        public abstract void BuildCertificate();
        public abstract void BuildClientBinding(ContactEndpoint contractEndpoint);
        public abstract void BuildServerBinding();
        public abstract void BuildContract();
        public abstract void BuildUri(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity);
        public abstract void BuildBehavior(ServiceHost svcHost);
    }
}
