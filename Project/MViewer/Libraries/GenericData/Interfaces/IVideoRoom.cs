using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace GenericDataLayer
{
    public interface IVideoRoom : IRoom
    {
        void SetPicture(Image picture);

        IntPtr FormHandle
        {
            get;
        }
    }
}
