using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsInput
{
    public class KeyCodeParser
    {
        /// <summary>
        /// method used to convert keycode to virtual keycode
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public VirtualKeyCode ParseKeyCode(Keys keyCode)
        {
            VirtualKeyCode virtualKeyCode = VirtualKeyCode.Undefined;
            switch (keyCode)
            {
                case Keys.LButton:
                    virtualKeyCode = VirtualKeyCode.LBUTTON;
                    break;
                case Keys.RButton:
                    virtualKeyCode = VirtualKeyCode.RBUTTON;
                    break;
                case Keys.Cancel:
                    virtualKeyCode = VirtualKeyCode.CANCEL;
                    break;
                case Keys.MButton:
                    virtualKeyCode = VirtualKeyCode.MBUTTON;
                    break;
                case Keys.XButton1:
                    virtualKeyCode = VirtualKeyCode.XBUTTON1;
                    break;
                case Keys.XButton2:
                    virtualKeyCode = VirtualKeyCode.XBUTTON2;
                    break;
                case Keys.Back:
                    virtualKeyCode = VirtualKeyCode.BACK;
                    break;
                case Keys.Tab:
                    virtualKeyCode = VirtualKeyCode.TAB;
                    break;
                case Keys.Clear:
                    virtualKeyCode = VirtualKeyCode.CLEAR;
                    break;
                case Keys.Enter:
                    virtualKeyCode = VirtualKeyCode.ENTER;
                    break;
                case Keys.ShiftKey:
                    virtualKeyCode = VirtualKeyCode.SHIFTKEY;
                    break;
                case Keys.ControlKey:
                    virtualKeyCode = VirtualKeyCode.CONTROLKEY;
                    break;
                case Keys.Menu:
                    virtualKeyCode = VirtualKeyCode.Alt;
                    break;
                case Keys.Pause:
                    virtualKeyCode = VirtualKeyCode.PAUSE;
                    break;
                case Keys.CapsLock:
                    virtualKeyCode = VirtualKeyCode.CapsLock;
                    break;
                //case Keys.KanaMode:
                //    virtualKeyCode = VirtualKeyCode.KanaMode;
                //    break;
                //case Keys.HanguelMode:
                //virtualKeyCode = VirtualKeyCode.HANGEUL;
                //break;
                case Keys.HangulMode:
                    virtualKeyCode = VirtualKeyCode.HANGUL;
                    break;
                case Keys.JunjaMode:
                    virtualKeyCode = VirtualKeyCode.JUNJA;
                    break;
                case Keys.FinalMode:
                    virtualKeyCode = VirtualKeyCode.FINAL;
                    break;
                case Keys.KanjiMode:
                    virtualKeyCode = VirtualKeyCode.KANJI;
                    break;
                case Keys.Escape:
                    virtualKeyCode = VirtualKeyCode.ESCAPE;
                    break;
                case Keys.IMEConvert:
                    virtualKeyCode = VirtualKeyCode.CONVERT;
                    break;
                case Keys.IMENonconvert:
                    virtualKeyCode = VirtualKeyCode.NONCONVERT;
                    break;
                case Keys.IMEAccept:
                    virtualKeyCode = VirtualKeyCode.ACCEPT;
                    break;
                case Keys.IMEModeChange:
                    virtualKeyCode = VirtualKeyCode.MODECHANGE;
                    break;
                case Keys.Space:
                    virtualKeyCode = VirtualKeyCode.SPACE;
                    break;
                case Keys.PageUp:
                    virtualKeyCode = VirtualKeyCode.PageUp;
                    break;
                case Keys.PageDown:
                    virtualKeyCode = VirtualKeyCode.PageDOWN;
                    break;
                case Keys.End:
                    virtualKeyCode = VirtualKeyCode.END;
                    break;
                case Keys.Home:
                    virtualKeyCode = VirtualKeyCode.HOME;
                    break;
                case Keys.Left:
                    virtualKeyCode = VirtualKeyCode.LEFT;
                    break;
                case Keys.Up:
                    virtualKeyCode = VirtualKeyCode.UP;
                    break;
                case Keys.Right:
                    virtualKeyCode = VirtualKeyCode.RIGHT;
                    break;
                case Keys.Down:
                    virtualKeyCode = VirtualKeyCode.DOWN;
                    break;
                case Keys.Select:
                    virtualKeyCode = VirtualKeyCode.SELECT;
                    break;
                case Keys.Print:
                    virtualKeyCode = VirtualKeyCode.PRINT;
                    break;
                case Keys.Execute:
                    virtualKeyCode = VirtualKeyCode.EXECUTE;
                    break;
                case Keys.Snapshot:
                    virtualKeyCode = VirtualKeyCode.SNAPSHOT;
                    break;
                case Keys.Insert:
                    virtualKeyCode = VirtualKeyCode.INSERT;
                    break;
                case Keys.Delete:
                    virtualKeyCode = VirtualKeyCode.DELETE;
                    break;
                case Keys.Help:
                    virtualKeyCode = VirtualKeyCode.HELP;
                    break;
                case Keys.D0:
                    virtualKeyCode = VirtualKeyCode.VK_0;
                    break;
                case Keys.D1:
                    virtualKeyCode = VirtualKeyCode.VK_1;
                    break;
                case Keys.D2:
                    virtualKeyCode = VirtualKeyCode.VK_2;
                    break;
                case Keys.D3:
                    virtualKeyCode = VirtualKeyCode.VK_3;
                    break;
                case Keys.D4:
                    virtualKeyCode = VirtualKeyCode.VK_4;
                    break;
                case Keys.D5:
                    virtualKeyCode = VirtualKeyCode.VK_5;
                    break;
                case Keys.D6:
                    virtualKeyCode = VirtualKeyCode.VK_6;
                    break;
                case Keys.D7:
                    virtualKeyCode = VirtualKeyCode.VK_7;
                    break;
                case Keys.D8:
                    virtualKeyCode = VirtualKeyCode.VK_8;
                    break;
                case Keys.D9:
                    virtualKeyCode = VirtualKeyCode.VK_9;
                    break;
                case Keys.A:
                    virtualKeyCode = VirtualKeyCode.VK_A;
                    break;
                case Keys.B:
                    virtualKeyCode = VirtualKeyCode.VK_B;
                    break;
                case Keys.C:
                    virtualKeyCode = VirtualKeyCode.VK_C;
                    break;
                case Keys.D:
                    virtualKeyCode = VirtualKeyCode.VK_D;
                    break;
                case Keys.E:
                    virtualKeyCode = VirtualKeyCode.VK_E;
                    break;
                case Keys.F:
                    virtualKeyCode = VirtualKeyCode.VK_F;
                    break;
                case Keys.G:
                    virtualKeyCode = VirtualKeyCode.VK_G;
                    break;
                case Keys.H:
                    virtualKeyCode = VirtualKeyCode.VK_H;
                    break;
                case Keys.I:
                    virtualKeyCode = VirtualKeyCode.VK_I;
                    break;
                case Keys.J:
                    virtualKeyCode = VirtualKeyCode.VK_J;
                    break;
                case Keys.K:
                    virtualKeyCode = VirtualKeyCode.VK_K;
                    break;
                case Keys.L:
                    virtualKeyCode = VirtualKeyCode.VK_L;
                    break;
                case Keys.M:
                    virtualKeyCode = VirtualKeyCode.VK_M;
                    break;
                case Keys.N:
                    virtualKeyCode = VirtualKeyCode.VK_N;
                    break;
                case Keys.O:
                    virtualKeyCode = VirtualKeyCode.VK_O;
                    break;
                case Keys.P:
                    virtualKeyCode = VirtualKeyCode.VK_P;
                    break;
                case Keys.Q:
                    virtualKeyCode = VirtualKeyCode.VK_Q;
                    break;
                case Keys.R:
                    virtualKeyCode = VirtualKeyCode.VK_R;
                    break;
                case Keys.S:
                    virtualKeyCode = VirtualKeyCode.VK_S;
                    break;
                case Keys.T:
                    virtualKeyCode = VirtualKeyCode.VK_T;
                    break;
                case Keys.U:
                    virtualKeyCode = VirtualKeyCode.VK_U;
                    break;
                case Keys.V:
                    virtualKeyCode = VirtualKeyCode.VK_V;
                    break;
                case Keys.W:
                    virtualKeyCode = VirtualKeyCode.VK_W;
                    break;
                case Keys.X:
                    virtualKeyCode = VirtualKeyCode.VK_X;
                    break;
                case Keys.Y:
                    virtualKeyCode = VirtualKeyCode.VK_Y;
                    break;
                case Keys.Z:
                    virtualKeyCode = VirtualKeyCode.VK_Z;
                    break;
                case Keys.LWin:
                    virtualKeyCode = VirtualKeyCode.LWIN;
                    break;
                case Keys.RWin:
                    virtualKeyCode = VirtualKeyCode.RWIN;
                    break;
                case Keys.Apps:
                    virtualKeyCode = VirtualKeyCode.APPS;
                    break;
                case Keys.Sleep:
                    virtualKeyCode = VirtualKeyCode.SLEEP;
                    break;
                case Keys.NumPad0:
                    virtualKeyCode = VirtualKeyCode.NUMPAD0;
                    break;
                case Keys.NumPad1:
                    virtualKeyCode = VirtualKeyCode.NUMPAD1;
                    break;
                case Keys.NumPad2:
                    virtualKeyCode = VirtualKeyCode.NUMPAD2;
                    break;
                case Keys.NumPad3:
                    virtualKeyCode = VirtualKeyCode.NUMPAD3;
                    break;
                case Keys.NumPad4:
                    virtualKeyCode = VirtualKeyCode.NUMPAD4;
                    break;
                case Keys.NumPad5:
                    virtualKeyCode = VirtualKeyCode.NUMPAD5;
                    break;
                case Keys.NumPad6:
                    virtualKeyCode = VirtualKeyCode.NUMPAD6;
                    break;
                case Keys.NumPad7:
                    virtualKeyCode = VirtualKeyCode.NUMPAD7;
                    break;
                case Keys.NumPad8:
                    virtualKeyCode = VirtualKeyCode.NUMPAD8;
                    break;
                case Keys.NumPad9:
                    virtualKeyCode = VirtualKeyCode.NUMPAD9;
                    break;
                case Keys.Multiply:
                    virtualKeyCode = VirtualKeyCode.MULTIPLY;
                    break;
                case Keys.Add:
                    virtualKeyCode = VirtualKeyCode.ADD;
                    break;
                case Keys.Separator:
                    virtualKeyCode = VirtualKeyCode.SEPARATOR;
                    break;
                case Keys.Subtract:
                    virtualKeyCode = VirtualKeyCode.SUBTRACT;
                    break;
                case Keys.Decimal:
                    virtualKeyCode = VirtualKeyCode.DECIMAL;
                    break;
                case Keys.Divide:
                    virtualKeyCode = VirtualKeyCode.DIVIDE;
                    break;
                case Keys.F1:
                    virtualKeyCode = VirtualKeyCode.F1;
                    break;
                case Keys.F2:
                    virtualKeyCode = VirtualKeyCode.F2;
                    break;
                case Keys.F3:
                    virtualKeyCode = VirtualKeyCode.F3;
                    break;
                case Keys.F4:
                    virtualKeyCode = VirtualKeyCode.F4;
                    break;
                case Keys.F5:
                    virtualKeyCode = VirtualKeyCode.F5;
                    break;
                case Keys.F6:
                    virtualKeyCode = VirtualKeyCode.F6;
                    break;
                case Keys.F7:
                    virtualKeyCode = VirtualKeyCode.F7;
                    break;
                case Keys.F8:
                    virtualKeyCode = VirtualKeyCode.F8;
                    break;
                case Keys.F9:
                    virtualKeyCode = VirtualKeyCode.F9;
                    break;
                case Keys.F10:
                    virtualKeyCode = VirtualKeyCode.F10;
                    break;
                case Keys.F11:
                    virtualKeyCode = VirtualKeyCode.F11;
                    break;
                case Keys.F12:
                    virtualKeyCode = VirtualKeyCode.F12;
                    break;
                case Keys.F13:
                    virtualKeyCode = VirtualKeyCode.F13;
                    break;
                case Keys.F14:
                    virtualKeyCode = VirtualKeyCode.F14;
                    break;
                case Keys.F15:
                    virtualKeyCode = VirtualKeyCode.F15;
                    break;
                case Keys.F16:
                    virtualKeyCode = VirtualKeyCode.F16;
                    break;
                case Keys.F17:
                    virtualKeyCode = VirtualKeyCode.F17;
                    break;
                case Keys.F18:
                    virtualKeyCode = VirtualKeyCode.F18;
                    break;
                case Keys.F19:
                    virtualKeyCode = VirtualKeyCode.F19;
                    break;
                case Keys.F20:
                    virtualKeyCode = VirtualKeyCode.F20;
                    break;
                case Keys.F21:
                    virtualKeyCode = VirtualKeyCode.F21;
                    break;
                case Keys.F22:
                    virtualKeyCode = VirtualKeyCode.F22;
                    break;
                case Keys.F23:
                    virtualKeyCode = VirtualKeyCode.F23;
                    break;
                case Keys.F24:
                    virtualKeyCode = VirtualKeyCode.F24;
                    break;
                case Keys.NumLock:
                    virtualKeyCode = VirtualKeyCode.NUMLOCK;
                    break;
                case Keys.Scroll:
                    virtualKeyCode = VirtualKeyCode.SCROLL;
                    break;
                case Keys.LShiftKey:
                    virtualKeyCode = VirtualKeyCode.LSHIFT;
                    break;
                case Keys.RShiftKey:
                    virtualKeyCode = VirtualKeyCode.RSHIFT;
                    break;
                case Keys.LControlKey:
                    virtualKeyCode = VirtualKeyCode.LCONTROL;
                    break;
                case Keys.RControlKey:
                    virtualKeyCode = VirtualKeyCode.RCONTROL;
                    break;
                case Keys.LMenu:
                    virtualKeyCode = VirtualKeyCode.LMENU;
                    break;
                case Keys.RMenu:
                    virtualKeyCode = VirtualKeyCode.RMENU;
                    break;
                case Keys.BrowserBack:
                    virtualKeyCode = VirtualKeyCode.BROWSER_BACK;
                    break;
                case Keys.BrowserForward:
                    virtualKeyCode = VirtualKeyCode.BROWSER_FORWARD;
                    break;
                case Keys.BrowserRefresh:
                    virtualKeyCode = VirtualKeyCode.BROWSER_REFRESH;
                    break;
                case Keys.BrowserStop:
                    virtualKeyCode = VirtualKeyCode.BROWSER_STOP;
                    break;
                case Keys.BrowserSearch:
                    virtualKeyCode = VirtualKeyCode.BROWSER_SEARCH;
                    break;
                case Keys.BrowserFavorites:
                    virtualKeyCode = VirtualKeyCode.BROWSER_FAVORITES;
                    break;
                case Keys.BrowserHome:
                    virtualKeyCode = VirtualKeyCode.BROWSER_HOME;
                    break;
                case Keys.VolumeMute:
                    virtualKeyCode = VirtualKeyCode.VOLUME_MUTE;
                    break;
                case Keys.VolumeDown:
                    virtualKeyCode = VirtualKeyCode.VOLUME_DOWN;
                    break;
                case Keys.VolumeUp:
                    virtualKeyCode = VirtualKeyCode.VOLUME_UP;
                    break;
                case Keys.MediaNextTrack:
                    virtualKeyCode = VirtualKeyCode.MEDIA_NEXT_TRACK;
                    break;
                case Keys.MediaPreviousTrack:
                    virtualKeyCode = VirtualKeyCode.MEDIA_PREV_TRACK;
                    break;
                case Keys.MediaStop:
                    virtualKeyCode = VirtualKeyCode.MEDIA_STOP;
                    break;
                case Keys.MediaPlayPause:
                    virtualKeyCode = VirtualKeyCode.MEDIA_PLAY_PAUSE;
                    break;
                case Keys.LaunchMail:
                    virtualKeyCode = VirtualKeyCode.LAUNCH_MAIL;
                    break;
                case Keys.SelectMedia:
                    virtualKeyCode = VirtualKeyCode.MEDIA_SELECT;
                    break;
                case Keys.LaunchApplication1:
                    virtualKeyCode = VirtualKeyCode.LAUNCH_APP1;
                    break;
                case Keys.LaunchApplication2:
                    virtualKeyCode = VirtualKeyCode.LAUNCH_APP2;
                    break;
                case Keys.Oem1:
                    virtualKeyCode = VirtualKeyCode.OEM_1;
                    break;
                case Keys.Oemplus:
                    virtualKeyCode = VirtualKeyCode.OEM_PLUS;
                    break;
                case Keys.Oemcomma:
                    virtualKeyCode = VirtualKeyCode.OEM_COMMA;
                    break;
                case Keys.OemMinus:
                    virtualKeyCode = VirtualKeyCode.OEM_MINUS;
                    break;
                case Keys.OemPeriod:
                    virtualKeyCode = VirtualKeyCode.OEM_PERIOD;
                    break;
                case Keys.Oem2:
                    virtualKeyCode = VirtualKeyCode.OEM_2;
                    break;
                case Keys.Oem3:
                    virtualKeyCode = VirtualKeyCode.OEM_3;
                    break;
                case Keys.Oem4:
                    virtualKeyCode = VirtualKeyCode.OEM_4;
                    break;
                case Keys.Oem5:
                    virtualKeyCode = VirtualKeyCode.OEM_5;
                    break;
                case Keys.Oem6:
                    virtualKeyCode = VirtualKeyCode.OEM_6;
                    break;
                case Keys.Oem7:
                    virtualKeyCode = VirtualKeyCode.OEM_7;
                    break;
                case Keys.Oem8:
                    virtualKeyCode = VirtualKeyCode.OEM_8;
                    break;
                case Keys.Oem102:
                    virtualKeyCode = VirtualKeyCode.OEM_102;
                    break;
                case Keys.ProcessKey:
                    virtualKeyCode = VirtualKeyCode.PROCESSKEY;
                    break;
                case Keys.Packet:
                    virtualKeyCode = VirtualKeyCode.PACKET;
                    break;
                case Keys.Attn:
                    virtualKeyCode = VirtualKeyCode.ATTN;
                    break;
                case Keys.Crsel:
                    virtualKeyCode = VirtualKeyCode.CRSEL;
                    break;
                case Keys.Exsel:
                    virtualKeyCode = VirtualKeyCode.EXSEL;
                    break;
                case Keys.EraseEof:
                    virtualKeyCode = VirtualKeyCode.EraseEof;
                    break;
                case Keys.Play:
                    virtualKeyCode = VirtualKeyCode.PLAY;
                    break;
                case Keys.Zoom:
                    virtualKeyCode = VirtualKeyCode.ZOOM;
                    break;
                case Keys.NoName:
                    virtualKeyCode = VirtualKeyCode.NONAME;
                    break;
                case Keys.Pa1:
                    virtualKeyCode = VirtualKeyCode.PA1;
                    break;
                case Keys.OemClear:
                    virtualKeyCode = VirtualKeyCode.OEM_CLEAR;
                    break;
                case Keys.KeyCode:
                    virtualKeyCode = VirtualKeyCode.KeyCode;
                    break;
                case Keys.Shift:
                    virtualKeyCode = VirtualKeyCode.SHIFTKEY;
                    break;
                case Keys.Control:
                    virtualKeyCode = VirtualKeyCode.CONTROLKEY;
                    break;
                case Keys.Alt:
                    virtualKeyCode = VirtualKeyCode.Alt;
                    break;

            }

            return virtualKeyCode;
        }
    }

    /// <summary>
    /// list of VirtualKeyCodes: http://msdn.microsoft.com/en-us/library/ms645540(VS.85).aspx
    /// </summary>
    public enum VirtualKeyCode : ushort 
    {
        Undefined = 42634,

        KeyCode = 65535,

        /// <summary>
        /// Left mouse button
        /// </summary>
        LBUTTON = 0x01,

        /// <summary>
        /// Right mouse button
        /// </summary>
        RBUTTON = 0x02,

        /// <summary>
        /// Control-break processing
        /// </summary>
        CANCEL = 0x03,

        /// <summary>
        /// Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
        /// </summary>
        MBUTTON = 0x04,

        /// <summary>
        /// Windows 2000/XP: X1 mouse button - NOT contiguous with LBUTTON and RBUTTON
        /// </summary>
        XBUTTON1 = 0x05,

        /// <summary>
        /// Windows 2000/XP: X2 mouse button - NOT contiguous with LBUTTON and RBUTTON
        /// </summary>
        XBUTTON2 = 0x06,

        // 0x07 : Undefined

        /// <summary>
        /// BACKSPACE key
        /// </summary>
        BACK = 0x08,

        /// <summary>
        /// TAB key
        /// </summary>
        TAB = 0x09,

        // 0x0A - 0x0B : Reserved

        /// <summary>
        /// CLEAR key
        /// </summary>
        CLEAR = 0x0C,

        /// <summary>
        /// ENTER key
        /// </summary>
        ENTER = 0x0D,

        // 0x0E - 0x0F : Undefined

        /// <summary>
        /// SHIFT key
        /// </summary>
        SHIFTKEY = 0x10,

        /// <summary>
        /// CTRL key
        /// </summary>
        CONTROLKEY = 0x11,

        /// <summary>
        /// ALT key
        /// </summary>
        Alt = 0x12,

        /// <summary>
        /// PAUSE key
        /// </summary>
        PAUSE = 0x13,

        /// <summary>
        /// CAPS LOCK key
        /// </summary>
        CapsLock = 0x14,

        /// <summary>
        /// IME Hanguel mode (maintained for compatibility; use HANGUL)
        /// </summary>
        HANGEUL = 0x15,

        /// <summary>
        /// IME Hangul mode
        /// </summary>
        HANGUL = 0x15,

        // 0x16 : Undefined

        /// <summary>
        /// IME Junja mode
        /// </summary>
        JUNJA = 0x17,

        /// <summary>
        /// IME final mode
        /// </summary>
        FINAL = 0x18,

        /// <summary>
        /// IME Hanja mode
        /// </summary>
        HANJA = 0x19,

        /// <summary>
        /// IME Kanji mode
        /// </summary>
        KANJI = 0x19,

        // 0x1A : Undefined

        /// <summary>
        /// ESC key
        /// </summary>
        ESCAPE = 0x1B,

        /// <summary>
        /// IME convert
        /// </summary>
        CONVERT = 0x1C,

        /// <summary>
        /// IME nonconvert
        /// </summary>
        NONCONVERT = 0x1D,

        /// <summary>
        /// IME accept
        /// </summary>
        ACCEPT = 0x1E,

        /// <summary>
        /// IME mode change request
        /// </summary>
        MODECHANGE = 0x1F,

        /// <summary>
        /// SPACEBAR
        /// </summary>
        SPACE = 0x20,

        /// <summary>
        /// PAGE UP key
        /// </summary>
        PageUp = 0x21,

        /// <summary>
        /// PAGE DOWN key
        /// </summary>
        PageDOWN = 0x22,

        /// <summary>
        /// END key
        /// </summary>
        END = 0x23,

        /// <summary>
        /// HOME key
        /// </summary>
        HOME = 0x24,

        /// <summary>
        /// LEFT ARROW key
        /// </summary>
        LEFT = 0x25,

        /// <summary>
        /// UP ARROW key
        /// </summary>
        UP = 0x26,

        /// <summary>
        /// RIGHT ARROW key
        /// </summary>
        RIGHT = 0x27,

        /// <summary>
        /// DOWN ARROW key
        /// </summary>
        DOWN = 0x28,

        /// <summary>
        /// SELECT key
        /// </summary>
        SELECT = 0x29,

        /// <summary>
        /// PRINT key
        /// </summary>
        PRINT = 0x2A,

        /// <summary>
        /// EXECUTE key
        /// </summary>
        EXECUTE = 0x2B,

        /// <summary>
        /// PRINT SCREEN key
        /// </summary>
        SNAPSHOT = 0x2C,

        /// <summary>
        /// INS key
        /// </summary>
        INSERT = 0x2D,

        /// <summary>
        /// DEL key
        /// </summary>
        DELETE = 0x2E,

        /// <summary>
        /// HELP key
        /// </summary>
        HELP = 0x2F,

        /// <summary>
        /// 0 key
        /// </summary>
        VK_0 = 0x30,

        /// <summary>
        /// 1 key
        /// </summary>
        VK_1 = 0x31,

        /// <summary>
        /// 2 key
        /// </summary>
        VK_2 = 0x32,

        /// <summary>
        /// 3 key
        /// </summary>
        VK_3 = 0x33,

        /// <summary>
        /// 4 key
        /// </summary>
        VK_4 = 0x34,

        /// <summary>
        /// 5 key
        /// </summary>
        VK_5 = 0x35,

        /// <summary>
        /// 6 key
        /// </summary>
        VK_6 = 0x36,

        /// <summary>
        /// 7 key
        /// </summary>
        VK_7 = 0x37,

        /// <summary>
        /// 8 key
        /// </summary>
        VK_8 = 0x38,

        /// <summary>
        /// 9 key
        /// </summary>
        VK_9 = 0x39,

        //
        // 0x3A - 0x40 : Udefined
        //

        /// <summary>
        /// A key
        /// </summary>
        VK_A = 0x41,

        /// <summary>
        /// B key
        /// </summary>
        VK_B = 0x42,

        /// <summary>
        /// C key
        /// </summary>
        VK_C = 0x43,

        /// <summary>
        /// D key
        /// </summary>
        VK_D = 0x44,

        /// <summary>
        /// E key
        /// </summary>
        VK_E = 0x45,

        /// <summary>
        /// F key
        /// </summary>
        VK_F = 0x46,

        /// <summary>
        /// G key
        /// </summary>
        VK_G = 0x47,

        /// <summary>
        /// H key
        /// </summary>
        VK_H = 0x48,

        /// <summary>
        /// I key
        /// </summary>
        VK_I = 0x49,

        /// <summary>
        /// J key
        /// </summary>
        VK_J = 0x4A,

        /// <summary>
        /// K key
        /// </summary>
        VK_K = 0x4B,

        /// <summary>
        /// L key
        /// </summary>
        VK_L = 0x4C,

        /// <summary>
        /// M key
        /// </summary>
        VK_M = 0x4D,

        /// <summary>
        /// N key
        /// </summary>
        VK_N = 0x4E,

        /// <summary>
        /// O key
        /// </summary>
        VK_O = 0x4F,

        /// <summary>
        /// P key
        /// </summary>
        VK_P = 0x50,

        /// <summary>
        /// Q key
        /// </summary>
        VK_Q = 0x51,

        /// <summary>
        /// R key
        /// </summary>
        VK_R = 0x52,

        /// <summary>
        /// S key
        /// </summary>
        VK_S = 0x53,

        /// <summary>
        /// T key
        /// </summary>
        VK_T = 0x54,

        /// <summary>
        /// U key
        /// </summary>
        VK_U = 0x55,

        /// <summary>
        /// V key
        /// </summary>
        VK_V = 0x56,

        /// <summary>
        /// W key
        /// </summary>
        VK_W = 0x57,

        /// <summary>
        /// X key
        /// </summary>
        VK_X = 0x58,

        /// <summary>
        /// Y key
        /// </summary>
        VK_Y = 0x59,

        /// <summary>
        /// Z key
        /// </summary>
        VK_Z = 0x5A,

        /// <summary>
        /// Left Windows key (Microsoft Natural keyboard)
        /// </summary>
        LWIN = 0x5B,

        /// <summary>
        /// Right Windows key (Natural keyboard)
        /// </summary>
        RWIN = 0x5C,

        /// <summary>
        /// Applications key (Natural keyboard)
        /// </summary>
        APPS = 0x5D,

        // 0x5E : reserved

        /// <summary>
        /// Computer Sleep key
        /// </summary>
        SLEEP = 0x5F,

        /// <summary>
        /// Numeric keypad 0 key
        /// </summary>
        NUMPAD0 = 0x60,

        /// <summary>
        /// Numeric keypad 1 key
        /// </summary>
        NUMPAD1 = 0x61,

        /// <summary>
        /// Numeric keypad 2 key
        /// </summary>
        NUMPAD2 = 0x62,

        /// <summary>
        /// Numeric keypad 3 key
        /// </summary>
        NUMPAD3 = 0x63,

        /// <summary>
        /// Numeric keypad 4 key
        /// </summary>
        NUMPAD4 = 0x64,

        /// <summary>
        /// Numeric keypad 5 key
        /// </summary>
        NUMPAD5 = 0x65,

        /// <summary>
        /// Numeric keypad 6 key
        /// </summary>
        NUMPAD6 = 0x66,

        /// <summary>
        /// Numeric keypad 7 key
        /// </summary>
        NUMPAD7 = 0x67,

        /// <summary>
        /// Numeric keypad 8 key
        /// </summary>
        NUMPAD8 = 0x68,

        /// <summary>
        /// Numeric keypad 9 key
        /// </summary>
        NUMPAD9 = 0x69,

        /// <summary>
        /// Multiply key
        /// </summary>
        MULTIPLY = 0x6A,

        /// <summary>
        /// Add key
        /// </summary>
        ADD = 0x6B,

        /// <summary>
        /// Separator key
        /// </summary>
        SEPARATOR = 0x6C,

        /// <summary>
        /// Subtract key
        /// </summary>
        SUBTRACT = 0x6D,

        /// <summary>
        /// Decimal key
        /// </summary>
        DECIMAL = 0x6E,

        /// <summary>
        /// Divide key
        /// </summary>
        DIVIDE = 0x6F,

        /// <summary>
        /// F1 key
        /// </summary>
        F1 = 0x70,

        /// <summary>
        /// F2 key
        /// </summary>
        F2 = 0x71,

        /// <summary>
        /// F3 key
        /// </summary>
        F3 = 0x72,

        /// <summary>
        /// F4 key
        /// </summary>
        F4 = 0x73,

        /// <summary>
        /// F5 key
        /// </summary>
        F5 = 0x74,

        /// <summary>
        /// F6 key
        /// </summary>
        F6 = 0x75,

        /// <summary>
        /// F7 key
        /// </summary>
        F7 = 0x76,

        /// <summary>
        /// F8 key
        /// </summary>
        F8 = 0x77,

        /// <summary>
        /// F9 key
        /// </summary>
        F9 = 0x78,

        /// <summary>
        /// F10 key
        /// </summary>
        F10 = 0x79,

        /// <summary>
        /// F11 key
        /// </summary>
        F11 = 0x7A,

        /// <summary>
        /// F12 key
        /// </summary>
        F12 = 0x7B,

        /// <summary>
        /// F13 key
        /// </summary>
        F13 = 0x7C,

        /// <summary>
        /// F14 key
        /// </summary>
        F14 = 0x7D,

        /// <summary>
        /// F15 key
        /// </summary>
        F15 = 0x7E,

        /// <summary>
        /// F16 key
        /// </summary>
        F16 = 0x7F,

        /// <summary>
        /// F17 key
        /// </summary>
        F17 = 0x80,

        /// <summary>
        /// F18 key
        /// </summary>
        F18 = 0x81,

        /// <summary>
        /// F19 key
        /// </summary>
        F19 = 0x82,

        /// <summary>
        /// F20 key
        /// </summary>
        F20 = 0x83,

        /// <summary>
        /// F21 key
        /// </summary>
        F21 = 0x84,

        /// <summary>
        /// F22 key
        /// </summary>
        F22 = 0x85,

        /// <summary>
        /// F23 key
        /// </summary>
        F23 = 0x86,

        /// <summary>
        /// F24 key
        /// </summary>
        F24 = 0x87,

        //
        // 0x88 - 0x8F : Unassigned
        //

        /// <summary>
        /// NUM LOCK key
        /// </summary>
        NUMLOCK = 0x90,

        /// <summary>
        /// SCROLL LOCK key
        /// </summary>
        SCROLL = 0x91,

        // 0x92 - 0x96 : OEM Specific

        // 0x97 - 0x9F : Unassigned

        //
        // L* & R* - left and right Alt, Ctrl and Shift virtual keys.
        // Used only as parameters to GetAsyncKeyState() and GetKeyState().
        // No other API or message will distinguish left and right keys in this way.
        //

        /// <summary>
        /// Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        LSHIFT = 0xA0,

        /// <summary>
        /// Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        RSHIFT = 0xA1,

        /// <summary>
        /// Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        LCONTROL = 0xA2,

        /// <summary>
        /// Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        RCONTROL = 0xA3,

        /// <summary>
        /// Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        LMENU = 0xA4,

        /// <summary>
        /// Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        /// </summary>
        RMENU = 0xA5,

        /// <summary>
        /// Windows 2000/XP: Browser Back key
        /// </summary>
        BROWSER_BACK = 0xA6,

        /// <summary>
        /// Windows 2000/XP: Browser Forward key
        /// </summary>
        BROWSER_FORWARD = 0xA7,

        /// <summary>
        /// Windows 2000/XP: Browser Refresh key
        /// </summary>
        BROWSER_REFRESH = 0xA8,

        /// <summary>
        /// Windows 2000/XP: Browser Stop key
        /// </summary>
        BROWSER_STOP = 0xA9,

        /// <summary>
        /// Windows 2000/XP: Browser Search key
        /// </summary>
        BROWSER_SEARCH = 0xAA,

        /// <summary>
        /// Windows 2000/XP: Browser Favorites key
        /// </summary>
        BROWSER_FAVORITES = 0xAB,

        /// <summary>
        /// Windows 2000/XP: Browser Start and Home key
        /// </summary>
        BROWSER_HOME = 0xAC,

        /// <summary>
        /// Windows 2000/XP: Volume Mute key
        /// </summary>
        VOLUME_MUTE = 0xAD,

        /// <summary>
        /// Windows 2000/XP: Volume Down key
        /// </summary>
        VOLUME_DOWN = 0xAE,

        /// <summary>
        /// Windows 2000/XP: Volume Up key
        /// </summary>
        VOLUME_UP = 0xAF,

        /// <summary>
        /// Windows 2000/XP: Next Track key
        /// </summary>
        MEDIA_NEXT_TRACK = 0xB0,

        /// <summary>
        /// Windows 2000/XP: Previous Track key
        /// </summary>
        MEDIA_PREV_TRACK = 0xB1,

        /// <summary>
        /// Windows 2000/XP: Stop Media key
        /// </summary>
        MEDIA_STOP = 0xB2,

        /// <summary>
        /// Windows 2000/XP: Play/Pause Media key
        /// </summary>
        MEDIA_PLAY_PAUSE = 0xB3,

        /// <summary>
        /// Windows 2000/XP: Start Mail key
        /// </summary>
        LAUNCH_MAIL = 0xB4,

        /// <summary>
        /// Windows 2000/XP: Select Media key
        /// </summary>
        MEDIA_SELECT = 0xB5,

        /// <summary>
        /// Windows 2000/XP: Start Application 1 key
        /// </summary>
        LAUNCH_APP1 = 0xB6,

        /// <summary>
        /// Windows 2000/XP: Start Application 2 key
        /// </summary>
        LAUNCH_APP2 = 0xB7,

        //
        // 0xB8 - 0xB9 : Reserved
        //

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the ';:' key 
        /// </summary>
        OEM_1 = 0xBA,

        /// <summary>
        /// Windows 2000/XP: For any country/region, the '+' key
        /// </summary>
        OEM_PLUS = 0xBB,

        /// <summary>
        /// Windows 2000/XP: For any country/region, the ',' key
        /// </summary>
        OEM_COMMA = 0xBC,

        /// <summary>
        /// Windows 2000/XP: For any country/region, the '-' key
        /// </summary>
        OEM_MINUS = 0xBD,

        /// <summary>
        /// Windows 2000/XP: For any country/region, the '.' key
        /// </summary>
        OEM_PERIOD = 0xBE,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '/?' key 
        /// </summary>
        OEM_2 = 0xBF,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '`~' key 
        /// </summary>
        OEM_3 = 0xC0,

        //
        // 0xC1 - 0xD7 : Reserved
        //

        //
        // 0xD8 - 0xDA : Unassigned
        //

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '[{' key
        /// </summary>
        OEM_4 = 0xDB,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the '\|' key
        /// </summary>
        OEM_5 = 0xDC,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the ']}' key
        /// </summary>
        OEM_6 = 0xDD,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote' key
        /// </summary>
        OEM_7 = 0xDE,

        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// </summary>
        OEM_8 = 0xDF,

        //
        // 0xE0 : Reserved
        //

        //
        // 0xE1 : OEM Specific
        //

        /// <summary>
        /// Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
        /// </summary>
        OEM_102 = 0xE2,

        //
        // (0xE3-E4) : OEM specific
        //

        /// <summary>
        /// Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
        /// </summary>
        PROCESSKEY = 0xE5,

        //
        // 0xE6 : OEM specific
        //

        /// <summary>
        /// Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        /// </summary>
        PACKET = 0xE7,

        //
        // 0xE8 : Unassigned
        //

        //
        // 0xE9-F5 : OEM specific
        //

        /// <summary>
        /// Attn key
        /// </summary>
        ATTN = 0xF6,

        /// <summary>
        /// CrSel key
        /// </summary>
        CRSEL = 0xF7,

        /// <summary>
        /// ExSel key
        /// </summary>
        EXSEL = 0xF8,

        /// <summary>
        /// Erase EOF key
        /// </summary>
        EraseEof = 0xF9,

        /// <summary>
        /// Play key
        /// </summary>
        PLAY = 0xFA,

        /// <summary>
        /// Zoom key
        /// </summary>
        ZOOM = 0xFB,

        /// <summary>
        /// Reserved
        /// </summary>
        NONAME = 0xFC,

        /// <summary>
        /// PA1 key
        /// </summary>
        PA1 = 0xFD,

        /// <summary>
        /// Clear key
        /// </summary>
        OEM_CLEAR = 0xFE,
    }
}
