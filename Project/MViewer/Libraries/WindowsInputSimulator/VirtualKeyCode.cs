using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsInput
{
    public class KeyCodeParser
    {
        public VirtualKeyCode ParseKeyCode(Keys keyCode)
        {
            VirtualKeyCode virtualKeyCode = VirtualKeyCode.Undefined;

            switch(keyCode)
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

                //case Keys.LineFeed:
                //    virtualKeyCode = VirtualKeyCode.LineFeed;
                    break;
                case Keys.Clear:
                    virtualKeyCode = VirtualKeyCode.CLEAR;
                    break;
                    case Keys.Enter:
                    virtualKeyCode = VirtualKeyCode.ENTER;
                    break;
                    //case Keys.Return:
                    //virtualKeyCode = VirtualKeyCode.Return;
                    break;
                    case Keys.ShiftKey:
                    virtualKeyCode = VirtualKeyCode.SHIFTKEY;
                    break;
                    case Keys.ControlKey:
                    virtualKeyCode = VirtualKeyCode.CONTROLKEY;
                    break;
                    case Keys.Menu:
                    virtualKeyCode = VirtualKeyCode.MENU;
                    break;
                    case Keys.Pause:
                    virtualKeyCode = VirtualKeyCode.PAUSE;
                    break;
      
        //
        // Summary:
        //     The CAPS LOCK key.
        CapsLock = 20,
        //
        // Summary:
        //     The CAPS LOCK key.
        Capital = 20,
        //
        // Summary:
        //     The IME Kana mode key.
        KanaMode = 21,
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        HanguelMode = 21,
        //
        // Summary:
        //     The IME Hangul mode key.
        HangulMode = 21,
        //
        // Summary:
        //     The IME Junja mode key.
        JunjaMode = 23,
        //
        // Summary:
        //     The IME final mode key.
        FinalMode = 24,
        //
        // Summary:
        //     The IME Kanji mode key.
        KanjiMode = 25,
        //
        // Summary:
        //     The IME Hanja mode key.
        HanjaMode = 25,
        //
        // Summary:
        //     The ESC key.
        Escape = 27,
        //
        // Summary:
        //     The IME convert key.
        IMEConvert = 28,
        //
        // Summary:
        //     The IME nonconvert key.
        IMENonconvert = 29,
        //
        // Summary:
        //     The IME accept key. Obsolete, use System.Windows.Forms.Keys.IMEAccept instead.
        IMEAceept = 30,
        //
        // Summary:
        //     The IME accept key, replaces System.Windows.Forms.Keys.IMEAceept.
        IMEAccept = 30,
        //
        // Summary:
        //     The IME mode change key.
        IMEModeChange = 31,
        //
        // Summary:
        //     The SPACEBAR key.
        Space = 32,
        //
        // Summary:
        //     The PAGE UP key.
        Prior = 33,
        //
        // Summary:
        //     The PAGE UP key.
        PageUp = 33,
        //
        // Summary:
        //     The PAGE DOWN key.
        Next = 34,
        //
        // Summary:
        //     The PAGE DOWN key.
        PageDown = 34,
        //
        // Summary:
        //     The END key.
        End = 35,
        //
        // Summary:
        //     The HOME key.
        Home = 36,
        //
        // Summary:
        //     The LEFT ARROW key.
        Left = 37,
        //
        // Summary:
        //     The UP ARROW key.
        Up = 38,
        //
        // Summary:
        //     The RIGHT ARROW key.
        Right = 39,
        //
        // Summary:
        //     The DOWN ARROW key.
        Down = 40,
        //
        // Summary:
        //     The SELECT key.
        Select = 41,
        //
        // Summary:
        //     The PRINT key.
        Print = 42,
        //
        // Summary:
        //     The EXECUTE key.
        Execute = 43,
        //
        // Summary:
        //     The PRINT SCREEN key.
        PrintScreen = 44,
        //
        // Summary:
        //     The PRINT SCREEN key.
        Snapshot = 44,
        //
        // Summary:
        //     The INS key.
        Insert = 45,
        //
        // Summary:
        //     The DEL key.
        Delete = 46,
        //
        // Summary:
        //     The HELP key.
        Help = 47,
        //
        // Summary:
        //     The 0 key.
        D0 = 48,
        //
        // Summary:
        //     The 1 key.
        D1 = 49,
        //
        // Summary:
        //     The 2 key.
        D2 = 50,
        //
        // Summary:
        //     The 3 key.
        D3 = 51,
        //
        // Summary:
        //     The 4 key.
        D4 = 52,
        //
        // Summary:
        //     The 5 key.
        D5 = 53,
        //
        // Summary:
        //     The 6 key.
        D6 = 54,
        //
        // Summary:
        //     The 7 key.
        D7 = 55,
        //
        // Summary:
        //     The 8 key.
        D8 = 56,
        //
        // Summary:
        //     The 9 key.
        D9 = 57,
        //
        // Summary:
        //     The A key.
        A = 65,
        //
        // Summary:
        //     The B key.
        B = 66,
        //
        // Summary:
        //     The C key.
        C = 67,
        //
        // Summary:
        //     The D key.
        D = 68,
        //
        // Summary:
        //     The E key.
        E = 69,
        //
        // Summary:
        //     The F key.
        F = 70,
        //
        // Summary:
        //     The G key.
        G = 71,
        //
        // Summary:
        //     The H key.
        H = 72,
        //
        // Summary:
        //     The I key.
        I = 73,
        //
        // Summary:
        //     The J key.
        J = 74,
        //
        // Summary:
        //     The K key.
        K = 75,
        //
        // Summary:
        //     The L key.
        L = 76,
        //
        // Summary:
        //     The M key.
        M = 77,
        //
        // Summary:
        //     The N key.
        N = 78,
        //
        // Summary:
        //     The O key.
        O = 79,
        //
        // Summary:
        //     The P key.
        P = 80,
        //
        // Summary:
        //     The Q key.
        Q = 81,
        //
        // Summary:
        //     The R key.
        R = 82,
        //
        // Summary:
        //     The S key.
        S = 83,
        //
        // Summary:
        //     The T key.
        T = 84,
        //
        // Summary:
        //     The U key.
        U = 85,
        //
        // Summary:
        //     The V key.
        V = 86,
        //
        // Summary:
        //     The W key.
        W = 87,
        //
        // Summary:
        //     The X key.
        X = 88,
        //
        // Summary:
        //     The Y key.
        Y = 89,
        //
        // Summary:
        //     The Z key.
        Z = 90,
        //
        // Summary:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        LWin = 91,
        //
        // Summary:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        RWin = 92,
        //
        // Summary:
        //     The application key (Microsoft Natural Keyboard).
        Apps = 93,
        //
        // Summary:
        //     The computer sleep key.
        Sleep = 95,
        //
        // Summary:
        //     The 0 key on the numeric keypad.
        NumPad0 = 96,
        //
        // Summary:
        //     The 1 key on the numeric keypad.
        NumPad1 = 97,
        //
        // Summary:
        //     The 2 key on the numeric keypad.
        NumPad2 = 98,
        //
        // Summary:
        //     The 3 key on the numeric keypad.
        NumPad3 = 99,
        //
        // Summary:
        //     The 4 key on the numeric keypad.
        NumPad4 = 100,
        //
        // Summary:
        //     The 5 key on the numeric keypad.
        NumPad5 = 101,
        //
        // Summary:
        //     The 6 key on the numeric keypad.
        NumPad6 = 102,
        //
        // Summary:
        //     The 7 key on the numeric keypad.
        NumPad7 = 103,
        //
        // Summary:
        //     The 8 key on the numeric keypad.
        NumPad8 = 104,
        //
        // Summary:
        //     The 9 key on the numeric keypad.
        NumPad9 = 105,
        //
        // Summary:
        //     The multiply key.
        Multiply = 106,
        //
        // Summary:
        //     The add key.
        Add = 107,
        //
        // Summary:
        //     The separator key.
        Separator = 108,
        //
        // Summary:
        //     The subtract key.
        Subtract = 109,
        //
        // Summary:
        //     The decimal key.
        Decimal = 110,
        //
        // Summary:
        //     The divide key.
        Divide = 111,
        //
        // Summary:
        //     The F1 key.
        F1 = 112,
        //
        // Summary:
        //     The F2 key.
        F2 = 113,
        //
        // Summary:
        //     The F3 key.
        F3 = 114,
        //
        // Summary:
        //     The F4 key.
        F4 = 115,
        //
        // Summary:
        //     The F5 key.
        F5 = 116,
        //
        // Summary:
        //     The F6 key.
        F6 = 117,
        //
        // Summary:
        //     The F7 key.
        F7 = 118,
        //
        // Summary:
        //     The F8 key.
        F8 = 119,
        //
        // Summary:
        //     The F9 key.
        F9 = 120,
        //
        // Summary:
        //     The F10 key.
        F10 = 121,
        //
        // Summary:
        //     The F11 key.
        F11 = 122,
        //
        // Summary:
        //     The F12 key.
        F12 = 123,
        //
        // Summary:
        //     The F13 key.
        F13 = 124,
        //
        // Summary:
        //     The F14 key.
        F14 = 125,
        //
        // Summary:
        //     The F15 key.
        F15 = 126,
        //
        // Summary:
        //     The F16 key.
        F16 = 127,
        //
        // Summary:
        //     The F17 key.
        F17 = 128,
        //
        // Summary:
        //     The F18 key.
        F18 = 129,
        //
        // Summary:
        //     The F19 key.
        F19 = 130,
        //
        // Summary:
        //     The F20 key.
        F20 = 131,
        //
        // Summary:
        //     The F21 key.
        F21 = 132,
        //
        // Summary:
        //     The F22 key.
        F22 = 133,
        //
        // Summary:
        //     The F23 key.
        F23 = 134,
        //
        // Summary:
        //     The F24 key.
        F24 = 135,
        //
        // Summary:
        //     The NUM LOCK key.
        NumLock = 144,
        //
        // Summary:
        //     The SCROLL LOCK key.
        Scroll = 145,
        //
        // Summary:
        //     The left SHIFT key.
        LShiftKey = 160,
        //
        // Summary:
        //     The right SHIFT key.
        RShiftKey = 161,
        //
        // Summary:
        //     The left CTRL key.
        LControlKey = 162,
        //
        // Summary:
        //     The right CTRL key.
        RControlKey = 163,
        //
        // Summary:
        //     The left ALT key.
        LMenu = 164,
        //
        // Summary:
        //     The right ALT key.
        RMenu = 165,
        //
        // Summary:
        //     The browser back key (Windows 2000 or later).
        BrowserBack = 166,
        //
        // Summary:
        //     The browser forward key (Windows 2000 or later).
        BrowserForward = 167,
        //
        // Summary:
        //     The browser refresh key (Windows 2000 or later).
        BrowserRefresh = 168,
        //
        // Summary:
        //     The browser stop key (Windows 2000 or later).
        BrowserStop = 169,
        //
        // Summary:
        //     The browser search key (Windows 2000 or later).
        BrowserSearch = 170,
        //
        // Summary:
        //     The browser favorites key (Windows 2000 or later).
        BrowserFavorites = 171,
        //
        // Summary:
        //     The browser home key (Windows 2000 or later).
        BrowserHome = 172,
        //
        // Summary:
        //     The volume mute key (Windows 2000 or later).
        VolumeMute = 173,
        //
        // Summary:
        //     The volume down key (Windows 2000 or later).
        VolumeDown = 174,
        //
        // Summary:
        //     The volume up key (Windows 2000 or later).
        VolumeUp = 175,
        //
        // Summary:
        //     The media next track key (Windows 2000 or later).
        MediaNextTrack = 176,
        //
        // Summary:
        //     The media previous track key (Windows 2000 or later).
        MediaPreviousTrack = 177,
        //
        // Summary:
        //     The media Stop key (Windows 2000 or later).
        MediaStop = 178,
        //
        // Summary:
        //     The media play pause key (Windows 2000 or later).
        MediaPlayPause = 179,
        //
        // Summary:
        //     The launch mail key (Windows 2000 or later).
        LaunchMail = 180,
        //
        // Summary:
        //     The select media key (Windows 2000 or later).
        SelectMedia = 181,
        //
        // Summary:
        //     The start application one key (Windows 2000 or later).
        LaunchApplication1 = 182,
        //
        // Summary:
        //     The start application two key (Windows 2000 or later).
        LaunchApplication2 = 183,
        //
        // Summary:
        //     The OEM 1 key.
        Oem1 = 186,
        //
        // Summary:
        //     The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
        OemSemicolon = 186,
        //
        // Summary:
        //     The OEM plus key on any country/region keyboard (Windows 2000 or later).
        Oemplus = 187,
        //
        // Summary:
        //     The OEM comma key on any country/region keyboard (Windows 2000 or later).
        Oemcomma = 188,
        //
        // Summary:
        //     The OEM minus key on any country/region keyboard (Windows 2000 or later).
        OemMinus = 189,
        //
        // Summary:
        //     The OEM period key on any country/region keyboard (Windows 2000 or later).
        OemPeriod = 190,
        //
        // Summary:
        //     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
        OemQuestion = 191,
        //
        // Summary:
        //     The OEM 2 key.
        Oem2 = 191,
        //
        // Summary:
        //     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
        Oemtilde = 192,
        //
        // Summary:
        //     The OEM 3 key.
        Oem3 = 192,
        //
        // Summary:
        //     The OEM 4 key.
        Oem4 = 219,
        //
        // Summary:
        //     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
        OemOpenBrackets = 219,
        //
        // Summary:
        //     The OEM pipe key on a US standard keyboard (Windows 2000 or later).
        OemPipe = 220,
        //
        // Summary:
        //     The OEM 5 key.
        Oem5 = 220,
        //
        // Summary:
        //     The OEM 6 key.
        Oem6 = 221,
        //
        // Summary:
        //     The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
        OemCloseBrackets = 221,
        //
        // Summary:
        //     The OEM 7 key.
        Oem7 = 222,
        //
        // Summary:
        //     The OEM singled/double quote key on a US standard keyboard (Windows 2000
        //     or later).
        OemQuotes = 222,
        //
        // Summary:
        //     The OEM 8 key.
        Oem8 = 223,
        //
        // Summary:
        //     The OEM 102 key.
        Oem102 = 226,
        //
        // Summary:
        //     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows
        //     2000 or later).
        OemBackslash = 226,
        //
        // Summary:
        //     The PROCESS KEY key.
        ProcessKey = 229,
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key
        //     value is the low word of a 32-bit virtual-key value used for non-keyboard
        //     input methods.
        Packet = 231,
        //
        // Summary:
        //     The ATTN key.
        Attn = 246,
        //
        // Summary:
        //     The CRSEL key.
        Crsel = 247,
        //
        // Summary:
        //     The EXSEL key.
        Exsel = 248,
        //
        // Summary:
        //     The ERASE EOF key.
        EraseEof = 249,
        //
        // Summary:
        //     The PLAY key.
        Play = 250,
        //
        // Summary:
        //     The ZOOM key.
        Zoom = 251,
        //
        // Summary:
        //     A constant reserved for future use.
        NoName = 252,
        //
        // Summary:
        //     The PA1 key.
        Pa1 = 253,
        //
        // Summary:
        //     The CLEAR key.
        OemClear = 254,
        //
        // Summary:
        //     The bitmask to extract a key code from a key value.
        KeyCode = 65535,
        //
        // Summary:
        //     The SHIFT modifier key.
        Shift = 65536,
        //
        // Summary:
        //     The CTRL modifier key.
        Control = 131072,
        //
        // Summary:
        //     The ALT modifier key.
        Alt = 262144,
            }

            return virtualKeyCode;
        }
    }

    /// <summary>
    /// The list of VirtualKeyCodes (see: http://msdn.microsoft.com/en-us/library/ms645540(VS.85).aspx)
    /// </summary>
    public enum VirtualKeyCode : ushort // UInt16
    {
        Undefined = -25234634634,

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
        MENU = 0x12,

        /// <summary>
        /// PAUSE key
        /// </summary>
        PAUSE = 0x13,

        /// <summary>
        /// CAPS LOCK key
        /// </summary>
        CAPITAL = 0x14,

        /// <summary>
        /// Input Method Editor (IME) Kana mode
        /// </summary>
        KANA = 0x15,

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
        PRIOR = 0x21,

        /// <summary>
        /// PAGE DOWN key
        /// </summary>
        NEXT = 0x22,

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
        LAUNCH_MEDIA_SELECT = 0xB5,

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
        EREOF = 0xF9,

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
