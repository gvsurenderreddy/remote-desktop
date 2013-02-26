using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
