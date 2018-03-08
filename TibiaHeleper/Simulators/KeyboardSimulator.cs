using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Simulators
{
    class KeyboardSimulator
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;
        const UInt32 WM_CHAR = 0x0102;

        static Dictionary<string, int> DButton;
        static Process proc;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        static KeyboardSimulator()
        {
            proc = GetData.getProcess();
            DButton = new Dictionary<string, int>();
            KeyAddresses.assignKeys(DButton);
        }

        public static void Simulate(string action)
        {
            action = action.ToUpper();
            if (action.IndexOf('+') != -1 || (action.IndexOf('F') == 0 && (action[1] >= 49 && action[1] <= 57) && action.Length <= 3)) // 49 is '1' like f1 and 57 is '9' length of f* keys is max 3 "f10"
                Press(action);
            else
                Message(action);
        }

        public static void Press(string button)
        {
            button = button.Replace(" ", "");
            button = button.ToUpper();
            if (button.IndexOf("+") == -1)
            {
                PostMessage(proc.MainWindowHandle, WM_CHAR, DButton[button], 0);
            }
            else
            {
                string[] combination = button.Split('+');
                foreach (string singleButton in combination)
                {
                    PostMessage(proc.MainWindowHandle, WM_KEYDOWN, DButton[singleButton], 0);
                }
                foreach (string singleButton in combination)
                {
                    PostMessage(proc.MainWindowHandle, WM_KEYUP, DButton[singleButton], 0);
                }

            }

        }

        public static void Message(string text)
        {
            text = text.ToUpper();
            PostMessage(proc.MainWindowHandle, WM_CHAR, DButton["ENTER"], 0);
            foreach (char letter in text)
            {
                PostMessage(proc.MainWindowHandle, WM_CHAR, DButton[letter.ToString()], 0);
            }
            PostMessage(proc.MainWindowHandle, WM_CHAR, DButton["ENTER"], 0);
        }
    }
}
