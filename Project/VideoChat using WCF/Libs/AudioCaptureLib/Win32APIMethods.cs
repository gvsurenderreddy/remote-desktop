using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AudioCaptureLib
{
    public static class Win32APIMethods
    {
        #region public static Win32API methods

        //2. Add the below API.
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        //[DllImport("winmm.dll")]
        //public static extern uint mciSendString(
        //    string command,
        //    StringBuilder returnValue,
        //    int returnLength,
        //    IntPtr winHandle);

        #endregion
    }
}
