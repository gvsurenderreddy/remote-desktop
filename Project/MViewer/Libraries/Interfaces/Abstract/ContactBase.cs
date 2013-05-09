using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abstraction
{
    public abstract class ContactBase
    {
        public abstract int ContactNo { get; }
        public abstract string FriendlyName { get; set; }
        public abstract string Identity { get; }
    }
}
