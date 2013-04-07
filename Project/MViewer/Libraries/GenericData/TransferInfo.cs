using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class TransferInfo : TransferInfoBase
    {
        public override string FileName { get; set; }
        public override long FileSize { get; set; }
        public override bool HasPermission { get; set; }
    }
}
