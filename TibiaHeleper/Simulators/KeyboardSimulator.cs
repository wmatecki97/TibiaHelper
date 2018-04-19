using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Simulators
{
    class KeyboardSimulator
    {
        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint WM_CHAR = 0x0102;

        static Dictionary<string, int> DButton;
        static Process proc;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        static KeyboardSimulator()
        {
            proc = GetData.getProcess();
            DButton = new Dictionary<string, int>();
            KeyAddresses.assignKeys(DButton);
        }

        public static void Simulate(string action)
        {
            string text = action;
            action = action.ToUpper();
            if (action.IndexOf('+') != -1 || (action.IndexOf('F') == 0 && (action[1] >= 49 && action[1] <= 57) && action.Length <= 3)) // 49 is '1' like f1 and 57 is '9' length of f* keys is max 3 "f10"
                Press(action);
            else
            {
                string userActualInput = GetData.ActualInput;
                deleteActualInput(userActualInput.Length);
                Message(text);
                Message(userActualInput);
            }
        }

        public static void Press(string button)
        {
            SimulatorSynchronisation.semaphore.WaitOne();

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

            SimulatorSynchronisation.semaphore.Release();

        }

        public static void Message(string text)
        {
            SimulatorSynchronisation.semaphore.WaitOne();

            PostMessage(proc.MainWindowHandle, WM_CHAR, DButton["ENTER"], 0);
            foreach (char letter in text)
            {
                PostMessage(proc.MainWindowHandle, WM_CHAR, letter, 0);
            }
            PostMessage(proc.MainWindowHandle, WM_CHAR, DButton["ENTER"], 0);

            SimulatorSynchronisation.semaphore.Release();

        }

        private static void deleteActualInput(int lettersToDelete)
        {
            for (int i = 0; i < lettersToDelete; i++)
                PostMessage(proc.MainWindowHandle, WM_CHAR, DButton["BACKSPACE"], 0);
            //TO IMPLEMENT
        }
    }
}
