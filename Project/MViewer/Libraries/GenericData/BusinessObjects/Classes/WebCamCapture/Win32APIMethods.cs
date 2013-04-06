using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GenericObjects
{
    public static class Win32APIMethods
    {
        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        public static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, IntPtr hwndParent, int nID);

        [DllImport("user32", EntryPoint = "OpenClipboard")]
        public static extern int OpenClipboard(int hWnd);

        [DllImport("user32", EntryPoint = "EmptyClipboard")]
        public static extern int EmptyClipboard();

        [DllImport("user32", EntryPoint = "CloseClipboard")]
        public static extern int CloseClipboard();
    }
}
