using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abstraction
{
    public abstract class ContactEndpointBase
    {
        public abstract string Address { get; }
        public abstract int Port { get; }
        public abstract string Path { get; }
    }
}
