using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class TransferInfo : TransferInfoBase
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public bool HasPermission { get; set; }
    }
}
