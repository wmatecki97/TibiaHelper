using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Keyboard
{
    class KeyboardSimulator
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_KEYUP = 0x0101;

        static Dictionary<string, int> DButton;
        static Process proc;


        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        static KeyboardSimulator()
        {
            proc = GetData.getProcess();

            DButton = new Dictionary<string, int>();
            Keyboard.KeysAdresses.assignKeys(DButton);

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
    }
}
