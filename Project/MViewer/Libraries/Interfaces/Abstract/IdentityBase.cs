using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abstraction
{
    public abstract class IdentityBase
    {
        public abstract void UpdateFriendlyName(string newFriendlyName);
        public abstract string GenerateIdentity(string newAddress, int newPort, string newPath);

        public abstract string MyIdentity { get; }
        public abstract string FriendlyName { get; }
    }
}
