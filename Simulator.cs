using System;
using WindowsInput;
using WindowsInput.Native;

namespace drivers
{
    class VitualKey
    {
        public static void MouseAction(String[] values)
        {
            try
            {
                String actionKey = "";
                if (values != null && values.Length > 4)
                {
                    InputSimulator sim = new InputSimulator();
                    actionKey = values[4];
                    switch (actionKey.ToUpper())
                    {
                        case "CLICK":
                            sim.Mouse.LeftButtonClick();
                            break;
                        case "DOUBLE_CLICK":
                            sim.Mouse.LeftButtonDoubleClick();
                            break;
                        case "RIGHT_CLICK":
                            sim.Mouse.RightButtonClick();
                            break;
                        case "MOUSE_MOVE":
                            sim.Mouse.MoveMouseTo(Int32.Parse(values[2]), Int32.Parse(values[3]));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void HitKeys(String key)
        {
            try
            {
                if (key == null)
                {
                    return;
                }
                String[] commandList = TestUtil.SplitString(key, "->");
                InputSimulator sim = new InputSimulator();
                for (int i = 0; i < commandList.Length; i++)
                {
                    String[] keysList = TestUtil.SplitString(commandList[i], ";");
                    if (keysList != null)
                    {
                        int[] keyCodeArray = new int[keysList.Length];
                        for (int j = 0; j < keysList.Length; j++)
                        {
                            if (keysList[j] == null)
                            {
                                return;
                            }
                            else
                            {
                                keysList[j] = keysList[j].Trim();
                            }
                            sim.Keyboard.KeyPress(GetKeyCode(keysList[j]));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static VirtualKeyCode GetKeyCode(String key)
        {
            switch (key.ToUpper())
            {
                //
                // Summary:
                //     Left mouse button
                case "LBUTTON": return VirtualKeyCode.LBUTTON;
                //
                // Summary:
                //     Right mouse button
                case "RBUTTON": return VirtualKeyCode.RBUTTON;
                //
                // Summary:
                //     Control-break processing
                case "CANCEL": return VirtualKeyCode.CANCEL;
                //
                // Summary:
                //     Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
                case "MBUTTON": return VirtualKeyCode.MBUTTON;
                //
                // Summary:
                //     Windows 2000/XP: X1 mouse button - NOT contiguous with LBUTTON and RBUTTON
                case "XBUTTON1": return VirtualKeyCode.XBUTTON1;
                //
                // Summary:
                //     Windows 2000/XP: X2 mouse button - NOT contiguous with LBUTTON and RBUTTON
                case "XBUTTON2": return VirtualKeyCode.XBUTTON2;
                //
                // Summary:
                //     BACKSPACE key
                case "BACK": return VirtualKeyCode.BACK;
                //
                // Summary:
                //     TAB key
                case "TAB": return VirtualKeyCode.TAB;
                //
                // Summary:
                //     CLEAR key
                case "CLEAR": return VirtualKeyCode.CLEAR;
                //
                // Summary:
                //     ENTER key
                case "RETURN": return VirtualKeyCode.RETURN;
                case "ENTER": return VirtualKeyCode.RETURN;
                //
                // Summary:
                //     SHIFT key
                case "SHIFT": return VirtualKeyCode.SHIFT;
                //
                // Summary:
                //     CTRL key
                case "CONTROL": return VirtualKeyCode.CONTROL;
                //
                // Summary:
                //     ALT key
                case "MENU": return VirtualKeyCode.MENU;
                //
                // Summary:
                //     PAUSE key
                case "PAUSE": return VirtualKeyCode.PAUSE;
                //
                // Summary:
                //     CAPS LOCK key
                case "CAPITAL": return VirtualKeyCode.CAPITAL;
                //
                // Summary:
                //     Input Method Editor (IME) Kana mode
                case "KANA": return VirtualKeyCode.KANA;
                //
                // Summary:
                //     IME Hanguel mode (maintained for compatibility; use HANGUL)
                case "HANGEUL": return VirtualKeyCode.HANGEUL;
                //
                // Summary:
                //     IME Hangul mode
                case "HANGUL": return VirtualKeyCode.HANGUL;
                //
                // Summary:
                //     IME Junja mode
                case "JUNJA": return VirtualKeyCode.JUNJA;
                //
                // Summary:
                //     IME final mode
                case "FINAL": return VirtualKeyCode.FINAL;
                //
                // Summary:
                //     IME Hanja mode
                case "HANJA": return VirtualKeyCode.HANJA;
                //
                // Summary:
                //     IME Kanji mode
                case "KANJI": return VirtualKeyCode.KANJI;
                //
                // Summary:
                //     ESC key
                case "ESCAPE": return VirtualKeyCode.ESCAPE;
                //
                // Summary:
                //     IME convert
                case "CONVERT": return VirtualKeyCode.CONVERT;
                //
                // Summary:
                //     IME nonconvert
                case "NONCONVERT": return VirtualKeyCode.NONCONVERT;
                //
                // Summary:
                //     IME accept
                case "ACCEPT": return VirtualKeyCode.ACCEPT;
                //
                // Summary:
                //     IME mode change request
                case "MODECHANGE": return VirtualKeyCode.MODECHANGE;
                //
                // Summary:
                //     SPACEBAR
                case "SPACE": return VirtualKeyCode.SPACE;
                //
                // Summary:
                //     PAGE UP key
                case "PRIOR": return VirtualKeyCode.PRIOR;
                //
                // Summary:
                //     PAGE DOWN key
                case "NEXT": return VirtualKeyCode.NEXT;
                //
                // Summary:
                //     END key
                case "END": return VirtualKeyCode.END;
                //
                // Summary:
                //     HOME key
                case "HOME": return VirtualKeyCode.HOME;
                //
                // Summary:
                //     LEFT ARROW key
                case "LEFT": return VirtualKeyCode.LEFT;
                //
                // Summary:
                //     UP ARROW key
                case "UP": return VirtualKeyCode.UP;
                //
                // Summary:
                //     RIGHT ARROW key
                case "RIGHT": return VirtualKeyCode.RIGHT;
                //
                // Summary:
                //     DOWN ARROW key
                case "DOWN": return VirtualKeyCode.DOWN;
                //
                // Summary:
                //     SELECT key
                case "SELECT": return VirtualKeyCode.SELECT;
                //
                // Summary:
                //     PRINT key
                case "PRINT": return VirtualKeyCode.PRINT;
                //
                // Summary:
                //     EXECUTE key
                case "EXECUTE": return VirtualKeyCode.EXECUTE;
                //
                // Summary:
                //     PRINT SCREEN key
                case "SNAPSHOT": return VirtualKeyCode.SNAPSHOT;
                //
                // Summary:
                //     INS key
                case "INSERT": return VirtualKeyCode.INSERT;
                //
                // Summary:
                //     DEL key
                case "DELETE": return VirtualKeyCode.DELETE;
                //
                // Summary:
                //     HELP key
                case "HELP": return VirtualKeyCode.HELP;
                //
                // Summary:
                //     0 key
                case "VK_0": return VirtualKeyCode.VK_0;
                //
                // Summary:
                //     1 key
                case "VK_1": return VirtualKeyCode.VK_1;
                //
                // Summary:
                //     2 key
                case "VK_2": return VirtualKeyCode.VK_2;
                //
                // Summary:
                //     3 key
                case "VK_3": return VirtualKeyCode.VK_3;
                //
                // Summary:
                //     4 key
                case "VK_4": return VirtualKeyCode.VK_4;
                //
                // Summary:
                //     5 key
                case "VK_5": return VirtualKeyCode.VK_5;
                //
                // Summary:
                //     6 key
                case "VK_6": return VirtualKeyCode.VK_6;
                //
                // Summary:
                //     7 key
                case "VK_7": return VirtualKeyCode.VK_7;
                //
                // Summary:
                //     8 key
                case "VK_8": return VirtualKeyCode.VK_8;
                //
                // Summary:
                //     9 key
                case "VK_9": return VirtualKeyCode.VK_9;
                //
                // Summary:
                //     A key
                case "VK_A": return VirtualKeyCode.VK_A;
                //
                // Summary:
                //     B key
                case "VK_B": return VirtualKeyCode.VK_B;
                //
                // Summary:
                //     C key
                case "VK_C": return VirtualKeyCode.VK_C;
                //
                // Summary:
                //     D key
                case "VK_D": return VirtualKeyCode.VK_D;
                //
                // Summary:
                //     E key
                case "VK_E": return VirtualKeyCode.VK_E;
                //
                // Summary:
                //     F key
                case "VK_F": return VirtualKeyCode.VK_F;
                //
                // Summary:
                //     G key
                case "VK_G": return VirtualKeyCode.VK_G;
                //
                // Summary:
                //     H key
                case "VK_H": return VirtualKeyCode.VK_H;
                //
                // Summary:
                //     I key
                case "VK_I": return VirtualKeyCode.VK_I;
                //
                // Summary:
                //     J key
                case "VK_J": return VirtualKeyCode.VK_J;
                //
                // Summary:
                //     K key
                case "VK_K": return VirtualKeyCode.VK_K;
                //
                // Summary:
                //     L key
                case "VK_L": return VirtualKeyCode.VK_L;
                //
                // Summary:
                //     M key
                case "VK_M": return VirtualKeyCode.VK_M;
                //
                // Summary:
                //     N key
                case "VK_N": return VirtualKeyCode.VK_N;
                //
                // Summary:
                //     O key
                case "VK_O": return VirtualKeyCode.VK_O;
                //
                // Summary:
                //     P key
                case "VK_P": return VirtualKeyCode.VK_P;
                //
                // Summary:
                //     Q key
                case "VK_Q": return VirtualKeyCode.VK_Q;
                //
                // Summary:
                //     R key
                case "VK_R": return VirtualKeyCode.VK_R;
                //
                // Summary:
                //     S key
                case "VK_S": return VirtualKeyCode.VK_S;
                //
                // Summary:
                //     T key
                case "VK_T": return VirtualKeyCode.VK_T;
                //
                // Summary:
                //     U key
                case "VK_U": return VirtualKeyCode.VK_U;
                //
                // Summary:
                //     V key
                case "VK_V": return VirtualKeyCode.VK_V;
                //
                // Summary:
                //     W key
                case "VK_W": return VirtualKeyCode.VK_W;
                //
                // Summary:
                //     X key
                case "VK_X": return VirtualKeyCode.VK_X;
                //
                // Summary:
                //     Y key
                case "VK_Y": return VirtualKeyCode.VK_Y;
                //
                // Summary:
                //     Z key
                case "VK_Z": return VirtualKeyCode.VK_Z;
                //
                // Summary:
                //     Left Windows key (Microsoft Natural keyboard)
                case "LWIN": return VirtualKeyCode.LWIN;
                //
                // Summary:
                //     Right Windows key (Natural keyboard)
                case "RWIN": return VirtualKeyCode.RWIN;
                //
                // Summary:
                //     Applications key (Natural keyboard)
                case "APPS": return VirtualKeyCode.APPS;
                //
                // Summary:
                //     Computer Sleep key
                case "SLEEP": return VirtualKeyCode.SLEEP;
                //
                // Summary:
                //     Numeric keypad 0 key
                case "NUMPAD0": return VirtualKeyCode.NUMPAD0;
                //
                // Summary:
                //     Numeric keypad 1 key
                case "NUMPAD1": return VirtualKeyCode.NUMPAD1;
                //
                // Summary:
                //     Numeric keypad 2 key
                case "NUMPAD2": return VirtualKeyCode.NUMPAD2;
                //
                // Summary:
                //     Numeric keypad 3 key
                case "NUMPAD3": return VirtualKeyCode.NUMPAD3;
                //
                // Summary:
                //     Numeric keypad 4 key
                case "NUMPAD4": return VirtualKeyCode.NUMPAD4;
                //
                // Summary:
                //     Numeric keypad 5 key
                case "NUMPAD5": return VirtualKeyCode.NUMPAD5;
                //
                // Summary:
                //     Numeric keypad 6 key
                case "NUMPAD6": return VirtualKeyCode.NUMPAD6;
                //
                // Summary:
                //     Numeric keypad 7 key
                case "NUMPAD7": return VirtualKeyCode.NUMPAD7;
                //
                // Summary:
                //     Numeric keypad 8 key
                case "NUMPAD8": return VirtualKeyCode.NUMPAD8;
                //
                // Summary:
                //     Numeric keypad 9 key
                case "NUMPAD9": return VirtualKeyCode.NUMPAD9;
                //
                // Summary:
                //     Multiply key
                case "MULTIPLY": return VirtualKeyCode.MULTIPLY;
                //
                // Summary:
                //     Add key
                case "ADD": return VirtualKeyCode.ADD;
                //
                // Summary:
                //     Separator key
                case "SEPARATOR": return VirtualKeyCode.SEPARATOR;
                //
                // Summary:
                //     Subtract key
                case "SUBTRACT": return VirtualKeyCode.SUBTRACT;
                //
                // Summary:
                //     Decimal key
                case "DECIMAL": return VirtualKeyCode.DECIMAL;
                //
                // Summary:
                //     Divide key
                case "DIVIDE": return VirtualKeyCode.DIVIDE;
                //
                // Summary:
                //     F1 key
                case "F1": return VirtualKeyCode.F1;
                //
                // Summary:
                //     F2 key
                case "F2": return VirtualKeyCode.F2;
                //
                // Summary:
                //     F3 key
                case "F3": return VirtualKeyCode.F3;
                //
                // Summary:
                //     F4 key
                case "F4": return VirtualKeyCode.F4;
                //
                // Summary:
                //     F5 key
                case "F5": return VirtualKeyCode.F5;
                //
                // Summary:
                //     F6 key
                case "F6": return VirtualKeyCode.F6;
                //
                // Summary:
                //     F7 key
                case "F7": return VirtualKeyCode.F7;
                //
                // Summary:
                //     F8 key
                case "F8": return VirtualKeyCode.F8;
                //
                // Summary:
                //     F9 key
                case "F9": return VirtualKeyCode.F9;
                //
                // Summary:
                //     F10 key
                case "F10": return VirtualKeyCode.F10;
                //
                // Summary:
                //     F11 key
                case "F11": return VirtualKeyCode.F11;
                //
                // Summary:
                //     F12 key
                case "F12": return VirtualKeyCode.F12;
                //
                // Summary:
                //     F13 key
                case "F13": return VirtualKeyCode.F13;
                //
                // Summary:
                //     F14 key
                case "F14": return VirtualKeyCode.F14;
                //
                // Summary:
                //     F15 key
                case "F15": return VirtualKeyCode.F15;
                //
                // Summary:
                //     F16 key
                case "F16": return VirtualKeyCode.F16;
                //
                // Summary:
                //     F17 key
                case "F17": return VirtualKeyCode.F17;
                //
                // Summary:
                //     F18 key
                case "F18": return VirtualKeyCode.F18;
                //
                // Summary:
                //     F19 key
                case "F19": return VirtualKeyCode.F19;
                //
                // Summary:
                //     F20 key
                case "F20": return VirtualKeyCode.F20;
                //
                // Summary:
                //     F21 key
                case "F21": return VirtualKeyCode.F21;
                //
                // Summary:
                //     F22 key
                case "F22": return VirtualKeyCode.F22;
                //
                // Summary:
                //     F23 key
                case "F23": return VirtualKeyCode.F23;
                //
                // Summary:
                //     F24 key
                case "F24": return VirtualKeyCode.F24;
                //
                // Summary:
                //     NUM LOCK key
                case "NUMLOCK": return VirtualKeyCode.NUMLOCK;
                //
                // Summary:
                //     SCROLL LOCK key
                case "SCROLL": return VirtualKeyCode.SCROLL;
                //
                // Summary:
                //     Left SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
                case "LSHIFT": return VirtualKeyCode.LSHIFT;
                //
                // Summary:
                //     Right SHIFT key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
                case "RSHIFT": return VirtualKeyCode.RSHIFT;
                //
                // Summary:
                //     Left CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
                case "LCONTROL": return VirtualKeyCode.LCONTROL;
                //
                // Summary:
                //     Right CONTROL key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
                case "RCONTROL": return VirtualKeyCode.RCONTROL;
                //
                // Summary:
                //     Left MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
                case "LMENU": return VirtualKeyCode.LMENU;
                //
                // Summary:
                //     Right MENU key - Used only as parameters to GetAsyncKeyState() and GetKeyState()
                case "RMENU": return VirtualKeyCode.RMENU;
                //
                // Summary:
                //     Windows 2000/XP: Browser Back key
                case "BROWSER_BACK": return VirtualKeyCode.BROWSER_BACK;
                //
                // Summary:
                //     Windows 2000/XP: Browser Forward key
                case "BROWSER_FORWARD": return VirtualKeyCode.BROWSER_FORWARD;
                //
                // Summary:
                //     Windows 2000/XP: Browser Refresh key
                case "BROWSER_REFRESH": return VirtualKeyCode.BROWSER_REFRESH;
                //
                // Summary:
                //     Windows 2000/XP: Browser Stop key
                case "BROWSER_STOP": return VirtualKeyCode.BROWSER_STOP;
                //
                // Summary:
                //     Windows 2000/XP: Browser Search key
                case "BROWSER_SEARCH": return VirtualKeyCode.BROWSER_SEARCH;
                //
                // Summary:
                //     Windows 2000/XP: Browser Favorites key
                case "BROWSER_FAVORITES": return VirtualKeyCode.BROWSER_FAVORITES;
                //
                // Summary:
                //     Windows 2000/XP: Browser Start and Home key
                case "BROWSER_HOME": return VirtualKeyCode.BROWSER_HOME;
                //
                // Summary:
                //     Windows 2000/XP: Volume Mute key
                case "VOLUME_MUTE": return VirtualKeyCode.VOLUME_MUTE;
                //
                // Summary:
                //     Windows 2000/XP: Volume Down key
                case "VOLUME_DOWN": return VirtualKeyCode.VOLUME_DOWN;
                //
                // Summary:
                //     Windows 2000/XP: Volume Up key
                case "VOLUME_UP": return VirtualKeyCode.VOLUME_UP;
                //
                // Summary:
                //     Windows 2000/XP: Next Track key
                case "MEDIA_NEXT_TRACK": return VirtualKeyCode.MEDIA_NEXT_TRACK;
                //
                // Summary:
                //     Windows 2000/XP: Previous Track key
                case "MEDIA_PREV_TRACK": return VirtualKeyCode.MEDIA_PREV_TRACK;
                //
                // Summary:
                //     Windows 2000/XP: Stop Media key
                case "MEDIA_STOP": return VirtualKeyCode.MEDIA_STOP;
                //
                // Summary:
                //     Windows 2000/XP: Play/Pause Media key
                case "MEDIA_PLAY_PAUSE": return VirtualKeyCode.MEDIA_PLAY_PAUSE;
                //
                // Summary:
                //     Windows 2000/XP: Start Mail key
                case "LAUNCH_MAIL": return VirtualKeyCode.LAUNCH_MAIL;
                //
                // Summary:
                //     Windows 2000/XP: Select Media key
                case "LAUNCH_MEDIA_SELECT": return VirtualKeyCode.LAUNCH_MEDIA_SELECT;
                //
                // Summary:
                //     Windows 2000/XP: Start Application 1 key
                case "LAUNCH_APP1": return VirtualKeyCode.LAUNCH_APP1;
                //
                // Summary:
                //     Windows 2000/XP: Start Application 2 key
                case "LAUNCH_APP2": return VirtualKeyCode.LAUNCH_APP2;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the ';:' key
                case "OEM_1": return VirtualKeyCode.OEM_1;
                //
                // Summary:
                //     Windows 2000/XP: For any country/region, the '+' key
                case "OEM_PLUS": return VirtualKeyCode.OEM_PLUS;
                //
                // Summary:
                //     Windows 2000/XP: For any country/region, the ',' key
                case "OEM_COMMA": return VirtualKeyCode.OEM_COMMA;
                //
                // Summary:
                //     Windows 2000/XP: For any country/region, the '-' key
                case "OEM_MINUS": return VirtualKeyCode.OEM_MINUS;
                //
                // Summary:
                //     Windows 2000/XP: For any country/region, the '.' key
                case "OEM_PERIOD": return VirtualKeyCode.OEM_PERIOD;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the '/?' key
                case "OEM_2": return VirtualKeyCode.OEM_2;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the '`~' key
                case "OEM_3": return VirtualKeyCode.OEM_3;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the '[{' key
                case "OEM_4": return VirtualKeyCode.OEM_4;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the '\|' key
                case "OEM_5": return VirtualKeyCode.OEM_5;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the ']}' key
                case "OEM_6": return VirtualKeyCode.OEM_6;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
                //     For the US standard keyboard, the 'single-quote/double-quote' key
                case "OEM_7": return VirtualKeyCode.OEM_7;
                //
                // Summary:
                //     Used for miscellaneous characters; it can vary by keyboard.
                case "OEM_8": return VirtualKeyCode.OEM_8;
                //
                // Summary:
                //     Windows 2000/XP: Either the angle bracket key or the backslash key on the RT
                //     102-key keyboard
                case "OEM_102": return VirtualKeyCode.OEM_102;
                //
                // Summary:
                //     Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
                case "PROCESSKEY": return VirtualKeyCode.PROCESSKEY;
                //
                // Summary:
                //     Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
                //     The PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard
                //     input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN,
                //     and WM_KEYUP
                case "PACKET": return VirtualKeyCode.PACKET;
                //
                // Summary:
                //     Attn key
                case "ATTN": return VirtualKeyCode.ATTN;
                //
                // Summary:
                //     CrSel key
                case "CRSEL": return VirtualKeyCode.CRSEL;
                //
                // Summary:
                //     ExSel key
                case "EXSEL": return VirtualKeyCode.EXSEL;
                //
                // Summary:
                //     Erase EOF key
                case "EREOF": return VirtualKeyCode.EREOF;
                //
                // Summary:
                //     Play key
                case "PLAY": return VirtualKeyCode.PLAY;
                //
                // Summary:
                //     Zoom key
                case "ZOOM": return VirtualKeyCode.ZOOM;
                //
                // Summary:
                //     Reserved
                case "NONAME": return VirtualKeyCode.NONAME;
                //
                // Summary:
                //     PA1 key
                case "PA1": return VirtualKeyCode.PA1;
                //
                // Summary:
                //     Clear key
                case "OEM_CLEAR": return VirtualKeyCode.OEM_CLEAR;

                default: return VirtualKeyCode.NONAME;
            }
        }
    }
}

