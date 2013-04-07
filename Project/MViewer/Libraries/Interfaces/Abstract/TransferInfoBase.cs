using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abstraction
{
    public abstract class TransferInfoBase
    {
        public abstract string FileName { get; set; }
        public abstract long FileSize { get; set; }
        public abstract bool HasPermission { get; set; }
    }
}
