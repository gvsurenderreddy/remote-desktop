using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public class LanguageEventArgs : EventArgs
    {
        public GenericEnums.Language Language {get; set;}
    }
}
