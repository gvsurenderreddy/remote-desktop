using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DesktopSharing.Win32
{
    public class Win32Structures
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct IconInfo
        {
            public bool IsIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
            public Int32 Xcoord;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
            public Int32 Ycoord;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
            public IntPtr Bitmask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
            public IntPtr Color;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 X;
            public Int32 Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CursorInfo
        {
            public Int32 Size;        // Specifies the size, in bytes, of the structure. 
            public Int32 State;         // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr Handle;          // Handle to the cursor. 
            public Point Coordinates;       // A POINT structure that receives the screen coordinates of the cursor. 
        }
    }
}
