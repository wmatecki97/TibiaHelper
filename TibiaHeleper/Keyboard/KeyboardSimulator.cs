using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using WindowsInput;


namespace TibiaHeleper.Keyboard
{
    class KeyboardSimulator
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;
        const UInt32 WM_CHAR = 0x0102;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag


        static Dictionary<string, int> DButton;
        static Process proc;

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        static KeyboardSimulator()
        {
            proc = GetData.getProcess();
            DButton = new Dictionary<string, int>();
            KeysAdresses.assignKeys(DButton);

        }

        public static void Press(string button)
        {
            button = button.Replace(" ", "");
            button = button.ToUpper();
            if (button.IndexOf("+") == -1)
            {
                PostMessage(proc.MainWindowHandle, WM_KEYDOWN, DButton[button], 0);
                PostMessage(proc.MainWindowHandle, WM_KEYUP, DButton[button], 0);
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

        private static bool isSpecialLetter(char letter)
        {
            switch (letter)
            {
                case '\'': return true;
            }
            return false;
        }

        public static void Message(string text)
        {

            text = text.ToUpper();
            PostMessage(proc.MainWindowHandle, WM_KEYDOWN, DButton["ENTER"], 0);
            foreach (char letter in text)
            {
               if(isSpecialLetter(letter))//if letter needs shift like " is from ' and shift
                    keybd_event((byte)DButton["SHIFT"], 0, KEYEVENTF_EXTENDEDKEY, 0);

                PostMessage(proc.MainWindowHandle, WM_KEYDOWN, DButton[letter.ToString()], 0);

                if(isSpecialLetter(letter))
                    keybd_event((byte)DButton["SHIFT"], 0, KEYEVENTF_KEYUP, 0);


            }
            PostMessage(proc.MainWindowHandle, WM_KEYDOWN, DButton["ENTER"], 0);

        }

        
    }
}
