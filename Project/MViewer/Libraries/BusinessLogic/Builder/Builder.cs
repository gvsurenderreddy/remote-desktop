using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    public abstract class Builder
    {
        public abstract void BuildCertificate();
        public abstract void BuildBinding();
        public virtual void BuildContract(){}
        public virtual void BuildUri() { }
        public virtual void BuildBehavior() { }
        public abstract object GetResult();
    }
}
