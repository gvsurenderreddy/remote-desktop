using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public interface Server
    {
        void ShareDesktop(ref Client client);

    }
}
