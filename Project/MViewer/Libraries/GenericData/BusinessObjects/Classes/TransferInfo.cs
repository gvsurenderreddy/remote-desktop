using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDataLayer
{
    public class TransferInfo
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public bool HasPermission { get; set; }
    }
}
