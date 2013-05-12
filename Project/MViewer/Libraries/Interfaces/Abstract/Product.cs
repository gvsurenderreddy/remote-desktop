using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace GenericObjects
{
    public abstract class Product
    {
        public virtual void BuildCertificate(){}
        public virtual void BuildClientBinding(ContactEndpointBase contractEndpoint){}
        public virtual void BuildServerBinding(bool isSecured) { }
        public virtual void BuildContract(){}
        public virtual void BuildUri(string httpsAddress, ControllerEventHandlers controllerHandlers, string identity) { }
        public virtual void BuildBehavior(ServiceHost svcHost) { }
    }
}
